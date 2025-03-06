using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class containing information about a boss
/// </summary>
public abstract class Boss 
{
	private BossManager manager;

	public List<BossPhase> phases;
	public BossPhase currentPhase;

	public abstract void Initialize();

	// sample
	//
	// phase1 = new BossPhase(health);
	//
	// phase1.updateCoroutines.Add(SpawnParticles);
	// phase1.OnEnter += SpawnAnimation;
	//
	// attack1 = new BossAction(duration);
	// attack1.OnStart += SpawnBullets;
	// attack1.OnStart += MoveToCenter;
	// attack1.OnEnd += BurstBullets;
	// attack1.updateCoroutines(SpawnHomingBullets(frequency, duration));
	// attack1.ExitConditions += IsAtCenter;
	// 
	// phase1.AddAction(attack1);
	// phase1.AddAction(() => isCondition ? attack2 : attack3);
	// phase1.AddAction(BossAction.RandomBetween(attack3, attack4));
	//
	//
	// AddPhase(phase1);
	//


	public abstract void Update();

	public void EnterPhase(int phase)
	{
		currentPhase?.Exit();

		currentPhase = phases[phase];

		currentPhase.Enter();
	}
}
