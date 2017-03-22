using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGame : SimpleSingleton<MainGame>
{
    public SpriteFactory SpriteFactory;
    public GameObject MoveLimit;
    public GameObject CountScore;
    public GameObject[] CellTarget = new GameObject[4];

    public int LevelSlect;
    public GridController _grid;


    private Text[] _textCelltarget = new Text[4];
    Text _textMove;
    Text _textScore;

    private void Start()
    {
                
         DefaultCanvas();
        _grid.LoadMap(Manager.Instance.LevelChose);
    }

    public void UpdateCanvas(int limit = 0, int[] limitcell = null,int score = 0)
    {
        _textMove.text = "Move :" + limit.ToString();
       // _textScore.text = "Score :" + score.ToString();

        if (limitcell == null) return;
        for (int i = 0; i < limitcell.Length; i++)
        {
            _textCelltarget[i].text = limitcell[i].ToString();
        }
    }

    public void UpdateLImit(int limit)
    {
        _textMove.text = "Move :" + limit.ToString();
    }

    public void UpdateScore(int score)
    {
       // _textScore.text = "Score :" + score.ToString();
    }

    [SerializeField]
    CellData[] _celltarget;
    [SerializeField]
    int[] _limittarget;

    public void UpdateCellTarget(int id,CellType type)
    {       
        for (int i = 0; i < _celltarget.Length; i++)
        {                         
            if(_celltarget[i].Id == id && checkCellType(type))
            {
               if(_limittarget[i] > 0)
                {
                    _limittarget[i]--;
                    _textCelltarget[i].text = _limittarget[i].ToString();
                }
                              
                return;
            }
        }
    }

    bool checkCellType(CellType type)
    {
        for (int i = 0; i < _celltarget.Length; i++)
        {
            if(_celltarget[i].CellType == type)
            {
                return true;
            }
        }

        return false;
    }

    public void ContrucCanvas(int limit,int[] limitcell,CellData[] celltarget)
    {
        
      
        _textMove = MoveLimit.transform.GetChild(0).GetComponent<Text>();
        _textMove.text = "Move " + limit.ToString();             
      //  _textScore = CountScore.transform.GetChild(0).GetComponent<Text>();

        int count = celltarget.Length;
        _celltarget = celltarget;

        _limittarget = new int[count];

        for (int i = 0; i < count; i++)
        {
            _limittarget[i] = limitcell[i];
        }

        for (int i = 0; i < count; i++)
        {
            CellTarget[i].Show();
            CellTarget[i].GetComponent<Image>().sprite = SpriteFactory.AddSpriteCell(celltarget[i].CellType, celltarget[i].Id);

            _textCelltarget[i] = CellTarget[i].transform.GetChild(0).GetComponent<Text>();
            _textCelltarget[i].text = limitcell[i].ToString();
        }

        Vector3[] positionTarget = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            positionTarget[i] = CellTarget[i].transform.position;
            positionTarget[i].z = 0;
        }

        _grid.SetPosition = positionTarget;
    }

    void DefaultCanvas()
    {
        int count = CellTarget.Length;

        for (int i = 0; i < count; i++)
        {
            CellTarget[i].GetComponent<Image>().sprite = null;
            CellTarget[i].transform.GetChild(0).GetComponent<Text>().text = "0";
        }
    }

    public GameObject PanalGame;

    public void GameOver()
    {
        PanalGame.Show();
        PanalGame.transform.GetChild(0).GetComponent<Text>().text = "Game Over";
        StartCoroutine(LoadSceen(3));
    }

    int _star;
    int _score;
    public void NextLevel(int star, int score)
    {
        _star = star;
        _score = score;
        SaveDataUser(star, score);
        StartCoroutine(WaitForFinish());
    }

    public IEnumerator LoadSceen(float time)
    {
        yield return new WaitForSeconds(time);
        TouchManager.Instance.Enabled = true;
        SceneManager.LoadScene("NewPlay");
    }

    IEnumerator TEst()
    {
        yield return new WaitForSeconds(3);
        
        PanalGame.Show();
        Settings.Level += 1;
        PanalGame.transform.GetChild(0).GetComponent<Text>().text = "Next Level";

        StartCoroutine(LoadSceen(3));
    }

    IEnumerator WaitForFinish()
    {
        yield return new WaitForSeconds(2);
        PanalGame.Show();
       PanalGame.transform.GetChild(0).GetComponent<LevelSelectController>().contructMainGame(Manager.Instance.LevelChose, _star, _score,SpriteFactory.GetSpriteCell(_celltarget));
    }


    void SaveDataUser(int star, int score)
    {
        UserData _userdata = UserData.Instance;
        LevelUserData data = new LevelUserData(star, score);
        _userdata.InsertLevel(Manager.Instance.LevelChose,data);
        _userdata.Save();
    }

}
