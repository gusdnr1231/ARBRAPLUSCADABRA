using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
	[SerializeField]
	public LowSpellBase AttackZone;

	private EnemyMain _enemy;

	public void Initialize(EnemyMain enemy)
	{
		_enemy = enemy;
	}
}
