using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossAction
{
	private BossManager manager;

	public float duration;
	public float elapsed = 0f;

	public event Action OnStart;
	public event Action OnEnd;

	public Func<bool> EndConditions;

	/// <summary>
	/// create a custom update loop with coroutines, passed duration
	/// </summary>
	public List<Func<float, IEnumerator>> updateCoroutines = new();
	private List<Coroutine> runningCoroutines = new();

	public BossAction(float duration)
	{
		this.duration = duration;
	}

	public void Start()
	{
		OnStart?.Invoke();

		elapsed = 0f;

		foreach (Func<float, IEnumerator> coroutine in updateCoroutines)
		{
			runningCoroutines.Add(manager.StartCoroutine(coroutine(duration)));
		}
	}

	public void End()
	{
		OnEnd?.Invoke();

		foreach (Coroutine coroutine in runningCoroutines)
		{
			manager.StopCoroutine(coroutine);
		}

		runningCoroutines.Clear();
	}

	public bool CheckExitCondition()
	{
		if (elapsed >= duration) return true;

		foreach (Func<bool> condition in EndConditions?.GetInvocationList())
		{
			if (condition()) return true;
		}

		return false;
	}

	public static Func<BossAction> RandomBetween(params BossAction[] bossActions)
	{
		return () =>
		{
			int selected = UnityEngine.Random.Range(0, bossActions.Length);
			return bossActions[selected];
		};
	}
}
