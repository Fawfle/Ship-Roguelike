using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	// distance out of world bounds to destroy
	public static readonly float DESTROY_PADDING = 5f;

	// set by bulletmanager, key for returning object to pool
	public string poolKey;

	public MoveInDirection moveDirection;
	public MoveInChangingDirection moveChangingDirection;
	public MoveInCircle moveCircle;
	public MoveInEllipse moveEllipse;
	public MoveTowardsTarget moveTarget;

	[Tooltip("zero/negative lifetime is 'Infinite'")]
	public float lifetime = 0f;

	private void Awake()
	{
		moveDirection = GetComponent<MoveInDirection>();
		moveChangingDirection = GetComponent<MoveInChangingDirection>();
		moveCircle = GetComponent<MoveInCircle>();
		moveEllipse = GetComponent<MoveInEllipse>();
		moveTarget = GetComponent<MoveTowardsTarget>();
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
		if (moveCircle != null) moveCircle.enabled = false;
		if (moveEllipse != null) moveEllipse.enabled = false;
		lifetime = 0f;
	}

	public void GiveMoveDirection(Vector2 direction, UpdatableFloat speed)
	{
		if (moveDirection == null) moveDirection = gameObject.AddComponent<MoveInDirection>();
		else moveDirection.enabled = true;

		moveDirection.SetDirection(direction);
		moveDirection.speed = speed;
	}

	public void GiveMoveChangingDirection(Vector2 startingDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveChangingDirection == null) moveChangingDirection = gameObject.AddComponent<MoveInChangingDirection>();
		else moveChangingDirection.enabled = true;

		moveChangingDirection.speed = speed;
		moveChangingDirection.rotateSpeed = rotateSpeed;
		moveChangingDirection.SetDirection(startingDirection);
	}

	public void GiveMoveChangingDirection(float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveChangingDirection == null) moveChangingDirection = gameObject.AddComponent<MoveInChangingDirection>();
		else moveChangingDirection.enabled = true;

		moveChangingDirection.speed = speed;
		moveChangingDirection.rotateSpeed = rotateSpeed;
		moveChangingDirection.SetDirection(startAngle);
	}

	public void GiveMoveTarget(Transform target, Vector2 startingDirection, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveTarget == null) moveTarget = gameObject.AddComponent<MoveTowardsTarget>();
		else moveTarget.enabled = true;

		moveTarget.target = target;
		moveTarget.speed = speed;
		moveTarget.rotateSpeed = rotateSpeed;
		moveTarget.SetDirection(startingDirection);
	}

	public void GiveMoveTarget(Transform target, float startAngle, UpdatableFloat speed, UpdatableFloat rotateSpeed)
	{
		if (moveTarget == null) moveTarget = gameObject.AddComponent<MoveTowardsTarget>();
		else moveTarget.enabled = true;

		moveTarget.target = target;
		moveTarget.speed = speed;
		moveTarget.rotateSpeed = rotateSpeed;
		moveTarget.SetDirection(startAngle);
	}

	/*
	public void GiveMoveCircle(float radius, float speed)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();

		moveCircle.radius = radius;
		moveCircle.SetSpeed(speed);
		moveCircle.t = Vector2.zero;
	}
	*/

	public void GiveMoveCircle(UpdatableFloat radius, UpdatableFloat speed)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

		moveCircle.radius = radius;
		moveCircle.SetSpeed(speed);
		moveCircle.t = 0f;
	}

	/// <summary>
	/// Specifies an center which sets the appropriate radius and t
	/// </summary>
	public void GiveMoveCircle(Vector2 center, UpdatableFloat speed)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

			moveCircle.SetOnCircle(center);
		moveCircle.SetSpeed(speed);
	}

	public void GiveMoveCircle(Vector2 center, UpdatableFloat speed, UpdatableFloat radius)
	{
		if (moveCircle == null) moveCircle = gameObject.AddComponent<MoveInCircle>();
		else moveCircle.enabled = true;

		moveCircle.SetOnCircleWithRadius(center, radius);
		moveCircle.SetSpeed(speed);
	}

	public void GiveMoveEllipse(Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		if (moveEllipse == null) moveEllipse = gameObject.AddComponent<MoveInEllipse>();
		else moveEllipse.enabled = true;

			moveEllipse.size = size;
		moveEllipse.SetSpeed(speedX, speedY);
		moveEllipse.t = Vector2.zero;
	}

	/// <summary>
	/// specifies an center to determine the angle, a size, and a speed
	/// </summary>
	public void GiveMoveEllipse(Vector2 center, Vector2 size, UpdatableFloat speedX, UpdatableFloat speedY)
	{
		if (moveEllipse == null) moveEllipse = gameObject.AddComponent<MoveInEllipse>();
		else moveEllipse.enabled = true;

		moveEllipse.SetOnEllipse(center, size);
		moveEllipse.SetSpeed(speedX, speedY);
	}
}