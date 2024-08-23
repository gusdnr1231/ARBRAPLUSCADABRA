using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoCharacter : MonoBehaviour
{
    [Header("Character Stat")]
    [Range(1, 1000)]public int MaxHP = 10;
    public float CurrentHP = 0;

	[HideInInspector] public Vector2Int CurrentTileNumber;
}
