using Unity.Collections;
using UnityEngine;

/// <summary>
/// Move object straight in specified direction
/// </summary>
public class MoveInDirection : MonoBehaviour
{
    public UpdatableFloat speed = new(1f);

	[SerializeField] private Vector2 direction = Vector2.zero;

	private void Update()
	{
		transform.localPosition += speed.value * Time.deltaTime * (Vector3)direction;
		speed.Update(Time.deltaTime);
	}

	public void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}

	public void SetDirectionToAngle(float angle)
	{
		float angleRadians = angle * Mathf.Deg2Rad;
		direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
	}
}
