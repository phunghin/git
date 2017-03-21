using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;
using System.Collections;
using Random = UnityEngine.Random;

public class MainGame : SceneBehaviour, IBoardEventListener
{
	private static readonly float BoardPadding = 0.2f;

    private static int n;  

	/// <summary>
	/// The cell prefab.
	/// </summary>
	public GameObject cellPrefab;

	/// <summary>
	/// The leaderboard prefab.
	/// </summary>
	public GameObject leaderboardPrefab;

	/// <summary>
	/// The how to play prefab.
	/// </summary>
	public GameObject howToPlayPrefab;

	/// <summary>
	/// The score text.
	/// </summary>
	public Text scoreText;

	/// <summary>
	/// The best text.
	/// </summary>
	public Text bestText;

	/// <summary>
	/// The next transform.
	/// </summary>
	public Transform nextTransform;

	/// <summary>
	/// The next text.
	/// </summary>
	public Text nextText;

	// The background
	[SerializeField]
	private Transform _background;

	// The board
	[SerializeField]
	private Board _board;

	[SerializeField]
	private Transform _boardTop;

	[SerializeField]
	private Transform _boardBottom;

	[Header("Game Over")]

	public GameObject gameOver;
	public Transform gameOverPopup;
	public Text gameOverScoreText;
	public Text gameOverBestText;
	public GameObject gameOverNew;

	// The row
	private int _row;

	// The column
	private int _column;

	// The array of numbers
	private int[,] _numbers;

	// The array of cells
	private Cell[,] _cells;

	// The best score
	private int _bestScore;

	// The score
	private int _score;

	// The next number
	private int _nextNumber;

	// True if new high score
	private bool _isNewHighScore;

	// True if game over
	private bool _isGameOver;

	// The user data
	private UserData _userData;

	protected override string Name
	{
		get { return "Main Game"; }
	}

	protected override void Start()
	{
		base.Start();

		// Fit background
		_background.SetScale(ResolutionHelper.GetNoBorderScale());

		// Calculate board size
		float boardWidth  = Camera.main.GetWidth() - BoardPadding * 2;
		float boardHeight = _boardTop.position.y - _boardBottom.position.y;

		_board.Construct(boardWidth, boardHeight);
		_board.transform.position = new Vector3(0, (_boardBottom.position.y + _boardTop.position.y) * 0.5f, 0);

		// Set board event listener
		_board.Listener = this;

		float scaleFactor = _board.CellSize / 1.28f;

		// Set cell scale factor
		Cell.scaleFactor = scaleFactor;

		// Set next position
		nextTransform.position = new Vector3(-_board.Width * 0.5f, _board.transform.position.y + _board.Height * 0.5f + 0.1f * scaleFactor, 0);

		// Set next scale
		nextTransform.SetScale(scaleFactor);

		// Hide board top and bottom
		_boardTop.Hide();
		_boardBottom.Hide();

		// Set row
		_row = _board.Row;

		// Set column
		_column = _board.Column;

		// Create array of numbers
		_numbers = new int[_row, _column];

		// Create array of cells
		_cells = new Cell[_row, _column];

		for (int i = 0; i < _row; i++)
		{
			for (int j = 0; j < _column; j++)
			{
				_cells[i, j] = AddCell(i, j, GetNextNumber());
			}
		}

		// Set user data
		_userData = UserData.Instance;

		// Set best score
		_bestScore = _userData.BestScore;

		// Update text
		bestText.text = _bestScore.ToString();

		// Sync Facebook score
		Manager.Instance.SyncFacebookScore(() => {
			// Set best score
			_bestScore = _userData.BestScore;

			// Update text
			bestText.text = _bestScore.ToString();
		});

		if (!GamePrefs.isAdsRemoved)
		{
			// Load ads config
			Manager.Instance.LoadURL("http://m3.cubicer.com/api/ads.config.php", (www) => {
				if (string.IsNullOrEmpty(www.error))
				{
					var dict = www.text.ToDictionary();

					if (dict.Count > 0)
					{
						GamePrefs.adsDecision.Construct(dict);
//						Log.Debug(GamePrefs.adsDecision.ToString());
					}
				}
				else
				{
					Log.Debug("Load ads config error: " + www.error);
				}
			});
		}

		GamePrefs.isVisitedMainGame = true;

		// Play
		Play();

		// How to play
		if (!_userData.PlayedHowToPlay)
		{
			// Disable interaction
			SetInteractable(false);

			// Disable board
			_board.Interactable = false;

			Invoke("ShowHowToPlay", 1.0f);
		}
	}

