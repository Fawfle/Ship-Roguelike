using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// distance out of world bounds to destroy
	public static readonly float DESTROY_PADDING = 5f;

	// set by bulletmanager, key for returning object to pool
	public string poolKey;

	private MoveInDirection moveDirection;
	private MoveInChangingDirection moveChangingDirection;
	private MoveTowardsTarget moveTarget;
	private MoveInCircle moveCircle;
	private MoveInEllipse moveEllipse;

	[Tooltip("zero/negative lifetime is 'Infinite'")]
	[SerializeField] private float lifetime = 0f;

	private void Awake()
	{
		moveDirection = GetComponent<MoveInDirection>();
		moveChangingDirection = GetComponent<MoveInChangingDirection>();
		moveTarget = GetComponent<MoveTowardsTarget>();
		moveCircle = GetComponent<MoveInCircle>();
		moveEllipse = GetComponent<MoveInEllipse>();
	}

	private void Update()
	{
		if (!WorldBounds.IsInBounds(transform.position, DESTROY_PADDING))
		{
			Deactivate();
			return;
		}

		if (lifetime <= 0f) return;
		lifetime -= Time.deltaTime;
		if (lifetime <= 0f) Deactivate();
	}

	public void Deactivate()
	{
		if (poolKey != null) BulletManager.Release(this);
		else Destroy(gameObject);
	}

	public void OnDisable()
	{
		if (moveDirection != null) moveDirection.enabled = false;
		if (moveChangingDirection != null) moveChangingDirection.enabled = false;
		if (moveTarget != null) moveTarget.enabled = false;
		if (moveCircle != null) moveCircle.enabled = false;
		if (moveEllipse != null) moveEllipse.enabled = false;
		lifetime = 0f;
	}

	public void SetLifetime(float lifetime)
	{
		this.lifetime = lifetime;
	}

	public MoveInDirection GiveMoveDirection(Vector2 direction, UpdatableFloat speed)
	{
		if (moveDirection == null) moveDirection = gameObject.AddComponent<MoveInDirection>();
		else moveDirection.enabled = true;

		moveDirection.speed = speed;
		moveDirection.SetDirection(direction);

		return moveDirection;
	}

	public MoveInChangingDirection GiveMoveChangingDirection(Vector2 startingDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveChangingDirection == null) moveChangingDirection = gameObject.AddComponent<MoveInChangingDirection>();
		else moveChangingDirection.enabled = true;

		moveChangingDirection.speed = speed;
		moveChangingDirection.rotateSpeed = rotateSpeed;
		moveChangingDirection.SetDirection(startingDirection);

		return moveChangingDirection;
	}

	public MoveInChangingDirection GiveMoveChangingDirection(float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveChangingDirection == null) moveChangingDirection = gameObject.AddComponent<MoveInChangingDirection>();
		else moveChangingDirection.enabled = true;

		moveChangingDirection.speed = speed;
		moveChangingDirection.rotateSpeed = rotateSpeed;
		moveChangingDirection.SetDirection(startAngle);

		return moveChangingDirection;
	}

	public MoveTowardsTarget GiveMoveTarget(Transform target, Vector2 startingDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveTarget == null) moveTarget = gameObject.AddComponent<MoveTowardsTarget>();
		else moveTarget.enabled = true;

		moveTarget.target = target;
		moveTarget.speed = speed;
		moveTarget.rotateSpeed = rotateSpeed;
		moveTarget.SetDirection(startingDirection);

		return moveTarget;
	}

	public MoveTowardsTarget GiveMoveTarget(Transform target, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveTarget == null) moveTarget = gameObject.AddComponent<MoveTowardsTarget>();
		else moveTarget.enabled = true;

		moveTarget.target = target;
		moveTarget.speed = speed;
		moveTarget.rotateSpeed = rotateSpeed;
		moveTarget.SetDirection(startAngle);

		return moveTarget;
	}

	public MoveInCircle GiveMoveCircle(UpdatableFloat radius, UpdatableFloat speed)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

		moveCircle.radius = radius;
		moveCircle.SetSpeed(speed);
		moveCircle.t = 0f;

		return moveCircle;
	}

	/// <summary>
	/// Specifies an center which sets the appropriate radius and t
	/// </summary>
	public MoveInCircle GiveMoveCircle(Vector2 center, UpdatableFloat speed)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

		moveCircle.SetOnCircle(center);
		moveCircle.SetSpeed(speed);

		return moveCircle;
	}

	public MoveInCircle GiveMoveCircle(Vector2 center, UpdatableFloat speed, UpdatableFloat radius)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

		moveCircle.SetOnCircleWithRadius(center, radius);
		moveCircle.SetSpeed(speed);

		return moveCircle;
	}

	public MoveInEllipse GiveMoveEllipse(Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		if (moveEllipse == null) moveEllipse = gameObject.AddComponent<MoveInEllipse>();
		else moveEllipse.enabled = true;

		moveEllipse.size = size;
		moveEllipse.SetSpeed(speedX, speedY);
		moveEllipse.t = Vector2.zero;

		return moveEllipse;
	}

	/// <summary>
	/// specifies an center to determine the angle, a size, and a speed
	/// </summary>
	public MoveInEllipse GiveMoveEllipse(Vector2 center, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		if (moveEllipse == null) moveEllipse = gameObject.AddComponent<MoveInEllipse>();
		else moveEllipse.enabled = true;

		moveEllipse.SetOnEllipse(center, size);
		moveEllipse.SetSpeed(speedX, speedY);

		return moveEllipse;
	}
}