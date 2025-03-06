#if UNITY_EDITOR
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTester : MonoBehaviour
{
	public BulletTesterPattern pattern;

	private void FixedUpdate()
	{
		if (pattern.enabled)
		{
			pattern.FixedUpdate();
		}
	}

	public void TestBullets()
	{
		if (!Application.isPlaying) return;

		pattern.TestBullets();
	}

	public void ClearBullets()
	{
		if (!Application.isPlaying) return;

		pattern.ClearBullets();
	}

	[System.Serializable]
	public class BulletTesterPattern
	{
		public bool enabled = true;
		public float spawnRate = 1f;
		public UpdatableFloat spinSpeed = new(0f);

		private float t = 0f;
		[HideInInspector] public float rotation = 0f;

		public BulletTesterEmitter emitter;
		public List<BulletTesterModifier> modifiers = new();

		private List<Bullet> bullets = new();

		public void FixedUpdate()
		{
			// remove bullets
			for (int i = 0; i < bullets.Count; i++)
			{
				if (!bullets[i].isActiveAndEnabled)
				{
					bullets.Remove(bullets[i]);
					i--;
				}
			}

				if (!enabled) return;

			t += Time.fixedDeltaTime * spawnRate;

			if (t >= 1f)
			{
				TestBullets();
				t = 0f;
			}

			rotation = (rotation + spinSpeed.value * Time.fixedDeltaTime) % 360f;
			spinSpeed.Update(Time.fixedDeltaTime);
		}

		public Bullet[] TestBullets()
		{
			Bullet[] bs = emitter.CreateBullets(this);

			foreach (BulletTesterModifier modifier in modifiers)

			{
				modifier.Apply(bs);
			}

			bullets.AddRange(bs);

			return bs;
		}

		public void ClearBullets()
		{
			foreach (Bullet b in bullets)
			{
				if (b == null || !b.isActiveAndEnabled) continue;
				BulletManager.Release(b);
			}

			bullets.Clear();
		}
	}


	[System.Serializable]
	public class BulletTesterEmitter
	{
		public Bullet prefab;
		public BulletTesterEmitterType type = BulletTesterEmitterType.Point;

		[HideInInspector] public int count = 0;
		[HideInInspector] public Vector2 origin = Vector2.zero;

		[HideInInspector] public float radius;

		[HideInInspector] public float startAngle;
		[HideInInspector] public float endAngle;

		[HideInInspector] public Vector2 startPosition;
		[HideInInspector] public Vector2 endPosition;

		public Bullet[] CreateBullets(BulletTesterPattern pattern)
		{
			return type switch
			{
				BulletTesterEmitterType.Point => BulletManager.CreateBullets(prefab, origin, count),
				BulletTesterEmitterType.Circle => BulletManager.CreateBulletsInCircle(prefab, origin, radius, count, pattern.rotation),
				BulletTesterEmitterType.Arc => BulletManager.CreateBulletsInArc(prefab, origin, radius, startAngle, endAngle, count, pattern.rotation),
				BulletTesterEmitterType.Line => BulletManager.RotateBulletsAboutPoint(BulletManager.CreateBulletsInLine(prefab, startPosition, endPosition, count), origin, pattern.rotation),
				_ => throw new System.NotImplementedException()
			};
		}
	}

	[System.Serializable]
	public class BulletTesterModifier
	{
		public BulletModifierType type = BulletModifierType.Direction;

		public UpdatableFloat speed = new(1f);
		[HideInInspector] public Vector2 direction = Vector2.down;
		[HideInInspector] public Vector2 point = Vector2.zero;
		public UpdatableFloat radius = new(1f);


		[HideInInspector] public UpdatableFloat speedX = new(1f);
		[HideInInspector] public UpdatableFloat speedY = new(1f);
		[HideInInspector] public Vector2 size = Vector2.one;

		public float angle = 0f;
		public UpdatableFloat rotateSpeed = new(0f);

		public Transform target;

		public Bullet[] Apply(Bullet[] bullets)
		{
			foreach (Bullet bullet in bullets)
			{
				switch (type)
				{
					case BulletModifierType.Direction:
						BulletManager.MoveBulletsInDirection(bullets, direction, speed);
						break;
					case BulletModifierType.TowardsPoint:
						BulletManager.MoveBulletsTowardsPoint(bullets, point, speed);
						break;
					case BulletModifierType.AwayPoint:
						BulletManager.MoveBulletsAwayFromPoint(bullets, point, speed);
						break;
					case BulletModifierType.ChangingDirection:
						BulletManager.MoveBulletsInChangingDirection(bullets, angle, speed, rotateSpeed);
						break;
					case BulletModifierType.ChangingDirectionPoint:
						BulletManager.MoveBulletsInChangingDirectionTowardPoint(bullets, point, speed, rotateSpeed);
						break;
					case BulletModifierType.ChangingDirectionAwayPoint:
						BulletManager.MoveBulletsInChangingDirectionAwayFromPoint(bullets, point, speed, rotateSpeed);
						break;
					case BulletModifierType.Circle:
						BulletManager.MoveBulletsInCircle(bullets, radius, speed);
						break;
					case BulletModifierType.AroundPoint:
						BulletManager.MoveBulletsAroundPoint(bullets, point, speed);
						break;
					case BulletModifierType.AroundPointWithRadius:
						BulletManager.MoveBulletsAroundPointWithRadius(bullets, point, speed, radius);
						break;
					case BulletModifierType.Ellipse:
						BulletManager.MoveBulletsInEllipse(bullets, size, speedX, speedY);
						break;
					case BulletModifierType.AroundPointEllipse:
						BulletManager.MoveBulletsAroundPointOnEllipse(bullets, point, size, speedX, speedY);
						break;
					case BulletModifierType.Target:
						BulletManager.MoveBulletsHomingToTarget(bullets, target, angle, speed, rotateSpeed);
						break;
				}
			}

			return bullets;
		}
	}

	// changes emitter
	public enum BulletTesterEmitterType
	{
		Point,
		Arc,
		Circle,
		Line
	}

	// affects actual bullets
	public enum BulletModifierType
	{
		Direction,
		TowardsPoint,
		AwayPoint,
		ChangingDirection,
		ChangingDirectionPoint,
		ChangingDirectionAwayPoint,
		Circle,
		AroundPoint,
		AroundPointWithRadius,
		Ellipse,
		AroundPointEllipse,
		Target,
	}
}


#endif