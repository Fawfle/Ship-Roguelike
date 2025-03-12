using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TestBossData", menuName = "Scriptable Objects/BossData/TestBoss")]
public class TestBossData : BossData
{
	public override Type boss { get => typeof(TestBoss); }

	public Bullet basicBulletPrefab;

	[Header("Phase 1")]
	public float phase1Health = 10f;
	[Header("Attack 1")]
	public float attack1Duration = 4f;
	public float attack1BulletSpeed = 1f;
	public float attack1SpawnPeriod = 1f;

	[Header("Attack 2")]
	public float attack2Duration = 4f;
	public float attack2BulletSpeed = 1f;
	public float attack2SpawnPeriod = 1f;

	[Header("Phase 2")]
	public float phase2Health = 5f;
	[Header("Attack 3")]
	public float attack3Duration = 4f;
	public float attack3BulletSpeed = 2f;
	public float attack3SpawnPeriod = 0.5f;
}
