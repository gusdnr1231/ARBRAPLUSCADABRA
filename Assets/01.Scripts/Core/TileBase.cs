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
    PlayerMove = 2,
    EnemyAttack = 3,
    EnemyMove = 4,
    End
}

public class TileBase : MonoBehaviour
{
    [Header("Tile Data")]
    public TileData M_TileData;
    
    private Animator TileAnimator;
    private SpriteRenderer TileRenderer;

    private Coroutine ColorChangedCoroutine;

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
	}

    public void SetTileAnimation()
    {
		TileAnimator.SetTrigger("Disappear");
	}

    public Vector3 ReturnTilePosition()
    {
        return this.transform.position;
    }

    public void ChangeColor(Color AfterColor, float WaitTime)
	{
		TileRenderer.color = AfterColor;
        if (ColorChangedCoroutine != null)
        {
            StopAllCoroutines();
            TileRenderer.color = Color.white;
        }

        ColorChangedCoroutine = StartCoroutine(ChangeColorCoroutine(AfterColor, WaitTime));
    }

    private IEnumerator ChangeColorCoroutine(Color AfterColor, float WaitTime)
    {
        Debug.Log("Start Change Tile Color");
		TileRenderer.color = AfterColor;
		yield return new WaitForSeconds(WaitTime);
		TileRenderer.color = Color.white;
        Debug.Log("End Change Tile Color");
	}
}
