using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance { get; private set; }

    public BulletManagerData data;

	public static Dictionary<string, GameObjectPooler<Bullet>> objectPools = new();

	public static readonly int INITIAL_POOL_SIZE = 20;
	public static readonly int MAX_POOL_SIZE = 1000;

	private void Awake()
	{
		if (Instance != null && Instance != this) { Destroy(gameObject); return; }
	}

	#region Move Bullets

	public static Bullet[] MoveBulletsAroundPointOnEllipse(Bullet[] bullets, Vector2 center, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveEllipse(center, size, speedX, speedY);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsInEllipse(Bullet[] bullets, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveEllipse(size, speedX, speedY);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsAroundPoint(Bullet[] bullets, Vector3 center, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveCircle(center, speed);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsAroundPointWithRadius(Bullet[] bullets, Vector3 center, UpdatableFloat speed, UpdatableFloat radius)
	{
		foreach (Bullet b in bullets) b.GiveMoveCircle(center, speed, radius);

		return bullets;
	}

	public static Bullet[] MoveBulletsInCircle(Bullet[] bullets, UpdatableFloat radius, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveCircle(radius, speed);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsAwayFromPoint(Bullet[] bullets, Vector3 point, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveDirection(b.transform.position - point, speed);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsTowardsPoint(Bullet[] bullets, Vector3 point, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveDirection(point - b.transform.position, speed);
		}

		return bullets;
	}

	public static Bullet[] MoveBulletsInDirection(Bullet[] bullets, Vector2 direction, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets) b.GiveMoveDirection(direction, speed);

		return bullets;
	}

	#region Changning Direction

	public static Bullet[] MoveBulletsInChangingDirection(Bullet[] bullets, Vector2 startDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(startDirection, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] MoveBulletsInChangingDirection(Bullet[] bullets, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(startAngle, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] MoveBulletsInChangingDirectionAwayFromPoint(Bullet[] bullets, Vector3 point, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(b.transform.position - point, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] MoveBulletsInChangingDirectionTowardPoint(Bullet[] bullets, Vector3 point, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(point - b.transform.position, speed, rotateSpeed);

		return bullets;
	}

	#endregion

	public static Bullet[] MoveBulletsHomingToTarget(Bullet[] bullets, Transform target, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveTarget(target, startAngle, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] MoveBulletsHomingToTarget(Bullet[] bullets, Transform target, Vector2 startDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveTarget(target, startDirection, speed, rotateSpeed);

		return bullets;
	}

	#endregion

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
			Vector3 position = origin + radius * VectorFromAngle(offset + spacing * i);
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

		float spacing = (startAngle - endAngle) / (count - 1);

		for (int i = 0; i < count; i++)
		{
			Vector3 position = origin + radius * VectorFromAngle(startAngle + offset + spacing * i);
			Bullet bullet = CreateBullet(bulletPrefab, position);
			bullets[i] = bullet;
		}

		return bullets;
	}

	public static Bullet[] RotateBulletsAboutPoint(Bullet[] bullets, Vector3 point, float angle)
	{
		for (int i = 0; i < bullets.Length; i++)
		{
			float radius = Vector2.Distance(point, bullets[i].transform.position);
			float a = Vector2.SignedAngle(Vector2.right, bullets[i].transform.position - point);
			bullets[i].transform.position = VectorFromAngle(a + angle) * radius;
		}

		return bullets;
	}

	/// <summary>
	/// Creates bullets in a spread formation in a specified <paramref name="startDirection"> to <paramref name="endDirection"/>
	/// </summary>
	/// <returns>A list of the created bullets</returns>
	public static Bullet[] CreateBulletsInArc(Bullet bulletPrefab, Vector3 origin, float radius, Vector2 startDirection, Vector2 endDirection, int count)
	{
		return CreateBulletsInArc(bulletPrefab, origin, radius, VectorToAngle(startDirection), VectorToAngle(endDirection), count);
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

	/// <summary>
	/// Creates a vector at the specified <param name="angle">
	/// </summary>
	/// <param name="angle">Angle in degrees</param>
	/// <returns></returns>
	public static Vector3 VectorFromAngle(float angle)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
	}

	public static float VectorToAngle(Vector2 direction)
	{
		return Vector2.SignedAngle(direction, Vector2.right);
	}

	#endregion
}