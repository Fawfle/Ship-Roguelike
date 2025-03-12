using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
	public MovementData movement;
	public GunData shooter;
	public HealthData health;

	[System.Serializable]
	public class MovementData
	{
		[Header("Movement")]
		public float speed = 6f;
		public float sneakDamping = 0.5f;

		[Header("Dash")]
		public float dashStartSpeed = 20f;
		public float dashEndSpeed = 10f;
		public float dashDuration = 0.3f;
		public float dashCooldown = 0.8f;

		[Tooltip("Max angle player can change dash")]
		public float dashAngleChangeDegrees = 45f;

		[Header("Stall")]
		public float stallDuration = 1f;
	}

	[System.Serializable]
	public class GunData
	{
		[Header("Shoot")]
		public Bullet bulletPrefab;

		public float firerate = 1f;
		public float bulletSpeed = 2f;
	}

	[System.Serializable]
	public class HealthData
	{
		[Header("Health")]
		public int maxHealth = 5;

		public float hitInvincibilityTime = 0.1f;
	}
}
