using Unity.Collections;
using UnityEngine;

/// <summary>
/// Move object straight in specified direction
/// </summary>
public class MoveInDirection : MoveInDirectionBase
{
    public UpdatableFloat speed = new(1f);

	[SerializeField] private Vector2 direction = Vector2.zero;

	private void Update()
	{
		transform.localPosition += speed.value * Time.deltaTime * (Vector3)direction;
		speed.Update(Time.deltaTime);
	}

	public override void SetDirection(Vector2 dir)
	{
		direction = dir.normalized;
	}

	public override void SetDirection(float angle)
	{
		direction = MyUtils.Vector2FromAngle(angle);
	}
}
