using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BulletManager
{
    //public static BulletManager Instance { get; private set; }

    //public BulletManagerData data;

	public static Dictionary<string, GameObjectPooler<Bullet>> objectPools = new();

	public static readonly int INITIAL_POOL_SIZE = 20;
	public static readonly int MAX_POOL_SIZE = 1000;

	/*
	private void Awake()
	{
		if (Instance != null && Instance != this) { Destroy(gameObject); return; }
	}
	*/

	#region Create Bullets

	/// <summary>
	/// Creates <paramref name="count"/> bullets in a circle pattern centered about the <paramref name="origin"/>. Optional <paramref name="offset"/> in degrees
	/// </summary>
	/// <returns></returns>
	public static Bullet[] CreateBulletsInCircle(Bullet bulletPrefab, Vector3 origin, float radius, int count, float offset = 0)
	{
		Bullet[] bullets = new Bullet[count];

		float spacing = 360f / count;

		for (int i = 0; i < count; i++)
		{
			Vector3 position = origin + radius * (Vector3)MyUtils.Vector2FromAngle(offset + spacing * i);
			Bullet bullet = CreateBullet(bulletPrefab, position);
			bullets[i] = bullet;
		}

		return bullets;
	}

	/// <summary>
	/// Creates bullets in a spread from <paramref name="startAngle"/> to <paramref name="endAngle"/> seperated by an angle in degrees. Optional <paramref name="offset"/> in degrees
	/// </summary>
	/// <returns>A list of the created bullets</returns>
	public static Bullet[] CreateBulletsInArc(Bullet bulletPrefab, Vector3 origin, float radius, float startAngle, float endAngle, int count, float offset = 0)
	{
		Bullet[] bullets = new Bullet[count];

		float spacing = (endAngle - startAngle) / (count - 1);

		for (int i = 0; i < count; i++)
		{
			Vector3 position = origin + radius * (Vector3)MyUtils.Vector2FromAngle(startAngle + offset + spacing * i);
			Bullet bullet = CreateBullet(bulletPrefab, position);
			bullets[i] = bullet;
		}

		return bullets;
	}

	public static Bullet[] CreateBulletsOnSpline(Bullet bulletPrefab, SplineContainer spline, int count)
	{
		Bullet[] bullets = new Bullet[count];

		for (int i = 0; i < count; i++)
		{
			Vector3 position = spline.EvaluatePosition(i / count);
			Bullet bullet = CreateBullet(bulletPrefab, position);
			bullets[i] = bullet;
		}

		return bullets;
	}

	public static Bullet[] CreateBulletsRotatedAboutPoint(Bullet[] bullets, Vector3 point, float angle)
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			float radius = Vector2.Distance(point, bullets[i].transform.position);
			float a = Vector2.SignedAngle(Vector2.right, bullets[i].transform.position - point);
			bullets[i].transform.position = MyUtils.Vector2FromAngle(a + angle) * radius;
		}

		return bullets;
	}

	/// <summary>
	/// Creates bullets in a spread formation in a specified <paramref name="startDirection"> to <paramref name="endDirection"/>
	/// </summary>
	/// <returns>A list of the created bullets</returns>
	public static Bullet[] CreateBulletsInArc(Bullet bulletPrefab, Vector3 origin, float radius, Vector2 startDirection, Vector2 endDirection, int count)
	{
		return CreateBulletsInArc(bulletPrefab, origin, radius, startDirection.ToAngle(), endDirection.ToAngle(), count);
	}

	/// <summary>
	/// Create bullets in a line starting at <paramref name="startPosition"/> to <paramref name="endPosition"/>
	/// </summary>
	public static Bullet[] CreateBulletsInLine(Bullet bulletPrefab, Vector3 startPosition, Vector3 endPosition, int count)
	{
		Bullet[] bullets = new Bullet[count];

		Vector3 spacing = (endPosition - startPosition) / (count - 1);

		for (int i = 0; i < count; i++)
		{
			Vector3 position = startPosition + spacing * i;
			Bullet bullet = CreateBullet(bulletPrefab, position);
			bullets[i] = bullet;
		}

		return bullets;
	}

	public static Bullet[] CreateBullets(Bullet bulletPrefab, Vector3 position, int count)
	{
		Bullet[] bullets = new Bullet[count];
		
		for (int i = 0; i < count; i++)
		{
			bullets[i] = CreateBullet(bulletPrefab, position);
		}

		return bullets;
	}

	public static Bullet CreateBullet(Bullet bulletPrefab, Vector3 position)
	{
		if (!objectPools.ContainsKey(bulletPrefab.name))
		{
			AddObjectPool(bulletPrefab);
		}

		Bullet bullet  = objectPools[bulletPrefab.name].Get();
		bullet.poolKey = bulletPrefab.name;

		bullet.transform.position = position;

		return bullet;
	}

	#endregion

	#region Helpers

	public static void Release(Bullet bullet)
	{
		objectPools[bullet.poolKey].Release(bullet);
	}

	public static void AddObjectPool(Bullet prefab)
	{
		Transform parent = new GameObject(prefab.name + " Pool").transform;
		var pool = new GameObjectPooler<Bullet>(prefab, parent, INITIAL_POOL_SIZE, MAX_POOL_SIZE, false);
		objectPools.Add(prefab.name, pool);
	}

	#endregion
}