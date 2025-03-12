using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;

	private PlayerData.MovementData data;

	private bool canDash = true;

	[SerializeField] private MoveState state = MoveState.Normal;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		data = PlayerManager.Instance.playerData.movement;

		SetMoveState(MoveState.Normal);
	}

	private void FixedUpdate()
	{
		if (state == MoveState.Normal)
		{
			rb.linearVelocity = InputManager.MoveInput.normalized * data.speed;
			if (InputManager.Sneak.IsPressed()) rb.linearVelocity = rb.linearVelocity * data.sneakDamping;

			//float moveAngle = Vector2.SignedAngle(Vector2.up, InputManager.MoveInput);
			//if (InputManager.MoveInput.magnitude != 0 && moveAngle % 90f == 0) rb.rotation = moveAngle;

			if (canDash && InputManager.Dash.WasPressedThisFrame())
			{
				if (InputManager.MoveInput.magnitude != 0) StartCoroutine(Dash());
				else StartCoroutine(Stall());
			}
		}
	}

	private IEnumerator Dash()
	{
		SetMoveState(MoveState.Dash);

		float dashAngle = InputManager.MoveInput.magnitude != 0 ? Vector2.SignedAngle(Vector2.right, InputManager.MoveInput) : 90;

		float t = 0;

		while (t <= 1f)
		{
			t += Time.fixedDeltaTime / data.dashDuration;

			// move during dash
			if (InputManager.MoveInput.magnitude != 0) {
				float targetAngle = -Vector2.SignedAngle(InputManager.MoveInput, Vector2.right);
				if (Mathf.DeltaAngle(targetAngle, dashAngle) <= 90f)
				{
					dashAngle = Mathf.MoveTowardsAngle(dashAngle, targetAngle, data.dashAngleChangeDegrees * Time.fixedDeltaTime / data.dashDuration);
				} else
				{
					t += Time.fixedDeltaTime / data.dashDuration;
				}
				//print("target: " + targetAngle);
				//print("actual:" + dashAngle);
			}

			rb.linearVelocity = new Vector2(Mathf.Cos(dashAngle * Mathf.Deg2Rad), Mathf.Sin(dashAngle * Mathf.Deg2Rad)) * Mathf.Lerp(data.dashStartSpeed, data.dashEndSpeed, Mathf.Sqrt(t));

			yield return new WaitForFixedUpdate();
		}

		//rb.linearDamping = damping;

		StartCoroutine(StartDashCooldown());

		SetMoveState(MoveState.Normal);
	}

	private IEnumerator Stall()
	{
		SetMoveState(MoveState.Stall);

		float t = 0;

		while (t <= 1f && InputManager.Dash.IsPressed())
		{
			t += Time.fixedDeltaTime / data.stallDuration;

			if (InputManager.MoveInput.magnitude != 0 && rb.linearVelocity.magnitude == 0)
			{
				StartCoroutine(Dash());
				yield break;
			}
			else
			{
				yield return new WaitForFixedUpdate();
			}
		}

		StartCoroutine(StartDashCooldown());

		SetMoveState(MoveState.Normal);
	}

	private IEnumerator StartDashCooldown()
	{
		canDash = false;
		yield return new WaitForSeconds(data.dashCooldown);
		canDash = true;
	}

	private void SetMoveState(MoveState s)
	{
		state = s;
	}

	enum MoveState
	{
		Normal,
		Dash,
		Stall,
		Disabled
	}
}