	void ShowHowToPlay()
	{
		Canvas canvas = FindObjectOfType<Canvas>();

		GameObject howToPlay = howToPlayPrefab.CreateUI(canvas.transform);
		PopupBehaviour script = howToPlay.GetComponent<PopupBehaviour>();
		script.CloseCallback = () => {
			// Enable board
			_board.Interactable = true;
		};

		// Enable interaction
		SetInteractable(true);

		_userData.PlayedHowToPlay = true;
	}

	void OnDestroy()
	{
		// Clear pool of cells
		Cell.Clear();

		UserData.SafeSave();

		Manager.SafeHideBanner();
	}

	void Play()
	{
		// Random numbers
		int total = _row * _column;
		int n4 = Random.Range(2, 4);
		int n2 = Random.Range(4, 6);

		int[] numbers = new int[total];
		int count = 0;

		for (int i = 0; i < n4; i++)
		{
			numbers[count++] = 4;
		}

		for (int i = 0; i < n2; i++)
		{
			numbers[count++] = 2;
		}

		for (; count < total;)
		{
			numbers[count++] = 1;
		}

		// Swap numbers
		numbers.Swap();

		count = 0;

		for (int i = 0; i < _row; i++)
		{
			for (int j = 0; j < _column; j++)
			{
				// Get next number
				int number = numbers[count++];

				// Set number
				_numbers[i, j] = number;

				Cell cell = _cells[i, j];
				cell.gameObject.name = number.ToString();
				cell.transform.localPosition = _board.GetLocalPosition(i, j);
				cell.Number = number;
			}
		}

		// Clear score
		_score = 0;
		scoreText.text = "0";

		// Next number
		NextNumber();

		_isNewHighScore = false;
		_isGameOver = false;

		// Enable board
		_board.Interactable = true;

		Manager.Instance.ShowBanner();

//		Manager.Instance.FetchAd();
	}

	/// <summary>
	/// Get next number (1=50%, 2=35%, 4=15%).
	/// </summary>
	int GetNextNumber()
	{
		float rand = Random.value;

		if (rand > 0.85f) return 4;
		if (rand > 0.50f) return 2;

		return 1;
	}

	Cell AddCell(int row, int column, int number)
	{
		GameObject cellObj = Cell.Pop();

		if (cellObj == null)
		{
			cellObj = Instantiate(cellPrefab) as GameObject;
			cellObj.transform.SetParent(_board.transform);
		}

		cellObj.transform.localPosition = _board.GetLocalPosition(row, column);

		Cell cell = cellObj.GetComponent<Cell>();
		cell.Number = number;

		return cell;
	}

	void OnMerged(int number)
	{
		// Add score
		AddScore(number);

		// Check if game over
		CheckGameOver();
	}

	void AddScore(int score)
	{
		// Increase score
		_score += score;
		scoreText.text = _score.ToString();

		if (_score > _bestScore)
		{
			_bestScore = _score;
			_userData.BestScore = _bestScore;

			bestText.text = _bestScore.ToString();

			_isNewHighScore = true;
		}
	}

	void NextNumber()
	{
		_nextNumber = GetNextNumber();
		nextText.text = _nextNumber.ToString();
	}

