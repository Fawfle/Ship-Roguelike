using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public PlayerData playerData;

	[HideInInspector] public PlayerMovement movement;
	[HideInInspector] public PlayerGun gun;
	[HideInInspector] public PlayerHealth health;

	private void Awake()
	{
		if (Instance != null && Instance != this) { Destroy(gameObject); return; }
		Instance = this;

		movement = GetComponent<PlayerMovement>();
		gun = GetComponent<PlayerGun>();
		health = GetComponent<PlayerHealth>();
	}
}
