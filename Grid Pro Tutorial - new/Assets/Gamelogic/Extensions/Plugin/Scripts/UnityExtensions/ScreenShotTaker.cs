using System;
using System.Collections;
using UnityEngine;

namespace Gamelogic.Extensions
{
	/// <summary>
	/// A component that makes it easy to take screenshots, usually for development purposes.
	/// </summary>
	/// <seealso cref="Gamelogic.Extensions.GLMonoBehaviour" />
	public sealed class ScreenshotTaker : Singleton<ScreenshotTaker>
	{
		#region Configuration
		[Tooltip("The key to use for taking a screenshot.")]
		[SerializeField]
		private KeyCode screenshotKey = KeyCode.Q;

		[Tooltip("The scale at which to take the screen shot.")]
		[Positive]
		[SerializeField]
		private int scale = 1;

		[Tooltip("The fist part of the file name")]
		[SerializeField]
		private string fileNamePrefix = "screen_";

		[Tooltip("Set this to true to have screenshots taken periodically and specify the interval in seconds.")]
		[SerializeField]
		private OptionalFloat automaticScreenshotInterval = new OptionalFloat { UseValue = false, Value = 60f};

		[Tooltip("Objects to disable when taking a screenshot.")]
		[SerializeField]
		private GameObject[] dirtyObjects = new GameObject[0];
		#endregion

		#region Unity Messages
		public void Start()
		{
			if (automaticScreenshotInterval.UseValue)
			{
				if (dirtyObjects.Length > 0)
				{
					InvokeRepeating(TakeCleanImpl, automaticScreenshotInterval.Value, automaticScreenshotInterval.Value);
				}
				else
				{
					InvokeRepeating(TakeImpl, automaticScreenshotInterval.Value, automaticScreenshotInterval.Value);
				}
			}
		}

		public void Update()
		{
			if (Input.GetKeyDown(screenshotKey))
			{
				if (dirtyObjects.Length > 0)
				{
					TakeClean();
				}
				else
				{
					Take();
				}
			}
		}
		#endregion

		#region Public Methods
		public static void Take()
		{
			Instance.TakeImpl();
		}

		public static void TakeClean()
		{
			Instance.TakeCleanImpl();
		}
		#endregion

		#region Implementation
		private void TakeCleanImpl()
		{
			StartCoroutine(TakeCleanEnumerator());
		}

		private IEnumerator TakeCleanEnumerator()
		{
			foreach (var obj in dirtyObjects)
			{
				obj.SetActive(false);
			}

			yield return new WaitForEndOfFrame();

			TakeImpl();

			yield return new WaitForEndOfFrame();

			foreach (var obj in dirtyObjects)
			{
				obj.SetActive(true);
			}
		}

		private void TakeImpl()
		{
			string path = fileNamePrefix + DateTime.Now.Ticks + ".png";
			Application.CaptureScreenshot(path, scale);
		}
		#endregion
	}
}