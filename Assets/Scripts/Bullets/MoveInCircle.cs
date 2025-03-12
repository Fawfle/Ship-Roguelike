using UnityEngine;

/// <summary>
/// Move object on circular path
/// </summary>
public class MoveInCircle : MonoBehaviour
{
    public UpdatableFloat radius = new(3f);
    private UpdatableFloat speed = new(0);

	public float t = 0f;

	private void Update()
	{
		Vector2 fromPoint = GetPointOnCircle(t);

		t = (t + speed.value * Time.deltaTime) % 360f;
		speed.Update(Time.deltaTime);
		radius.Update(Time.deltaTime);
		// calculate velocity to get to next point on circle
		Vector3 velocity = GetPointOnCircle(t) - fromPoint;

		transform.localPosition += velocity;
	}

	public Vector2 GetPointOnCircle(float _t)
	{
		return MyUtils.Vector2FromAngle(_t) * radius.value;
	}

	public void SetSpeed(UpdatableFloat value)
	{
		speed = value;
	}

	public void SetOnCircle(Vector2 center)
	{
		radius = new UpdatableFloat(Vector2.Distance(transform.position, center));
		t = Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - center);
	}

	public void SetOnCircleWithRadius(Vector2 center, UpdatableFloat r)
	{
		radius = r;
		t = Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - center);
	}
}
