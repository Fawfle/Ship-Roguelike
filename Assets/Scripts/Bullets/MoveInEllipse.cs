using UnityEngine;

/// <summary>
/// Move object on an ellipitical path
/// </summary>
public class MoveInEllipse : MonoBehaviour
{
	public Vector2 size = Vector2.one * 3f;
	private UpdatableFloat speedX = new(0.5f);
	private UpdatableFloat speedY = new(0.5f);

	public Vector2 t = Vector2.zero;

	private static readonly float TAU = Mathf.PI * 2;

	private void Update()
	{
		Vector2 fromPoint = GetPointOnEllipse(t);

		t = new Vector2((t.x + speedX.value * Time.deltaTime) % TAU, (t.y + speedY.value * Time.deltaTime) % TAU);
		speedX.Update(Time.deltaTime);
		speedY.Update(Time.deltaTime);
		// calculate velocity to get to next point on circle
		Vector3 velocity = GetPointOnEllipse(t) - fromPoint;

		transform.localPosition += velocity;
	}

	public Vector2 GetPointOnEllipse(Vector2 _t)
	{
		return new Vector2(Mathf.Cos(_t.x) * size.x, Mathf.Sin(_t.y) * size.y);
	}

	public void SetSpeed(UpdatableFloat speedX, UpdatableFloat speedY)
	{
		this.speedX = speedX;
		this.speedY = speedY;
	}

	public void SetOnEllipse(Vector2 center, Vector2 size)
	{
		this.size = size;
		t = (Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - center) * Mathf.Deg2Rad) * Vector2.one;
	}
}
