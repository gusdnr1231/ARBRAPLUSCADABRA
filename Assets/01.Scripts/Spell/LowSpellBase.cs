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

public struct LowSpellData
{
	public List<AttackZoneElement> AttackZone;
	public float Damage;
	public string SpellName;
}

[CreateAssetMenu(fileName = "New Low Spell", menuName = "Spell/Low")]
public class LowSpellBase : MonoSpellBase, ISpell
{
	[Header("Low Spell Value")]
	[Range(0f, 100f)] public float Damage;
	public List<AttackZoneElement> AttackZone = new List<AttackZoneElement>(7);


	public void CancelSpell()
	{
	}

	public void CastSpell()
	{
	}

	public void UseSpell()
	{
	}

	
}