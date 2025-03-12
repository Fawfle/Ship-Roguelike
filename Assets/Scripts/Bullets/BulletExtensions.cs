using UnityEngine;
using UnityEngine.Splines;

public static class BulletExtensions
{
	// extension methods are based
	
	public static Bullet[] ApplyLifetime(this Bullet[] bullets, float lifetime)
	{
		foreach (Bullet b in bullets)
		{
			b.SetLifetime(lifetime);
		}

		return bullets;
	}

	#region Move Bullets

	public static Bullet[] MoveAroundPoint(this Bullet[] bullets, Vector3 center, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveCircle(center, speed);
		}

		return bullets;
	}
	public static Bullet[] GiveMoveEllipse(this Bullet[] bullets, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveEllipse(size, speedX, speedY);
		}

		return bullets;
	}

	public static Bullet[] MoveAroundPointWithRadius(this Bullet[] bullets, Vector3 center, UpdatableFloat speed, UpdatableFloat radius)
	{
		foreach (Bullet b in bullets) b.GiveMoveCircle(center, speed, radius);

		return bullets;
	}

	public static Bullet[] MoveAroundPointOnEllipse(this Bullet[] bullets, Vector2 center, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveEllipse(center, size, speedX, speedY);
		}

		return bullets;
	}


	public static Bullet[] GiveMoveCircle(this Bullet[] bullets, UpdatableFloat radius, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets)
		{
			b.GiveMoveCircle(radius, speed);
		}

		return bullets;
	}

	public static Bullet[] GiveMoveDirection(this Bullet[] bullets, Vector2 direction, UpdatableFloat speed)
	{
		foreach (Bullet b in bullets) b.GiveMoveDirection(direction, speed);

		return bullets;
	}

	#region Changing Direction

	public static Bullet[] GiveChangingDirection(this Bullet[] bullets, Vector2 startDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(startDirection, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] GiveChangingDirection(this Bullet[] bullets, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveChangingDirection(startAngle, speed, rotateSpeed);

		return bullets;
	}

	#endregion

	#region Homing

	public static Bullet[] HomeInOnTarget(this Bullet[] bullets, Transform target, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveTarget(target, startAngle, speed, rotateSpeed);

		return bullets;
	}

	public static Bullet[] HomeInOnTarget(this Bullet[] bullets, Transform target, Vector2 startDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		foreach (Bullet b in bullets) b.GiveMoveTarget(target, startDirection, speed, rotateSpeed);

		return bullets;
	}

	#endregion

	#endregion

	#region Move In Directions

	public static MoveInDirectionBase[] MoveAwayFromPoint(this MoveInDirectionBase[] moveInDirections, Vector3 point)
	{
		foreach (MoveInDirectionBase m in moveInDirections)
		{
			m.SetDirection(m.transform.position - point);
		}

		return moveInDirections;
	}

	public static MoveInDirectionBase[] MoveTowardsPoint(this MoveInDirectionBase[] moveInDirections, Vector3 point)
	{
		foreach (MoveInDirectionBase m in moveInDirections)
		{
			m.SetDirection(point - m.transform.position);
		}

		return moveInDirections;
	}

	public static MoveInDirectionBase[] MoveOutInCircle(this MoveInDirectionBase[] moveInDirections, float offset = 0f)
	{
		float spacing = 360f / (moveInDirections.Length);

		for (int i = 0; i < moveInDirections.Length; i++)
		{
			moveInDirections[i].SetDirection(MyUtils.Vector2FromAngle(offset + spacing * i));
		}

		return moveInDirections;
	}

	public static MoveInDirectionBase[] MoveOutInArc(this MoveInDirectionBase[] moveInDirections, float startAngle, float endAngle, float offset = 0f)
	{
		float spacing = (endAngle - startAngle) / (moveInDirections.Length - 1);

		for (int i = 0; i < moveInDirections.Length; i++)
		{
			moveInDirections[i].SetDirection(MyUtils.Vector2FromAngle(startAngle + offset + spacing * i));
		}

		return moveInDirections;
	}

	public static MoveInDirectionBase[] MoveOutInArc(this MoveInDirectionBase[] moveInDirections, Vector2 direction, float spread, float offset = 0)
	{
		float initialAngle = direction.ToAngle() - spread * ((moveInDirections.Length - 1) / 2f) + offset;

		for (int i = 0; i < moveInDirections.Length; i++)
		{
			moveInDirections[i].SetDirection(MyUtils.Vector2FromAngle(initialAngle + spread * i));
		}

		return moveInDirections;
	}

	public static MoveInDirectionBase[] MoveOutInSpline(this MoveInDirectionBase[] moveInDirections, SplineContainer spline)
	{
		for (int i = 0; i < moveInDirections.Length; i++)
		{
			moveInDirections[i].SetDirection((Vector3)spline.EvaluateUpVector(i / moveInDirections.Length));
		}

		return moveInDirections;
	}

	#endregion
}
