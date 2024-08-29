using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
	private readonly int AppearTriggerHash = Animator.StringToHash("Appear");
	private readonly int DisappearTriggerHash = Animator.StringToHash("Disappear");

	[Header("Tile Renderer Datas")]
	[SerializeField] private Sprite[] TileSprites;
	[SerializeField][ColorUsage(true, true)] private Color[] OutlineColors;
	[SerializeField][ColorUsage(true, true)] private Color[] GlowColors;

	[Header("Tile Rendering Values")]
	[SerializeField] private float OutlineDuration = 0.5f;
	[SerializeField] private float GlowDuration = 0.5f;

	private TileBase _tileBase;

	private SpriteRenderer Renderer;
	private Animator Animator;
	private Material TileMaterial;

	public void Initialize(TileBase tileBase)
	{
		_tileBase = tileBase;

		if (Animator == null) TryGetComponent(out Animator);
		if (Renderer == null) TryGetComponent(out Renderer);
		TileMaterial = Renderer.material;

		_tileBase.OnStateChange += ChangeShaderToState;
		_tileBase.SetActiveTile += SetTileAnimation;
		_tileBase.OnCastingTile += CastingRendering;
	}

	public void ChangeShaderToState(TileState initState)
	{
		switch (initState)
		{
			case TileState.None:
				SetOutlineFade(1f);
				SetOutlineColor(OutlineColors[0]);
				break;
				
			case TileState.PlayerHere:
				SetOutlineFade(1f);
				SetOutlineColor(OutlineColors[1]);
				break;
				
			case TileState.EnemyHere:
				SetOutlineFade(1f);
				SetOutlineColor(OutlineColors[2]);
				break;
				
			case TileState.ItemHere:
				SetOutlineFade(1f);
				SetOutlineColor(OutlineColors[3]);
				break;

			case TileState.PlayerAttack:
				SetGlowFade(1f);
				SetGlowColor(GlowColors[1]);
				break;

			case TileState.EnemyAttack:
				SetGlowFade(1f); 
				SetGlowColor(GlowColors[2]);
				break;

			default: break;
		}
	}

	public void CastingRendering(bool isCastingPlayer, bool isCastingEnemy)
	{
		if (isCastingPlayer == true) Renderer.sprite = TileSprites[1];
		else if (isCastingPlayer == false && isCastingEnemy == true) Renderer.sprite = TileSprites[2];
		else if (isCastingPlayer == false && isCastingEnemy == false)
		{
			Renderer.sprite = TileSprites[0];
			SetGlow(0f, GlowColors[0]);
		}
	}

	public void SetTileAnimation(bool isAppear)
	{
		Animator.SetTrigger(isAppear ? AppearTriggerHash : DisappearTriggerHash);
	}

	public void SetOutline(float OutlineFade, Color OutlineColor)
	{
		SetOutlineFade(OutlineFade);
		SetOutlineColor(OutlineColor);
	}

	public void SetGlow(float GlowFade, Color GlowColor)
	{
		SetGlowFade(GlowFade);
		SetGlowColor(GlowColor);
	}

	public void SetOutlineFade(float end)
	{
		TileMaterial.DOKill();
		TileMaterial.DOFloat(end, "_PixelOutlineFade", OutlineDuration);
	}

	public void SetOutlineColor(Color color)
	{
		TileMaterial.SetColor("_PixelOutlineColor", color);
	}
	public void SetGlowFade(float end)
	{
		TileMaterial.DOKill();
		TileMaterial.DOFloat(end, "_SineGlowFade", GlowDuration);
	}

	public void SetGlowColor(Color color)
	{
		TileMaterial.SetColor("_SineGlowColor", color);
	}
}
