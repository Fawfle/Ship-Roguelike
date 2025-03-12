#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace BulletTesting
{
	public class BulletTester : MonoBehaviour
	{
		public BulletTesterPattern pattern;

		private void Update()
		{
			if (pattern.enabled)
			{
				pattern.Update();
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
			public float lifetime = 0f;

			private float t = 0f;
			[HideInInspector] public float rotation = 0f;

			public BulletTesterEmitter emitter;
			public List<BulletTesterModifier> modifiers = new();

			private List<Bullet> bullets = new();

			public void Update()
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

				t += Time.deltaTime * spawnRate;

				if (t >= 1f)
				{
					TestBullets();
					t = 0f;
				}

				rotation = (rotation + spinSpeed.value * Time.deltaTime) % 360f;
				spinSpeed.Update(Time.deltaTime);
			}

			public Bullet[] TestBullets()
			{
				Bullet[] bs = emitter.CreateBullets(this);

				bs.ApplyLifetime(lifetime);

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
					BulletTesterEmitterType.Line => BulletManager.CreateBulletsRotatedAboutPoint(BulletManager.CreateBulletsInLine(prefab, startPosition, endPosition, count), origin, pattern.rotation),
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
				switch (type)
				{
					case BulletModifierType.Direction:
						bullets.GiveMoveDirection(direction, speed);
						break;
					case BulletModifierType.TowardsPoint:
						bullets.GiveMoveDirection(Vector2.zero, speed).GetComponents<MoveInDirection>().MoveTowardsPoint(point);
						break;
					case BulletModifierType.AwayPoint:
						bullets.GiveMoveDirection(Vector2.zero, speed).GetComponents<MoveInDirection>().MoveAwayFromPoint(point);
						break;
					case BulletModifierType.ChangingDirection:
						bullets.GiveChangingDirection(angle, speed, rotateSpeed);
						break;
					case BulletModifierType.ChangingDirectionPoint:
						bullets.GiveChangingDirection(Vector2.zero, speed, rotateSpeed).GetComponents<MoveInChangingDirection>().MoveAwayFromPoint(point);
						break;
					case BulletModifierType.ChangingDirectionAwayPoint:
						bullets.GiveChangingDirection(Vector2.zero, speed, rotateSpeed).GetComponents<MoveInChangingDirection>().MoveTowardsPoint(point);
						break;
					case BulletModifierType.Circle:
						bullets.GiveMoveCircle(radius, speed);
						break;
					case BulletModifierType.AroundPoint:
						bullets.MoveAroundPoint(point, speed);
						break;
					case BulletModifierType.AroundPointWithRadius:
						bullets.MoveAroundPointWithRadius(point, speed, radius);
						break;
					case BulletModifierType.Ellipse:
						bullets.GiveMoveEllipse(size, speedX, speedY);
						break;
					case BulletModifierType.AroundPointEllipse:
						bullets.MoveAroundPointOnEllipse(point, size, speedX, speedY);
						break;
					case BulletModifierType.Target:
						bullets.HomeInOnTarget(target, angle, speed, rotateSpeed);
						break;
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
}
#endif