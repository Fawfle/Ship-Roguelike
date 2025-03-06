using System;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// https://github.com/Matthew-J-Spencer/Object-Pooler
/// A simple base class to simplify object pooling in Unity 2021.
/// </summary>
public class GameObjectPooler<T> : ObjectPool<T> where T : Component
{
	private T prefab;
	Transform parent;

	public GameObjectPooler(T prefab, Transform parent, int defaultCapacity = 10, int maxSize = 10000, bool collectionCheck = false) : base(CreateFunc(prefab, parent), GetSetup, ReleaseSetup, DestroySetup, collectionCheck, defaultCapacity, maxSize)
	{
		this.prefab = prefab;
		this.parent = parent;
	}

	#region Overrides
	protected static Func<T> CreateFunc(T prefab, Transform parent) {
		return () =>
		{
			T g = GameObject.Instantiate(prefab);

			g.transform.SetParent(parent);

			return g;
		};
	}
	protected static void GetSetup(T obj)
	{ 
		obj.gameObject.SetActive(true);
	}
	protected static void ReleaseSetup(T obj)
	{
		obj.gameObject.SetActive(false);
	}
	protected static void DestroySetup(T obj) => GameObject.Destroy(obj);
	#endregion
}