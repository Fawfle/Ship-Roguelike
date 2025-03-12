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
	/// coroutines that are run on start (take in a float of the duration)
	/// </summary>
	private List<Func<float, IEnumerator>> coroutines = new();
	private List<Coroutine> runningCoroutines = new();

	public BossAction(BossManager manager, float duration)
	{
		this.manager = manager;
		this.duration = duration;
	}

	public void Start()
	{
		OnStart?.Invoke();

		elapsed = 0f;

		foreach (Func<float, IEnumerator> coroutine in coroutines)
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

	public void Update()
	{
		elapsed += Time.deltaTime;
	}

	public bool CheckExitConditions()
	{
		if (elapsed >= duration) return true;

		if (EndConditions != null)
		{
			foreach (Func<bool> condition in EndConditions?.GetInvocationList())
			{
				if (condition()) return true;
			}
		}

		return false;
	}

	public void AddCoroutine(Func<IEnumerator> routine)
	{
		coroutines.Add((float _) => routine());
	}

	public void AddCoroutine(Func<float, IEnumerator> routine)
	{
		coroutines.Add(routine);
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
