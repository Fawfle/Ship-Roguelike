using UnityEngine;

// rb required for ontriggerenter
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BossHitbox : MonoBehaviour
{
	public bool contactDamage = true;

	// assume gameobject is on correct physics layer for collisions
	private void OnTriggerEnter2D(Collider2D collision)
	{
		BossManager.Instance.boss.TakeDamage(1);
	}
}
