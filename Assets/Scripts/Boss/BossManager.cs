using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
	public static BossManager Instance { get; private set; }

	public BossData bossData;
	public Boss boss { get; private set; }
		
	private void Awake()
	{
		if (Instance != null && Instance != this) { Destroy(gameObject); return; }
		Instance = this;

		// literal garbage :)
		boss = bossData.CreateBossInstance(this);

		boss.EnterPhase(0);
	}

	private void Update()
	{
		boss.Update();
	}
}
