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

[CreateAssetMenu(fileName = "New High Spell", menuName = "Spell/High")]
public class HighSpellBase : MonoSpellBase
{
	[Header("High Spell Value")]
	public HighSpellTypeEnum HighSpellType;
}
