using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class containing information about a boss
/// </summary>
public abstract class Boss
{
	protected BossManager manager;
	private BossData bossData;

	public List<BossPhase> phases = new();
	public BossPhase currentPhase;

	public event Action OnDamage;
	public event Action OnDie;

	public Boss(BossManager manager, BossData bossData)
	{
		this.manager = manager;
		this.bossData = bossData;
	}

	public T GetBossData<T>() where T : BossData
	{
		return (T)bossData;
	}

	public virtual void Update()
	{
		currentPhase?.Update();
	}

	public void TakeDamage(float damage)
	{
		if (currentPhase == null) return;

		currentPhase.health -= damage;

		if (currentPhase.health <= 0)
		{
			int nextPhaseIndex = GetPhaseIndex + 1;
			if (nextPhaseIndex >= phases.Count) Die();
			else EnterPhase(nextPhaseIndex);
		}

		OnDamage?.Invoke();
	}

	public void Die()
	{
		currentPhase?.Exit();

		currentPhase = null;

		OnDie?.Invoke();
	}

	public void EnterPhase(int phase)
	{
		currentPhase?.Exit();

		Debug.Log("Entering phase: " + phase);

		currentPhase = phase >= 0 && phase < phases.Count ? phases[phase] : null;

		currentPhase?.Enter();
	}


	/// <summary>
	/// creates and adds a new phase to the boss
	/// </summary>
	/// <returns>the new phase</returns>
	public BossPhase AddNewPhase(float health)
	{
		BossPhase phase = new BossPhase(manager, health);

		phases.Add(phase);

		return phase;
	}

	public int GetPhaseIndex => phases.IndexOf(currentPhase);
}
