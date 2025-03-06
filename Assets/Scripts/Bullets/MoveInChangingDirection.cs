using Unity.Collections;
using UnityEngine;

/// <summary>
/// Move object in local forward direction
/// </summary>
public class MoveInChangingDirection : MonoBehaviour
{
    public UpdatableFloat speed = new(1f);

	public UpdatableFloat rotateSpeed = new(0f);

	private float directionAngle = 0f;

	private void Update()
	{
		directionAngle += rotateSpeed.value * Time.deltaTime;
		directionAngle %= 360f;

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