	bool CheckGameOver()
	{
		for (int i = 0; i < _row; i++)
		{
			for (int j = 0; j < _column - 1; j++)
			{
				if (_numbers[i, j] == _numbers[i, j + 1])
				{
					return false;
				}
			}
		}

		for (int j = 0; j < _column; j++)
		{
			for (int i = 0; i < _row - 1; i++)
			{
				if (_numbers[i, j] == _numbers[i + 1, j])
				{
					return false;
				}
			}
		}

		OnGameOver();

		return true;
	}

	void OnGameOver()
	{
		if (_isGameOver) return;

		SoundManager.PlaySound(SoundID.GameOver);

		// Game over
		_isGameOver = true;

		// Disable board
		_board.Interactable = false;

		// Save data
		UserData.SafeSave();

		// Track score
		Manager.Instance.LogEvent("Score", "Get", GetScoreRange(_score));

		// Send score to cubicer
		if (_isNewHighScore)
		{
			Manager.Instance.SendScore(_score);
		}

		Invoke("OnGameOverCallback", 0.5f);
	}

	void OnGameOverCallback()
	{
		Manager.Instance.SyncFacebookScore(ShowGameOver);
	}

	void ShowGameOver()
	{
		gameOver.Show();

		gameOver.PlayAction(FadeAction.FadeTo(0.75f, 0.5f), () => {
			// Score
			gameOverScoreText.text = _score.ToString();

			// Best
			gameOverBestText.text = _bestScore.ToString();

			// New
			gameOverNew.SetActive(_isNewHighScore);

			// Show popup
			gameOverPopup.Show();

			if (!GamePrefs.isAdsRemoved)
			{
				// Hide banner
				Manager.SafeHideBanner();

				// Get random ads
				AdsType adsType = GamePrefs.adsDecision.GetAdsType(_score);

				if (adsType != AdsType.None)
				{
					// Disable interaction
					SetInteractable(false);

					// Show ads
					StartCoroutine(ShowAds(adsType));
				}
			}
		});
	}

	IEnumerator ShowAds(AdsType adsType)
	{
		yield return new WaitForSeconds(1.0f);

		if (adsType == AdsType.AdMob)
		{
			Manager.Instance.ShowAdMobInterstitial();
		}
		else if (adsType == AdsType.NativeX)
		{
			Manager.Instance.ShowNativeXInterstitial();
		}
		else if (adsType == AdsType.Promotion)
		{
			Manager.Instance.ShowPromotion();
		}

		// Enable interaction
		SetInteractable(true);
	}

	string GetScoreRange(int score)
	{
		if (score < 100) return "Less than 100";
		if (score < 200) return "100-200";
		if (score < 500) return "200-500";
		if (score < 1000) return "500-1000";
		if (score < 2000) return "1000-2000";
		if (score < 5000) return "2000-5000";
		if (score < 10000) return "5000-10000";
		if (score < 20000) return "10000-20000";
		if (score < 50000) return "20000-50000";
		if (score < 100000) return "50000-100000";

		return "Greater than 100000";
	}

	void UpdateScore()
	{
		if (_isNewHighScore)
		{
			// Track score
			Manager.Instance.LogEvent("Score", "Get", GetScoreRange(_score));

			// Send score to cubicer
			Manager.Instance.SendScore(_score);

			// Sync FB score
			Manager.Instance.SyncFacebookScore();
		}
	}

	public void NewGame()
	{
		OnButtonClick("New Game");

		// Check if played
		if (_score > 0)
		{
			// Disable board
			_board.Interactable = false;

			Manager.Instance.ShowConfirm(Constants.ConfirmNewGame, (yes) => {
				if (yes)
				{
					UpdateScore();

					Play();
				}
				else
				{
					// Enable board
					_board.Interactable = true;
				}
			});
		}
		else
		{
			Play();
		}
	}

	public void Menu()
	{
		OnButtonClick("Menu");

		// Check if played
		if (_score > 0)
		{
			// Disable board
			_board.Interactable = false;

			Manager.Instance.ShowConfirm(Constants.ConfirmMenu, (yes) => {
				if (yes)
				{
					UpdateScore();

					TransitionManager.Instance.TransitionScene(Constants.Menu);
				}
				else
				{
					// Enable board
					_board.Interactable = true;
				}
			});
		}
		else
		{
			TransitionManager.Instance.TransitionScene(Constants.Menu);
		}
	}

