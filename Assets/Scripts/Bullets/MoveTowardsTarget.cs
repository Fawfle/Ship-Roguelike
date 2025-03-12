using UnityEngine;

/// <summary>
/// Homing objects
/// </summary>
public class MoveTowardsTarget : MoveInDirectionBase
{
	public Transform target;
	// direction bullet is currently traveling in
	public float directionAngle = 0;

	public UpdatableFloat speed = 3f;
	public UpdatableFloat rotateSpeed = 0f;

	private void Update()
	{
		float toPlayerAngle = Vector2.SignedAngle(Vector2.right, target.position - transform.position);
		directionAngle = Mathf.MoveTowardsAngle(directionAngle, toPlayerAngle, rotateSpeed.value * Time.deltaTime);

		speed.Update(Time.deltaTime);
		rotateSpeed.Update(Time.deltaTime);

		transform.localPosition += speed.value * Time.deltaTime * (Vector3)MyUtils.Vector2FromAngle(directionAngle);
	}

	public override void SetDirection(Vector2 dir)
	{
		directionAngle = Vector2.SignedAngle(Vector2.right, dir);
	}

	public override void SetDirection(float angle)
	{
		directionAngle = angle;
	}
}
