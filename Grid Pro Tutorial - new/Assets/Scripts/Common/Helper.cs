using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public static class Helper
{
	//public static Direction GetDirection(float deltaX, float deltaY)
	//{
	//	float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

	//	if (angle < 0) angle += 360.0f;

	//	if (angle > 45.0f)
	//	{
	//		if (angle <= 135.0f) return Direction.Up;
	//		if (angle <= 225.0f) return Direction.Left;
	//		if (angle <= 315.0f) return Direction.Down;
	//	}

	//	return Direction.Right;
	//}

	#region Camera

	public static float GetOrthographicWidth(this Camera camera)
	{
		return camera.orthographicSize * camera.aspect;
	}

	public static float GetWidth(this Camera camera)
	{
		return camera.orthographicSize * camera.aspect * 2;
	}

	public static float GetHeight(this Camera camera)
	{
		return camera.orthographicSize * 2;
	}

	#endregion // Camera

	#region GameObject

	public static void Show(this GameObject go)
	{
		go.SetActive(true);
	}

	public static void Hide(this GameObject go)
	{
		go.SetActive(false);
	}

	public static Vector2 GetSpriteSize(this GameObject gameObject)
	{
		Vector2 size = Vector2.zero;

		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

		if (spriteRenderer != null)
		{
			Sprite sprite = spriteRenderer.sprite;

			if (sprite != null)
			{
				size = sprite.bounds.size;
			}
		}

		return size;
	}

	public static void SetRGB(this GameObject go, Vector3 rgb, bool isRecursive = false)
	{
		// Get sprite renderer
		SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();

		if (spriteRenderer != null)
		{
			spriteRenderer.SetRGB(rgb);
		}
		else
		{
			// Get renderer
			Renderer renderer = go.GetComponent<Renderer>();

			if (renderer != null)
			{
				renderer.SetRGB(rgb);
			}
			else
			{
				// Get image
				Image image = go.GetComponent<Image>();

				if (image != null)
				{
					image.SetRGB(rgb);
				}
				else
				{
					// Get text
					Text text = go.GetComponent<Text>();

					if (text != null)
					{
						text.SetRGB(rgb);
					}
				}
			}
		}

		if (isRecursive)
		{
			Transform transform = go.transform;

			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetRGB(rgb, true);
			}
		}
	}

	public static void SetAlpha(this GameObject go, float a, bool isRecursive = false)
	{
		// Get sprite renderer
		SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();

		if (spriteRenderer != null)
		{
			spriteRenderer.SetAlpha(a);
		}
		else
		{
			// Get renderer
			Renderer renderer = go.GetComponent<Renderer>();

			if (renderer != null)
			{
				renderer.SetAlpha(a);
			}
			else
			{
				// Get image
				Image image = go.GetComponent<Image>();

				if (image != null)
				{
					image.SetAlpha(a);
				}
				else
				{
					// Get text
					Text text = go.GetComponent<Text>();

					if (text != null)
					{
						text.SetAlpha(a);
					}
				}
			}
		}

		if (isRecursive)
		{
			Transform transform = go.transform;

			for (int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).gameObject.SetAlpha(a, true);
			}
		}
	}

	public static void SetRGBInChildren(this GameObject go, Vector3 rgb)
	{
		Transform transform = go.transform;

		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetRGB(rgb, true);
		}
	}

	public static void SetAlphaInChildren(this GameObject go, float a)
	{
		Transform transform = go.transform;

		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetAlpha(a, true);
		}
	}

	public static GameObject CreateUI(this GameObject prefab, Transform parent, Vector2? anchoredPosition = null)
	{
		GameObject gameObject = GameObject.Instantiate(prefab) as GameObject;
		gameObject.transform.SetParent(parent);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;

		RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

		if (rectTransform != null)
		{
			rectTransform.anchoredPosition3D = anchoredPosition ?? Vector3.zero;
		}

		Vector2 minAnchor = rectTransform.anchorMin;
		Vector2 maxAnchor = rectTransform.anchorMax;

		if ((minAnchor.x == 0 && maxAnchor.x == 1) && (minAnchor.y == 0 && maxAnchor.y == 1))
		{
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}
		else
		{
			Image image = gameObject.GetComponent<Image>();

			if (image != null)
			{
				if (image.type == Image.Type.Simple)
				{
					image.SetNativeSize();
				}
			}
		}

		return gameObject;
	}

	#endregion // GameObject

	#region Transform

	public static void TranslateX(this Transform transform, float deltaX)
	{
		Vector3 position = transform.position;
		position.x += deltaX;
		transform.position = position;
	}

	public static void TranslateY(this Transform transform, float deltaY)
	{
		Vector3 position = transform.position;
		position.y += deltaY;
		transform.position = position;
	}

    public static void TranslateZ(this Transform transform, float deltaZ)
    {
        var position = transform.localEulerAngles;

        position.z = deltaZ;
        transform.localEulerAngles = position;
    }

    public static void SetScale(this Transform transform, float scale)
	{
		transform.localScale = new Vector3(scale, scale, scale);
	}

	public static void HorizontalFlip(this Transform transform)
	{
		Vector3 scale = transform.localScale;
		scale.x *= -1.0f;
		transform.localScale = scale;
	}

	public static void VerticalFlip(this Transform transform)
	{
		Vector3 scale = transform.localScale;
		scale.y *= -1.0f;
		transform.localScale = scale;
	}

    

    

	public static void Show(this Transform transform)
	{
		transform.gameObject.SetActive(true);
	}

	public static void Hide(this Transform transform)
	{
		transform.gameObject.SetActive(false);
	}

	public static void TraverseComponentsInRootChildren<T>(this Transform transform, Action<T> callback)
	{
		int childCount = transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			T comp = child.GetComponent<T>();

			if (comp != null)
			{
				callback(comp);
			}
		}
	}

	public static void DestroyChildren(this Transform transform)
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}

	#endregion // Transform

	#region Gizmos

	public static void DrawRect(float left, float top, float right, float bottom, Color? color = null)
	{
		if (color != null)
		{
			Gizmos.color = color.Value;
		}

		Vector3 from = Vector3.zero;
		Vector3 to   = Vector3.zero;

		from.x = to.x = left;
		from.y = bottom;
		to.y   = top;
		Gizmos.DrawLine(from, to);

		from.x = to.x = right;
		Gizmos.DrawLine(from, to);

		from.x = left;
		from.y = top;
		Gizmos.DrawLine(from, to);

		from.y = to.y = bottom;
		Gizmos.DrawLine(from, to);
	}

	#endregion // Gizmos

	#region Float

	public static float GetRandom(this float variance)
	{
		return Random.Range(-variance, variance);
	}

	public static float Variance(this float f, float variance)
	{
		return variance != 0 ? f + Random.Range(-variance, variance) : f;
	}

	#endregion

	#region String

	public static int ToInt(this string s, int defaultValue = 0)
	{
		int value;

		if (int.TryParse(s, out value))
		{
			return value;
		}

		return defaultValue;
	}

	public static bool ToBool(this string s)
	{
		bool value = false;
		bool.TryParse(s, out value);

		return value;
	}

	public static float ToFloat(this string s)
	{
		float value = 0f;
		float.TryParse(s, out value);

		return value;
	}

	public static Dictionary<string, string> ToDictionary(this string s)
	{
		int length = s.Length;
		var dict = new Dictionary<string, string>();
		int from = -1;
		string key = "";
		int i = 0;
		char c;

		while (i < length)
		{
			c = s[i++];

			// Key
			if (c == '"')
			{
				if (from < 0)
				{
					from = i;
				}
				else
				{
					key = s.Substring(from, i - 1 - from);

					// Skip ':'
					while (i < length)
					{
						c = s[i++];

						if (c == ':') break;
					}

					// Skip space
					while (i < length && s[i] == ' ')
					{
						i++;
					}

					// String
					if (s[i] == '"')
					{
						from = ++i;

						while (i < length)
						{
							c = s[i++];

							if (c == '"')
							{
								dict.Add(key, s.Substring(from, i - 1 - from));
								break;
							}
						}
					}
					else
					{
						from = i;

						while (i < length)
						{
							c = s[i++];

							if (c == ',' || c == '}')
							{
								// Remove last space
								for (int j = i - 2; j >= 0; j--)
								{
									if (s[j] != ' ')
									{
										dict.Add(key, s.Substring(from, j + 1 - from));
										break;
									}
								}

								break;
							}
						}
					}

					// Next key
					from = -1;
				}
			}
		}

		return dict;
	}

	public static string GetFileName(this string path)
	{
		int p1 = path.LastIndexOf('/');
		int p2 = path.LastIndexOf('.');

		if (p2 > p1)
		{
			return path.Substring(p1 + 1, p2 - p1 - 1);
		}

		return path.Substring(p1 + 1);
	}

	#endregion // String

	#region 2-D

	public static int GetRow<T>(this T[,] a)
	{
		return a.GetUpperBound(0) + 1;
	}

	public static int GetColumn<T>(this T[,] a)
	{
		return a.GetUpperBound(1) + 1;
	}

	public static void ShiftLeft<T>(this T[,] a, int r, int c, T last)
	{
		int column = a.GetUpperBound(1) + 1;

		for (; c < column - 1; c++)
		{
			a[r, c] = a[r, c + 1];
		}

		a[r, c] = last;
	}

	public static void ShiftRight<T>(this T[,] a, int r, int c, T last)
	{
		for (; c > 0; c--)
		{
			a[r, c] = a[r, c - 1];
		}

		a[r, c] = last;
	}

	public static void ShiftUp<T>(this T[,] a, int r, int c, T last)
	{
		int row = a.GetUpperBound(0) + 1;

		for (; r < row - 1; r++)
		{
			a[r, c] = a[r + 1, c];
		}

		a[r, c] = last;
	}

	public static void ShiftDown<T>(this T[,] a, int r, int c, T last)
	{
		for (; r > 0; r--)
		{
			a[r, c] = a[r - 1, c];
		}

		a[r, c] = last;
	}

	#endregion // 2-D

	#region Vector2

	public static Vector2 Clamp01(this Vector2 v2)
	{
		return new Vector2(Mathf.Clamp01(v2.x), Mathf.Clamp01(v2.y));
	}

	public static Vector2 Variance(this Vector2 v2, float variance)
	{
		if (variance != 0)
		{
			v2.x += Random.Range(-variance, variance);
			v2.y += Random.Range(-variance, variance);
		}

		return v2;
	}

	public static Vector2 Variance(this Vector2 v2, Vector2 variance)
	{
		if (variance.x != 0)
		{
			v2.x += Random.Range(-variance.x, variance.x);
		}

		if (variance.y != 0)
		{
			v2.y += Random.Range(-variance.y, variance.y);
		}

		return v2;
	}

	public static void Clamp01(ref Vector2 v2)
	{
		v2.x = Mathf.Clamp01(v2.x);
		v2.y = Mathf.Clamp01(v2.y);
	}

	#endregion // Vector2

	#region Vector3

	public static Vector3 Clamp01(this Vector3 v3)
	{
		return new Vector3(Mathf.Clamp01(v3.x), Mathf.Clamp01(v3.y), Mathf.Clamp01(v3.z));
	}

	public static Vector3 Variance(this Vector3 v3, float variance)
	{
		if (variance != 0)
		{
			v3.x += Random.Range(-variance, variance);
			v3.y += Random.Range(-variance, variance);
			v3.z += Random.Range(-variance, variance);
		}

		return v3;
	}

	public static Vector3 Variance(this Vector3 v3, Vector3 variance)
	{
		if (variance.x != 0)
		{
			v3.x += Random.Range(-variance.x, variance.x);
		}

		if (variance.y != 0)
		{
			v3.y += Random.Range(-variance.y, variance.y);
		}

		if (variance.z != 0)
		{
			v3.z += Random.Range(-variance.z, variance.z);
		}

		return v3;
	}

	public static Vector3 VarianceXY(this Vector3 v3, float variance)
	{
		return variance != 0 ? new Vector3(v3.x + Random.Range(-variance, variance), v3.y + Random.Range(-variance, variance), v3.z) : v3;
	}

	public static void Clamp01(ref Vector3 v3)
	{
		v3.x = Mathf.Clamp01(v3.x);
		v3.y = Mathf.Clamp01(v3.y);
		v3.z = Mathf.Clamp01(v3.z);
	}

	#endregion // Vector3

	#region Dictionary

	public static int GetInt(this Dictionary<string, string> dict, string key, int defaultValue = 0)
	{
		if (dict.ContainsKey(key))
		{
			return dict[key].ToInt(defaultValue);
		}

		return defaultValue;
	}

	#endregion // Dictionary

	#region Color

	public static Vector3 RGB(this Color color)
	{
		return new Vector3(color.r, color.g, color.b);
	}

	public static Color Reverse(this Color color)
	{
		return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b, color.a);
	}

	#endregion // Color

	#region Renderer

	public static void SetRGB(this Renderer renderer, Vector3 rgb)
	{
		Material material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;

		if (material != null)
		{
			Color color = material.color;
			color.r = rgb.x;
			color.g = rgb.y;
			color.b = rgb.z;

			material.color = color;
		}
	}

	public static void SetAlpha(this Renderer renderer, float a)
	{
		Material material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;

		if (material != null)
		{
			Color color = material.color;
			color.a = a;

			material.color = color;
		}
	}

	public static void SetColor(this Renderer renderer, Color color)
	{
		Material material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;

		if (material != null)
		{
			material.color = color;
		}
	}

	public static void SetTexture(this Renderer renderer, Texture texture)
	{
		Material material = Application.isPlaying ? renderer.material : renderer.sharedMaterial;

		if (material != null)
		{
			material.mainTexture = texture;
		}
	}

	#endregion // Renderer

	#region SpriteRenderer

	public static void SetRGB(this SpriteRenderer renderer, Vector3 rgb)
	{
		Color color = renderer.color;
		color.r = rgb.x;
		color.g = rgb.y;
		color.b = rgb.z;

		renderer.color = color;
	}

	public static void SetAlpha(this SpriteRenderer renderer, float alpha)
	{
		Color color = renderer.color;
		color.a = alpha;

		renderer.color = color;
	}

	#endregion // SpriteRenderer

	#region Image

	public static void SetRGB(this Image image, Vector3 rgb)
	{
		Color color = image.color;
		color.r = rgb.x;
		color.g = rgb.y;
		color.b = rgb.z;

		image.color = color;
	}

	public static void SetAlpha(this Image image, float alpha)
	{
		Color color = image.color;
		color.a = alpha;

		image.color = color;
	}

	#endregion

	#region Text

	public static void SetRGB(this Text text, Vector3 rgb)
	{
		Color color = text.color;
		color.r = rgb.x;
		color.g = rgb.y;
		color.b = rgb.z;

		text.color = color;
	}

	public static void SetAlpha(this Text text, float alpha)
	{
		Color color = text.color;
		color.a = alpha;

		text.color = color;
	}

	#endregion

	#region Texture2D

	public static void Save(this Texture2D texture, string path)
	{
		File.WriteAllBytes(path, texture.EncodeToPNG());
	}

	public static Texture2D LoadTexture(string path)
	{
		if (File.Exists(path))
		{
			Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
			texture.LoadImage(File.ReadAllBytes(path));

			return texture;
		}

		return null;
	}

	public static Sprite ToSprite(this Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
	}

	#endregion // Texture2D

	#region Array 1-D

	public static void Swap(this int[] a)
	{
		int length = a.Length;
		int tmp;

		if (length < 2)
		{
			return;
		}

		if (length == 2)
		{
			if (Random.value > 0.5f)
			{
				tmp = a[0];
				a[0] = a[1];
				a[1] = tmp;
			}

			return;
		}

		for (int i = 0; i < length; i++)
		{
			int j = Random.Range(0, length);

			if (i != j)
			{
				tmp = a[i];
				a[i] = a[j];
				a[j] = tmp;
			}
		}
	}

	#endregion // Array 1-D

	#region File

	/// <summary>
	/// Gets the path of the specified file name.
	/// </summary>
	public static string GetFilePath(string fileName)
	{
		#if UNITY_EDITOR
		return string.Format("{0}/Resources/{1}", Application.dataPath, fileName);
		#elif UNITY_STANDALONE
		return string.Format("{0}/{1}", Application.dataPath, fileName);
		#else
		return string.Format("{0}/{1}", Application.persistentDataPath, fileName);
		#endif
	}
	
	public static string GetOrCreatePath(string name)
	{
		string path = GetFilePath(name);

		try
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		catch (IOException ex)
		{
			Debug.Log(ex.Message);
		}

		return path;
	}

	/// <summary>
	/// Saves data to the specified file.
	/// </summary>
	public static bool Save<T>(T data, string fileName)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(GetFilePath(fileName));
		bf.Serialize(file,data);
		file.Close();

		return true;
	}

	/// <summary>
	/// Loads data from the specified file.
	/// </summary>
	public static bool Load<T>(string fileName, ref T data)
	{
		string path = GetFilePath(fileName);

		if (File.Exists(path))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			data = (T)bf.Deserialize(file);
			file.Close();

			return true;
		}

		return false;
	}

    public static bool SaveJson<T>(T data , string fileName)
    {
        string path = GetFilePath(fileName);
        string datatext = JsonUtility.ToJson(data);
        File.WriteAllText(path,datatext);

        return true;
    }

    public static bool LoadJson<T>(string fileName,ref T data,bool read = false)
    {
        if(read)
        {
            string path = GetFilePath(fileName);

            if (File.Exists(path))
            {               
               data = JsonUtility.FromJson<T>(File.ReadAllText(path));
               return true;                
            }
            else
            {
                File.Create(path);               
                return false;
            }          
        }
        else
        {
            var textAsset = Resources.Load<TextAsset>(fileName);

            if (textAsset != null)
            {
                data = JsonUtility.FromJson<T>(textAsset.text);
                return true;
            }
        }
                    
        return false;
    }


    #endregion // File

    #region Touch

   

    #endregion

    #region Network

    public static bool IsOnline()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	#endregion
}
