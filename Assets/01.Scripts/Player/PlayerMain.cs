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

	//Mobility Conditions
	private bool CanMoveUpX => CurrentTileNumber.x - 1 >= 0 && CanMove == true && IsMove == false;
	private bool CanMoveDownX => CurrentTileNumber.x + 1 <= 6 && CanMove == true && IsMove == false;
	private bool CanMoveUpY => CurrentTileNumber.y - 1 >= 0 && CanMove == true && IsMove == false;
	private bool CanMoveDownY => CurrentTileNumber.y + 1 <= 6 && CanMove == true && IsMove == false;

	public bool CanMove { get; set; }
	public bool IsMove { get; set; }
	public bool CanCasting { get; set; }
	public bool OnCasting { get; set; }

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
			Debug.Log($"Input W : CanMove[{CanMove}]");
			if (CanMoveUpX)
			{
				CurrentTileNumber.x = CurrentTileNumber.x - 1;
				PlayerRenderer.flipX = false;

				CheckAndMoveToTile();
			}
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			Debug.Log($"Input S : CanMove[{CanMove}]");
			if (CanMoveDownX)
			{
				CurrentTileNumber.x = CurrentTileNumber.x + 1;
				PlayerRenderer.flipX = true;

				CheckAndMoveToTile();
			}
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log($"Input A : CanMove[{CanMove}]");
			if (CanMoveUpY)
			{
				CurrentTileNumber.y = CurrentTileNumber.y - 1;
				PlayerRenderer.flipX = true;

				CheckAndMoveToTile();
			}
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			Debug.Log($"Input D : CanMove[{CanMove}]");
			if (CanMoveDownY)
			{
				CurrentTileNumber.y = CurrentTileNumber.y + 1;
				PlayerRenderer.flipX = false;

				CheckAndMoveToTile();
			}
		}
	}

	public void SetUpPlayer()
	{
		CurrentHP = MaxHP;

		CanMove = true;
		CanCasting = true;
		OnCasting = false;

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
						IsMove = true;
					})
					.OnComplete(() =>
					{
						IsMove = false;
					});
			}
			else if (UseDotween == false)
			{
				transform.position = MovedTile.ReturnTilePosition();
			}
		}
	}

	#endregion

	#region Damageable Methods

	public void TakeDamage(float damage, HighSpellTypeEnum AttackedType)
	{
		CurrentHP = CurrentHP - damage;
		if (CurrentHP <= 0) Die();
	}

	public void TakeHeal(float heal)
	{
		CurrentHP = Mathf.Clamp(CurrentHP + heal,0, MaxHP);
	}

	public void Die()
	{
	}

	#endregion
}
