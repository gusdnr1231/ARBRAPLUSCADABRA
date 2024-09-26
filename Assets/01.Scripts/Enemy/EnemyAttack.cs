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
		Vector2Int distanceToCenter = new Vector2Int(_enemy.CurrentTileNumber.x - 3, _enemy.CurrentTileNumber.y - 3);
		Vector2Int calculatedPos = Vector2Int.zero;

		for (int x = 0; x < AttackData.MapSize; x++)
		{
			AttackZoneElement CalculatedZone = new AttackZoneElement();
			CalculatedZone.Line = new List<bool> { };

			for (int y = 0; y < AttackData.MapSize; y++)
			{
				calculatedPos.x = x + distanceToCenter.x;
				calculatedPos.y = y + distanceToCenter.y;


				if (IsWithinMapBounds(calculatedPos) == true)
				{
					Debug.Log(calculatedPos);
					Debug.Log(AttackData.AttackZone.Count);
					Debug.Log(AttackData.AttackZone[calculatedPos.x].Line.Count);

					if (calculatedPos.x < AttackData.AttackZone.Count &&
						calculatedPos.y < AttackData.AttackZone[calculatedPos.x].Line.Count)
					{
						CalculatedZone.Line.Add(AttackData.AttackZone[calculatedPos.x].Line[calculatedPos.y]);
					}
					else
					{
						CalculatedZone.Line.Add(false);
					}
				}
				else if (IsWithinMapBounds(calculatedPos) == false)
				{
					CalculatedZone.Line.Add(false);
				}
			}
			CalculateData.Add(CalculatedZone);
		}


		for (int tX = 0; tX < AttackData.MapSize; tX++)
		{
			string line = "";
			for (int tY = 0; tY < AttackData.MapSize; tY++)
			{
				if(CalculateData[tX].Line[tY] == true) line += "O ";
				else if(CalculateData[tX].Line[tY] == false) line += "X";
			}
			Debug.Log(line);
		}

		return CalculateData;
	}


	private bool IsWithinMapBounds(Vector2Int position)
	{
		return position.x >= 0 && position.x < AttackData.MapSize &&
			   position.y >= 0 && position.y < AttackData.MapSize;
	}
}
