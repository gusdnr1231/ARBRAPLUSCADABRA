using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum HighSpellTypeEnum
{
	Normal = 0,
	Fire = 1,
	Electric = 2,
	Water = 3,
	Ground = 4,
	End
}

public struct HighSpellData
{
	public HighSpellTypeEnum HighSpellType;
	public string SpellName;
}

[CreateAssetMenu(fileName = "New High Spell", menuName = "Spell/High")]
public class HighSpellBase : MonoSpellBase, ISpell
{
	[Header("High Spell Value")]
	public HighSpellTypeEnum HighSpellType;

	private HighSpellData Data = new HighSpellData();
	
	private void PackagingData()
	{
		Data = new HighSpellData() { HighSpellType = HighSpellType };
		Data.SpellName = string.IsNullOrEmpty(SpellName) ? "" : SpellName;
	}

	public void CastSpell()
	{
	}

	public void UseSpell()
	{
	}

	public void CancelSpell()
	{
	}

}