	public void Share()
	{
		OnButtonClick("Share");

		if (Helper.IsOnline())
		{
			if (FB.IsLoggedIn)
			{
				OnShare();
			}
			else
			{
				FBHelper.LogIn((error) => {
					if (string.IsNullOrEmpty(error))
					{
						OnShare();
					}
					else
					{
						Manager.Instance.ShowMessage(Constants.LoginFailed);
					}
				});
			}
		}
		else
		{
			Manager.Instance.ShowMessage(Constants.NoInternetConnection);
		}
	}

	void OnShare()
	{
		FB.ShareLink(
			contentURL: new Uri("http://m3.cubicer.com/api/redirect.php?name=1248-numberchallenge"),
			contentTitle: string.Format("I get score {0}.", _score),
			contentDescription: "Download this game on App Store and Play Store!",
			photoURL: new Uri("http://m3.cubicer.com/api/images/icon-1248.png")
		);
	}

	public void Replay()
	{
		OnButtonClick("Replay");

		// Hide popup
		gameOverPopup.Hide();
		gameOver.SetAlpha(0);
		gameOver.Hide();

		// Play
		Play();
	}

	public void Rate()
	{
		OnButtonClick("Rate");

		#if UNITY_ANDROID
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.vxsolution.numberchallenge");
		#else
		Application.OpenURL("https://itunes.apple.com/app/id1123320889");
		#endif
	}

	public void ShowLeaderboard()
	{
		OnButtonClick("Leaderboard");

		Leaderboard.Show(leaderboardPrefab, gameOver.transform.parent);
	}

	public void Home()
	{
		OnButtonClick("Home");

		TransitionManager.Instance.TransitionScene(Constants.Menu);
	}

	#region IBoardEventListener

	public void OnCellPressed(int row, int column)
	{
		_cells[row, column].OnPressed();
	}

	public void OnCellDraggedLeft(int row, int column)
	{
		Cell cell = _cells[row, column];

		if (column == 0)
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		if (_numbers[row, column - 1] != _numbers[row, column])
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		SoundManager.PlaySound(SoundID.Correct);

		// Get current number
		int number = _numbers[row, column];

		// Add number
		_numbers[row, column] += number;

		// Shift numbers
		_numbers.ShiftLeft(row, column - 1, _nextNumber);

		//
		Cell left = _cells[row, column - 1];
		Cell last = AddCell(row, _column - 1, _nextNumber);

		NextNumber();

		// Merge
		cell.MergeLeft(left, () => { OnMerged(number); });

		// Move left
		for (int i = column + 1; i < _column; i++)
		{
			_cells[row, i].MoveLeft(_board.GetLocalPosition(row, i - 1));
		}

		// Appear
		last.OnAppear();

		// Shift cells
		_cells.ShiftLeft(row, column - 1, last);
	}

	public void OnCellDraggedRight(int row, int column)
	{
		Cell cell = _cells[row, column];

		if (column == _column - 1)
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		if (_numbers[row, column + 1] != _numbers[row, column])
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		SoundManager.PlaySound(SoundID.Correct);

		// Get current number
		int number = _numbers[row, column];

		// Add number
		_numbers[row, column] += number;

		// Shift numbers
		_numbers.ShiftRight(row, column + 1, _nextNumber);

		//
		Cell right = _cells[row, column + 1];
		Cell last = AddCell(row, 0, _nextNumber);

		NextNumber();

		// Merge
		cell.MergeRight(right, () => { OnMerged(number); });

		// Move right
		for (int i = column - 1; i >= 0; i--)
		{
			_cells[row, i].MoveRight(_board.GetLocalPosition(row, i + 1));
		}

		// Appear
		last.OnAppear();

		// Shift cells
		_cells.ShiftRight(row, column + 1, last);
	}

