using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class containing information about a specific boss phase
/// </summary>
public class BossPhase
{
    private BossManager manager;

    public float maxHealth;
    public float health;

    public event Action OnEnter;
    public event Action OnExit;

    /// <summary>
    /// create a custom update loop with coroutines on start
    /// </summary>
    public List<Func<IEnumerator>> updateCoroutines = new();
    public List<Coroutine> runningCoroutines = new();

    public List<Func<BossAction>> pattern = new();
    private BossAction currentAction;

    public BossPhase(float health, BossManager manager)
    {
        this.manager = manager;
        this.health = health;
        this.maxHealth = health;
    }

    public void Enter()
    {
        OnEnter?.Invoke();

		foreach (Func<IEnumerator> coroutine in updateCoroutines)
		{
			manager.StartCoroutine(coroutine());
		}
	}

    public void Exit()
    {
        OnExit?.Invoke();

		foreach (Coroutine coroutine in runningCoroutines)
		{
			manager.StopCoroutine(coroutine);
		}

        runningCoroutines.Clear();
	}

	public void Update()
	{
		//if (currentAction.CheckExitCondition()) StartNextAction();
	}

	public void SetAction()
    {
        currentAction?.End();
    }

    public BossAction GetAction(int actionIndex)
    {
        return pattern[actionIndex]();
    }

    /// <returns>If the phase has ended</returns>
    public bool DealDamage(float damage)
    {
        health -= damage;
        

        return health <= 0f;
    }

    public void AddAction(BossAction action)
    {
        pattern.Add(() => action);
    }

	public void AddAction(Func<BossAction> multiAction)
	{
		pattern.Add(multiAction);
	}
}
