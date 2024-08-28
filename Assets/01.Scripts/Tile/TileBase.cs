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

	private bool isCastingPlayer => M_TileData.InstanceTileState == TileState.PlayerAttack;
    private bool isCastingEnemy => M_TileData.InstanceTileState == TileState.EnemyAttack;
    private bool isOnCharacter = false;
    private IDamageable OnCharacterDamageable;

    public event Action<TileState> OnStateChange;
	public event Action<bool> SetActiveTile;
    public event Action<bool, bool> OnCastingTile;

	public void SetUpTileData(int InitPositionX, int InitPositionY, TileState InitState)
    {
		M_TileData = new TileData() { TileInstance = this };
        M_TileData.TilePositionX = InitPositionX;
        M_TileData.TilePositionY = InitPositionY;
        if(InitState != TileState.End) M_TileData.InstanceTileState = InitState;
        
		if(_tileRenderer == null) TryGetComponent(out _tileRenderer);
        _tileRenderer.Initialize(this);

		SetActiveTile?.Invoke(true);
		OnStateChange?.Invoke(InitState);
	}

    public void ActionSetActiveTile(bool isActive) => SetActiveTile?.Invoke(isActive);

	public void UpdateTileState(TileState InitState)
	{
		M_TileData.InstanceTileState = InitState;
		
		OnStateChange?.Invoke(M_TileData.InstanceTileState);
		OnCastingTile?.Invoke(isCastingPlayer, isCastingEnemy);
	}

    public Vector3 ReturnTilePosition()
    {
        return this.transform.position;
    }

	public void SetOnCharacter(MonoCharacter character)
	{
		M_TileData.OnCharacter = character;

        if(M_TileData.OnCharacter == null) OnStateChange?.Invoke(TileState.None);
		else if(M_TileData.OnCharacter != null)
        {
			OnStateChange?.Invoke((TileState)character.Character_Type);
			isOnCharacter = M_TileData.OnCharacter.TryGetComponent(out OnCharacterDamageable);
		}
	}

    public void ActiveDamageable(int value, HighSpellTypeEnum spellType, bool isAttack = true)
    {
        if(isOnCharacter == false) return;
        if(isAttack == true) OnCharacterDamageable.TakeDamage(value, spellType);
        else if(isAttack == false) OnCharacterDamageable.TakeHeal(value);
    }

    public void ResetTileRender()
    {
		if (M_TileData.OnCharacter == null) UpdateTileState(TileState.None);
		else if (M_TileData.OnCharacter != null)
		{
			UpdateTileState((TileState)M_TileData.OnCharacter.Character_Type);
			isOnCharacter = M_TileData.OnCharacter.TryGetComponent(out OnCharacterDamageable);
		}
	}
}
