using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[System.Serializable]
public struct TileData
{
    public TileBase TileInstance;
    public int TilePositionX;
    public int TilePositionY;
    public TileState InstanceTileState;
	public MonoCharacter OnCharacter;
}

public enum TileState
{
    None = 0,
    PlayerHere = 1,
    EnemyHere = 2,
    ItemHere = 3,
    PlayerAttack = 4,
    EnemyAttack = 5,
    End
}

public class TileBase : MonoBehaviour
{
	[Header("Tile Data")]
    public TileData M_TileData;

    private TileRenderer _tileRenderer;

    private bool isOnCharacter = false;
    private IDamageable OnCharacterDamageable;

    public event Action<TileState> OnStateChange;
	public event Action<bool> SetActiveTile;

	public void SetUpTileData(int InitPositionX, int InitPositionY, TileState InitState)
    {
        _tileRenderer.Initialize(this);

		M_TileData = new TileData() { TileInstance = this };
        M_TileData.TilePositionX = InitPositionX;
        M_TileData.TilePositionY = InitPositionY;
        if(InitState != TileState.End) M_TileData.InstanceTileState = InitState;

        SetActiveTile?.Invoke(true);
    }

    public void ActionSetActiveTile(bool isActive) => SetActiveTile?.Invoke(isActive);

	public void UpdateTileState(TileState InitState)
	{
		M_TileData.InstanceTileState = InitState;
		OnStateChange?.Invoke(M_TileData.InstanceTileState);
	}

    public Vector3 ReturnTilePosition()
    {
        return this.transform.position;
    }

	public void SetOnCharacter(MonoCharacter character)
	{
		M_TileData.OnCharacter = character;
		isOnCharacter = M_TileData.OnCharacter.TryGetComponent(out OnCharacterDamageable);
	}

    public void ActiveDamageable(int value, HighSpellTypeEnum spellType, bool isAttack = true)
    {
        if(isOnCharacter == false) return;
        if(isAttack == true) OnCharacterDamageable.TakeDamage(value, spellType);
        else if(isAttack == false) OnCharacterDamageable.TakeHeal(value);
    }
}
