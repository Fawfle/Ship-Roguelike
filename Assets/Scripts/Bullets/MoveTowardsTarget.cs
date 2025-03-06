using UnityEngine;

/// <summary>
/// Homing objects
/// </summary>
public class MoveTowardsTarget : MonoBehaviour
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

		transform.localPosition += speed.value * Time.deltaTime * new Vector3(Mathf.Cos(directionAngle * Mathf.Deg2Rad), Mathf.Sin(directionAngle * Mathf.Deg2Rad));
	}

	public void SetDirection(Vector2 dir)
	{
		directionAngle = Vector2.SignedAngle(Vector2.right, dir);
	}

	public void SetDirection(float angle)
	{
		directionAngle = angle;
	}
}
