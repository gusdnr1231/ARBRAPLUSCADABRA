using UnityEngine;

public enum SpellTypeEnum
{
    High = 0,
    Low = 1
}

public class MonoSpellBase : ScriptableObject
{
    [Header("Base Spell Value")]
    public SpellTypeEnum SpellType;
    public string SpellName;
    public Sprite SpellIcon;
    [Range(0, 20)] public int CollectMP = 0;
	[Range(0, 20)] public int Damage;
}
