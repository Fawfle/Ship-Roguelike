using System;
using UnityEngine;

public abstract class BossData : ScriptableObject
{
	public abstract Type boss { get; }

	public Boss CreateBossInstance(BossManager manager) => (Boss)boss.GetConstructor(new System.Type[] { typeof(BossManager), typeof(BossData)}).Invoke(new object[] { manager, this });
}
