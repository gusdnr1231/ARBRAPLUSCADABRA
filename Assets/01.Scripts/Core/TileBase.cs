using DG.Tweening;
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
    PlayerHere = 2,
    EnemyAttack = 3,
    EnemyHere = 4,
    End
}

public class TileBase : MonoBehaviour
{
    [Header("Tile Data")]
    public TileData M_TileData;
    [SerializeField] private Sprite[] TileSprites;

    private Animator TileAnimator;
    private SpriteRenderer TileRenderer;

	public void SetUpTileData(int InitPositionX, int InitPositionY, TileState InitState)
    {
        if(TileAnimator == null) TryGetComponent(out TileAnimator);
        if(TileRenderer == null) TryGetComponent(out TileRenderer);
        M_TileData = new TileData() { TileInstance = this };
        M_TileData.TilePositionX = InitPositionX;
        M_TileData.TilePositionY = InitPositionY;
        if(InitState != TileState.End) M_TileData.InstanceTileState = InitState;

        TileAnimator.SetTrigger("Appear");
    }

	public void UpdateTileState(TileState InitState)
	{
		M_TileData.InstanceTileState = InitState;
        ChangeSprite(M_TileData.InstanceTileState);
	}

    public void SetTileAnimation()
    {
		TileAnimator.SetTrigger("Disappear");
	}

    public Vector3 ReturnTilePosition()
    {
        return this.transform.position;
    }

    public void ChangeSprite(TileState initState)
	{
		switch(initState)
        {
            case TileState.None:
                TileRenderer.sprite = TileSprites[0];
				break;
            case TileState.PlayerAttack:
                TileRenderer.sprite = TileSprites[1];
                break;

            case TileState.EnemyAttack:
                TileRenderer.sprite = TileSprites[2];
                break;

            default: break;
        }
    }
}
