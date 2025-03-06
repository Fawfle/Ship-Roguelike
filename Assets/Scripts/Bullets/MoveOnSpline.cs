using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Move object on spline path
/// </summary>
public class MoveOnSpline : MonoBehaviour
{
	[SerializeField] private SplineContainer spline;
	public UpdatableFloat speed = new(4f);

	public float t = 0f;

	private void Update()
	{
		Vector3 previousPosition = spline.EvaluatePosition(t);
		t += speed.value * Time.deltaTime;
		t %= 1;
		Vector3 newPosition = spline.EvaluatePosition(t);

		speed.Update(Time.deltaTime);

		transform.localPosition += (newPosition - previousPosition);
	}
}
