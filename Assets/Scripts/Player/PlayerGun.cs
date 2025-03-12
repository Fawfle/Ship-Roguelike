using UnityEngine;

public class PlayerGun : MonoBehaviour
{
	private PlayerData.GunData data;

    private float shootCooldown = 0f;

	public bool canShoot => shootCooldown <= 0f;

	private void Awake()
	{
		data = PlayerManager.Instance.playerData.shooter;
	}

	private void Update()
	{
		if (shootCooldown >= 0) shootCooldown -= Time.deltaTime;

		if (canShoot && InputManager.Shoot.IsPressed()) Shoot();
	}

	public void Shoot()
	{
		BulletManager.CreateBullet(data.bulletPrefab, transform.position).GiveMoveDirection(Vector2.up, new(data.bulletSpeed));

		shootCooldown = 1 / data.firerate;
	}
}
