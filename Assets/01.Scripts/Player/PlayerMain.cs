using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoCharacter, IDamageable
{
	//Player Componets
	private Animator PlayerAnimator;
	private SpriteRenderer PlayerRenderer;

	[Header("Player Children")]
	[SerializeField] private GameObject PlayerVisual;

	[Header("Player Move Value")]
	[SerializeField] private float MoveDuration = 0.5f;

	//Use To Move
	private Vector2Int CurrentTileNumber;
	private bool CanMoveUpX => CurrentTileNumber.x - 1 >= 0 && CanMove == true;
	private bool CanMoveDownX => CurrentTileNumber.x + 1<= 6 && CanMove == true;
	private bool CanMoveUpY => CurrentTileNumber.y - 1 >= 0 && CanMove == true;
	private bool CanMoveDownY => CurrentTileNumber.y + 1 <= 6 && CanMove == true;

	private bool CanMove = true;
	private bool CanAttack = true;
	private bool IsAttack = false;

	private void Awake()
	{
		MapManager.OnCompleteLoadingMap += SetUpPlayer;
	}

	private void Start()
	{
		PlayerVisual.TryGetComponent(out PlayerAnimator);
		PlayerVisual.TryGetComponent(out PlayerRenderer);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			if (CanMoveUpX)
			{
				CurrentTileNumber.x = CurrentTileNumber.x - 1;
				PlayerRenderer.flipX = false;
			}
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			if (CanMoveDownX)
			{
				CurrentTileNumber.x = CurrentTileNumber.x + 1;
				PlayerRenderer.flipX = true;
			}
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			if (CanMoveUpY)
			{
				CurrentTileNumber.y = CurrentTileNumber.y - 1;
				PlayerRenderer.flipX = true;
			}
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			if (CanMoveDownY)
			{
				CurrentTileNumber.y = CurrentTileNumber.y + 1;
				PlayerRenderer.flipX = false;
			}
			CheckAndMoveToTile();
		}
	}

	public void SetUpPlayer()
	{
		CurrentHP = MaxHP;

		CanMove = true;
		CanAttack = true;
		IsAttack = false;

		CurrentTileNumber = new Vector2Int(3, 3);

		transform.position = MapManager.Instance.SettedTiles[CurrentTileNumber].ReturnTilePosition();
	}

	#region Player Move Methods

	private void CheckAndMoveToTile(bool UseDotween = true)
	{
		if (MapManager.Instance.SettedTiles.TryGetValue(CurrentTileNumber, out TileBase MovedTile))
		{
			if (UseDotween == true)
			{
				transform.DOMove(MovedTile.ReturnTilePosition(), MoveDuration).SetEase(Ease.OutBack)
					.OnStart(() =>
					{
						CanMove = false;
						CanAttack = false;
					})
					.OnComplete(() =>
					{
						CanMove = true;
						CanAttack = true;
					});
			}
			else if(UseDotween == false)
			{
				transform.position = MovedTile.ReturnTilePosition();
			}
		}
	}

	#endregion

	#region Damageable Methods

	public void TakeDamage(float damage)
	{
		CurrentHP = CurrentHP - damage;
		if (CurrentHP <= 0) Die();
	}

	public void TakeHeal(float heal)
	{
		CurrentHP = CurrentHP + heal;
	}

	public void Die()
	{
	}

	#endregion
}
