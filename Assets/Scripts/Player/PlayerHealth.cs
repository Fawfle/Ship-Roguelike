using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public int health;

	public event Action OnDeath;

	[SerializeField] private bool invincible = false;

	private void Start()
	{
		health = PlayerManager.Instance.playerData.health.maxHealth;
	}

	public void TakeDamage(int damage)
	{
		if (invincible) return; // play diff sound?

		health -= damage;

		StartCoroutine(HitInvincibility());

		if (health <= 0) Die();
	}

	private IEnumerator HitInvincibility()
	{
		invincible = true;

		yield return new WaitForSeconds(PlayerManager.Instance.playerData.health.hitInvincibilityTime);

		invincible = false;
		
	}

	// assume collisions are on correct layer
	private void OnTriggerEnter2D(Collider2D collision)
	{
		TakeDamage(1);
	}

	private void Die()
	{
		OnDeath?.Invoke();
	}
}
