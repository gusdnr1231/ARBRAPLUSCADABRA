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

		_tileBase.OnStateChange += ChangeSpriteToState;
		_tileBase.SetActiveTile += SetTileAnimation;
	}

	public void ChangeSpriteToState(TileState initState)
	{
		switch (initState)
		{
			case TileState.None:
				Renderer.sprite = TileSprites[0];
				SetMaterial(1f, 0f, OutlineColors[0], GlowColors[0]);
				break;
				
			case TileState.PlayerHere:
				SetMaterial(1f, 0f, OutlineColors[1], GlowColors[0]);
				break;
				
			case TileState.EnemyHere:
				SetMaterial(1f, 0f, OutlineColors[2], GlowColors[0]);
				break;
				
			case TileState.ItemHere:
				SetMaterial(1f, 0f, OutlineColors[3], GlowColors[0]);
				break;

			case TileState.PlayerAttack:
				Renderer.sprite = TileSprites[1];
				SetGlowFade(1f);
				SetGlowColor(GlowColors[1]);
				break;

			case TileState.EnemyAttack:
				Renderer.sprite = TileSprites[2];
				SetGlowFade(1f); 
				SetGlowColor(GlowColors[2]);
				break;

			default: break;
		}
	}
	public void SetTileAnimation(bool isAppear)
	{
		Animator.SetTrigger(isAppear ? AppearTriggerHash : DisappearTriggerHash);
	}

	private void SetMaterial(float OutlineFade, float GlowFade, Color OutlineColor, Color GlowColor)
	{
		SetOutlineFade(OutlineFade);
		SetOutlineColor(OutlineColor);
		SetGlowFade(GlowFade);
		SetGlowColor(GlowColor);
	}

	public void SetOutlineFade(float end)
	{
		TileMaterial.DOFloat(end, "_PixelOutlineFade", OutlineDuration);
	}

	public void SetOutlineColor(Color color)
	{
		TileMaterial.DOColor(color, "_PixelOutlineColor", OutlineDuration);
	}
	public void SetGlowFade(float end)
	{
		TileMaterial.DOFloat(end, "_SineGlowFade", GlowDuration);
	}

	public void SetGlowColor(Color color)
	{
		TileMaterial.DOColor(color, "_SineGlowColor", GlowDuration);
	}
}