	public void OnCellDraggedUp(int row, int column)
	{
		Cell cell = _cells[row, column];

		if (row == _row - 1)
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		if (_numbers[row + 1, column] != _numbers[row, column])
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		SoundManager.PlaySound(SoundID.Correct);

		// Get current number
		int number = _numbers[row, column];

		// Add number
		_numbers[row, column] += number;

		// Shift numbers
		_numbers.ShiftDown(row + 1, column, _nextNumber);

		//
		Cell top = _cells[row + 1, column];
		Cell last = AddCell(0, column, _nextNumber);

		NextNumber();

		// Merge
		cell.MergeUp(top, () => { OnMerged(number); });

		// Move up
		for (int i = row - 1; i >= 0; i--)
		{
			_cells[i, column].MoveUp(_board.GetLocalPosition(i + 1, column));
		}

		// Appear
		last.OnAppear();

		// Shift cells
		_cells.ShiftDown(row + 1, column, last);
	}

	public void OnCellDraggedDown(int row, int column)
	{
		Cell cell = _cells[row, column];

		if (row == 0)
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		if (_numbers[row - 1, column] != _numbers[row, column])
		{
			SoundManager.PlaySound(SoundID.Wrong);
			cell.OnCancelled();
			return;
		}

		SoundManager.PlaySound(SoundID.Correct);

		// Get current number
		int number = _numbers[row, column];

		// Add number
		_numbers[row, column] += number;

		// Shift numbers
		_numbers.ShiftUp(row - 1, column, _nextNumber);

		//
		Cell bottom = _cells[row - 1, column];
		Cell last = AddCell(_row - 1, column, _nextNumber);

		NextNumber();

		// Merge
		cell.MergeDown(bottom, () => { OnMerged(number); });

		// Move down
		for (int i = row + 1; i < _row; i++)
		{
			_cells[i, column].MoveDown(_board.GetLocalPosition(i - 1, column));
		}

		// Appear
		last.OnAppear();

		// Shift cells
		_cells.ShiftUp(row - 1, column, last);
	}

	public void OnCellReleased(int row, int column)
	{
		_cells[row, column].OnReleased();
	}

	public void OnBoardPressed()
	{
		
	}

	public void OnPressedOutOfBoard()
	{
		
	}

	#endregion

	void OnDrawGizmos()
	{
		float halfScreenWidth  = Camera.main.GetOrthographicWidth();
		float halfScreenHeight = Camera.main.orthographicSize;

		float left = -halfScreenWidth + BoardPadding;
		float right = -left;
		float bottom = -halfScreenHeight;
		float top = -bottom;

		if (_boardBottom != null)
		{
			bottom = _boardBottom.position.y;
		}

        if (_boardTop != null)
        {
            top = _boardTop.position.y;

        }       

        Helper.DrawRect(left, top, right, bottom, Color.green);
	}
		
	public void DoGUI()
	{
		GUI.enabled = !_isGameOver;

		if (GUILayout.Button("Add score"))
		{
			AddScore(100);
		}

//		if (GUILayout.Button("Cheat"))
//		{
//			_score = 0;
//
//			int[,] numbers = new int[4, 4]
//			{
//				{2, 1, 2, 1},
//				{4, 2, 1, 2},
//				{8, 16, 32, 64},
//				{1024, 512, 256, 128}
//			};
//
//			for (int i = 0; i < _row; i++)
//			{
//				for (int j = 0; j < _column; j++)
//				{
//					int number = numbers[i, j];
//					_cells[_row - 1 - i, j].Number = number;
//					_score += number;
//				}
//			}
//
//			_score -= 7;
//			_bestScore = _score;
//			_isNewHighScore = true;
//
//			scoreText.text = _score.ToString();
//			bestText.text = _bestScore.ToString();
//		}

		if (GUILayout.Button("Game Over"))
		{
			OnGameOver();
		}

		GUI.enabled = true;

		if (!GamePrefs.isAdsRemoved)
		{
			GamePrefs.adsDecision.OnGUI();
		}
	}
}
