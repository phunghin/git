using UnityEngine;
using System.Collections.Generic;

public class ObjectPool
{
	private GameObject _prefab;
	private Transform _transform;
	private int _capacity;
	private Stack<GameObject> _pool;

   private Vector3 _hiden = new Vector3 (999,999,0);

	public void Construct(GameObject prefab, Transform transform, int capacity, int quantity)
	{
		_prefab    = prefab;
		_transform = transform;
		_capacity  = capacity;

		// Clear objects
		Clear();

		// Create pool
		_pool = new Stack<GameObject>(capacity);

		// Preload
		for (int i = 0; i < quantity; i++)
		{
			GameObject go = GameObject.Instantiate<GameObject>(_prefab);
			go.transform.SetParent(transform);
            go.transform.position = _hiden;
            go.SetActive(false);

			_pool.Push(go);
		}
	}

	public GameObject Get()
	{
		if (_pool.Count > 0)
		{
			GameObject go = _pool.Pop();
			go.SetActive(true);
            
			return go;
		}

		return GameObject.Instantiate<GameObject>(_prefab);
	}

	public void Return(GameObject go)
	{
		if (_pool.Count < _capacity)
		{
			go.transform.SetParent(_transform);
			go.SetActive(false);

			_pool.Push(go);
		}
		else
		{
			GameObject.Destroy(go);
		}
	}

    
	public void Clear()
	{
		if (_pool != null)
		{
			int count = _pool.Count;

			for (int i = 0; i < count; i++)
			{
				GameObject go = _pool.Pop();
				GameObject.Destroy(go);
			}
		}
	}
}
