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
    [Range(0, 100)] public int CollectMP = 0;
}
