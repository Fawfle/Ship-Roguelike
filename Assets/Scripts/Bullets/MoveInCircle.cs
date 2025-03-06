using UnityEngine;

/// <summary>
/// Move object on circular path
/// </summary>
public class MoveInCircle : MonoBehaviour
{
    public UpdatableFloat radius = new(3f);
    private UpdatableFloat speed = new(0.5f);

	public float t = 0f;

	private static readonly float TAU = Mathf.PI * 2;

	private void Update()
	{
		Vector2 fromPoint = GetPointOnCircle(t);

		t = (t + speed.value * Time.deltaTime) % TAU;
		speed.Update(Time.deltaTime);
		radius.Update(Time.deltaTime);
		// calculate velocity to get to next point on circle
		Vector3 velocity = GetPointOnCircle(t) - fromPoint;

		transform.localPosition += velocity;
	}

	public Vector2 GetPointOnCircle(float _t)
	{
		return new Vector2(Mathf.Cos(_t), Mathf.Sin(_t)) * radius.value;
	}

	public void SetSpeed(UpdatableFloat value)
	{
		speed = value;
	}

	public void SetOnCircle(Vector2 center)
	{
		radius = new UpdatableFloat(Vector2.Distance(transform.position, center));
		t = (Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - center) * Mathf.Deg2Rad);
	}

	public void SetOnCircleWithRadius(Vector2 center, UpdatableFloat r)
	{
		radius = r;
		t = (Vector2.SignedAngle(Vector2.right, (Vector2)transform.position - center) * Mathf.Deg2Rad);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (Application.isPlaying) return;

		Gizmos.color = new Color(0.8f, 0.8f, 0.8f, 0.1f);
		Gizmos.DrawWireSphere(transform.position - radius.value * new Vector3(Mathf.Cos(t), Mathf.Sin(t)), radius.value);
	}

#endif
}
