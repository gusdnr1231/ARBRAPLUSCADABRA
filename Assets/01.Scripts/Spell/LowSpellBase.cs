using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

[System.Serializable]
public struct AttackZoneElement
{
	public List<bool> Line;
}

[CreateAssetMenu(fileName = "New Low Spell", menuName = "Spell/Low")]
public class LowSpellBase : MonoSpellBase
{
	[Header("Low Spell Value")]
	public List<AttackZoneElement> AttackZone = new List<AttackZoneElement>(7);
}