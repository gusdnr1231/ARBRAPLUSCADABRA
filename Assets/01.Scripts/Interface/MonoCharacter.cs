using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CharacterType
{
    None = 0,
    Player = 1,
    Monster = 2,
    Item = 3,
    End
}

public class MonoCharacter : MonoBehaviour
{
    [Header("Character Stat")]
    public CharacterType Character_Type;
    [Range(1, 1000)]public int MaxHP = 10;
    public float CurrentHP = 0;
	[HideInInspector] public Vector2Int CurrentTileNumber;
}
