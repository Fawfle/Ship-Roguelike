using System;
using System.Collections;
using UnityEngine;

public class TestBoss : Boss
{
	public Type boss => throw new NotImplementedException();

	private TestBossData data;

	public TestBoss(BossManager manager, BossData bossData) : base(manager, bossData) 
	{
		data = GetBossData<TestBossData>();

		BossPhase phase1 = AddNewPhase(data.phase1Health);

		BossAction attack1 = phase1.AddNewAction(data.attack1Duration);
		attack1.AddCoroutine(() => ShootBulletArcAtPlayer(data.attack1BulletSpeed, data.attack1SpawnPeriod));

		BossAction wait = new BossAction(manager, 1f);
		phase1.AddAction(wait);

		BossPhase phase2 = AddNewPhase(data.phase2Health);

		BossAction attack3 = phase2.AddNewAction(data.attack3Duration);
		attack3.AddCoroutine(() => ShootBulletArcAtPlayer(data.attack2BulletSpeed, data.attack2SpawnPeriod));
		attack3.AddCoroutine(() => ShootBulletsAtPlayer(data.attack3BulletSpeed, data.attack3SpawnPeriod));

		phase2.AddAction(wait);
	}

	public override void Update()
	{
		base.Update();
	}

	public IEnumerator ShootBulletsAtPlayer(float speed, float period)
	{
		while (true)
		{
			BulletManager
				.CreateBullet(data.basicBulletPrefab, manager.transform.position)
				.GiveMoveDirection(PlayerManager.Instance.transform.position - manager.transform.position, speed);

			yield return new WaitForSeconds(period);
		}
	}

	public IEnumerator ShootBulletArcAtPlayer(float speed, float period)
	{
		while (true)
		{
			BulletManager
				.CreateBullets(data.basicBulletPrefab, manager.transform.position, 10)
				.GiveMoveDirection(Vector2.zero, speed)
				.GetComponents<MoveInDirection>().MoveOutInArc(PlayerManager.Instance.transform.position - manager.transform.position, 10f);

			yield return new WaitForSeconds(period);
		}
	}
}
