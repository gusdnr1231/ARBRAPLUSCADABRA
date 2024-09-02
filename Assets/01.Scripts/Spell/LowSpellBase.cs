using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackZoneElement
{
	public List<bool> Line;
}

[CreateAssetMenu(fileName = "New Low Spell", menuName = "Spell/Low")]
public class LowSpellBase : MonoSpellBase
{
	[Header("Low Spell Value")]
	[Range(0, 10)] public int MapSize = 7;
	public List<AttackZoneElement> AttackZone;
}
