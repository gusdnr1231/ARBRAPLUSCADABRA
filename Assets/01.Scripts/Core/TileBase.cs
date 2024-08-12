using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public TileBase TileInstance;
    public int TilePositionX;
    public int TilePositionY;
    public TileState InstanceTileState;
}

public enum TileState
{
    None = 0,
    PlayerAttack = 1,
    PlayerMove = 2,
    EnemyAttack = 3,
    EnemyMove = 4,
    End
}

public class TileBase : MonoBehaviour
{
    [Header("Tile Data")]
    public TileData M_TileData;
    
    private Animator TIleAnimator;

	public void SetUpTileData(int InitPositionX, int InitPositionY, TileState InitState)
    {
        if(TIleAnimator == null) TryGetComponent(out TIleAnimator);
        M_TileData = new TileData() { TileInstance = this };
        M_TileData.TilePositionX = InitPositionX;
        M_TileData.TilePositionY = InitPositionY;
        if(InitState != TileState.End) M_TileData.InstanceTileState = InitState;

        TIleAnimator.SetTrigger("Appear");
    }

	public void UpdateTileState(TileState InitState)
	{
		M_TileData.InstanceTileState = InitState;
	}

    public void SetTileAnimation()
    {
		TIleAnimator.SetTrigger("Disappear");
	}

    public Vector3 ReturnTilePosition()
    {
        return this.transform.position;
    }
}
