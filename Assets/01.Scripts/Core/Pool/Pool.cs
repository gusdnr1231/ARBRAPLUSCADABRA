using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Pool<T> where T : PoolableMono
{
	private Stack<T> _pool = new Stack<T>();
	private List<T> _activeObjects = new List<T>(); // Active Object List
	private T _prefab; // Origin Prefab
	private Transform _parent;

	public int PoolCount => _pool.Count;
	public int ActiveCount => _activeObjects.Count; // Active Objects Count

	public Pool(T prefab, Transform parent, int count)
	{
		_prefab = prefab;
		_parent = parent;

		for (int i = 0; i < count; i++)
		{
			T obj = GameObject.Instantiate(prefab, parent);
			obj.name = obj.name.Replace("(Clone)", "");
			obj.gameObject.SetActive(false);
			obj?.EnablePoolableMono();
			_pool.Push(obj);
		}
	}

	public T Pop(Transform parent = null)
	{
		T obj = null;
		if (_pool.Count <= 0)
		{
			obj = GameObject.Instantiate(_prefab, _parent);
			obj.name = obj.name.Replace("(Clone)", "");
			obj?.gameObject.SetActive(false);
		}
		else
		{
			obj = _pool.Pop();
		}

		if (parent != null) obj.transform.SetParent(parent);

		obj?.gameObject.SetActive(true);
		obj?.ResetPoolableMono();
		_activeObjects.Add(obj); // Add to Active Object List
		return obj;
	}

	public void Push(T obj)
	{
		obj.gameObject.SetActive(false);
		_activeObjects.Remove(obj); // Remove to Active Object List
		_pool.Push(obj);
	}

	public void DestroyAll()
	{
		foreach (var obj in _activeObjects)
		{
			GameObject.Destroy(obj.gameObject);
		}
		_activeObjects.Clear();

		while (_pool.Count > 0)
		{
			T obj = _pool.Pop();
			GameObject.Destroy(obj.gameObject);
		}
	}
}