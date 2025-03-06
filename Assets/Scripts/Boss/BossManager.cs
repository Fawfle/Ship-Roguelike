using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BossManager : MonoBehaviour
{
	Boss boss;

	private void Awake()
	{
		boss.Initialize();

		
	}

	private void Update()
	{
		boss.Update();
	}
}
