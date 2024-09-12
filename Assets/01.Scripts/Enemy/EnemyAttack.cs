using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
	[SerializeField]
	public LowSpellBase AttackData;

	private EnemyMain _enemy;

	public void Initialize(EnemyMain enemy)
	{
		_enemy = enemy;
	}

	public List<AttackZoneElement> CalculateAttackRange()
	{
		if (AttackData == null)
		{
			Debug.LogError("AttackData is null.");
			return null;
		}

		if (AttackData.AttackZone == null || AttackData.AttackZone.Count == 0)
		{
			Debug.LogError("AttackZone is not set to the right value.");
			return null;
		}

		List<AttackZoneElement> CalculateData = new List<AttackZoneElement>();
		Vector2Int distanceToCenter = _enemy.CurrentTileNumber - (Vector2Int.one * 3);
		Vector2Int calculatedPos = Vector2Int.zero;

		for (int x = 0; x < AttackData.MapSize; x++)
		{
			AttackZoneElement CalculatedZone = new AttackZoneElement();

			for (int y = 0; y < AttackData.MapSize; y++)
			{
				calculatedPos.x = x + distanceToCenter.x;
				calculatedPos.y = y + distanceToCenter.y;

				Debug.Log(calculatedPos);

				if (IsWithinMapBounds(calculatedPos) == true)
				{
					if (calculatedPos.x < AttackData.AttackZone.Count &&
						calculatedPos.y < AttackData.AttackZone[calculatedPos.x].Line.Count)
					{
						CalculatedZone.Line.Add(AttackData.AttackZone[calculatedPos.x].Line[calculatedPos.y]);
					}
					else
					{
						Debug.LogWarning($"OutOfRange In AttackZone: {calculatedPos.x}, {calculatedPos.y}");
					}
				}
				else if (IsWithinMapBounds(calculatedPos) == false)
				{
					continue;
				}
			}
			CalculateData.Add(CalculatedZone);
		}

		return CalculateData;
	}


	private bool IsWithinMapBounds(Vector2Int position)
	{
		return position.x >= 0 && position.x < AttackData.MapSize &&
			   position.y >= 0 && position.y < AttackData.MapSize;
	}
}
