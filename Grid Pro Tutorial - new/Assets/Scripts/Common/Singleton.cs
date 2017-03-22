using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T _instance;
	
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<T>();
				
				if (_instance == null)
				{
					_instance = new GameObject(typeof(T).Name).AddComponent<T>();
					DontDestroyOnLoad(_instance.gameObject);
				}
			}
			
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}
}
