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
    /// coroutines that are run on enter
    /// </summary>
    public List<Func<IEnumerator>> coroutines = new();
    public List<Coroutine> runningCoroutines = new();

    public List<Func<BossAction>> actions = new();
    private BossAction currentAction;
    private int currentActionIndex;

    public BossPhase(BossManager manager, float health)
    {
        this.manager = manager;
        this.health = health;
        this.maxHealth = health;
    }

    public void Enter()
    {
        OnEnter?.Invoke();

		foreach (Func<IEnumerator> coroutine in coroutines)
		{
			manager.StartCoroutine(coroutine());
		}

        StartAction(0);
	}

    public void Exit()
    {
        OnExit?.Invoke();

		foreach (Coroutine coroutine in runningCoroutines)
		{
			manager.StopCoroutine(coroutine);
		}

        runningCoroutines.Clear();

        currentAction.End();
	}

	public void Update()
	{
        currentAction.Update();
		if (currentAction.CheckExitConditions()) StartNextAction();
	}

	public void StartAction(int actionIndex)
    {
        //Debug.Log("starting action... " + actionIndex);
        currentAction?.End();

        currentAction = actions[actionIndex]();
        currentActionIndex = actionIndex;

        currentAction?.Start();
    }

    public void StartNextAction()
    {
        StartAction((currentActionIndex + 1) % actions.Count);
    }

    /// <returns>If the phase has ended</returns>
    public bool DealDamage(float damage)
    {
        health -= damage;
        

        return health <= 0f;
    }

    public void AddAction(BossAction action)
    {
        actions.Add(() => action);
    }

	public void AddAction(Func<BossAction> multiAction)
	{
		actions.Add(multiAction);
	}

    /// <summary>
    /// creates and adds a new single action to the phase
    /// </summary>
    /// <returns>the new action</returns>
    public BossAction AddNewAction(float duration)
    {
        BossAction action = new(manager, duration);

        actions.Add(() => action);
        
        return action;
    }
}
