using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerMovement : MonoBehaviour, IPlayerComponent
{
	[Header("Player Move Value")]
	[SerializeField] private float MoveDuration = 0.5f;

	// Mobility Conditions
	private bool CanMoveX(int moveTo) =>
		_player.CurrentTileNumber.x + moveTo >= 0 && _player.CurrentTileNumber.x + moveTo <= 6 && CanMove && !IsMove;

	private bool CanMoveY(int moveTo) =>
		_player.CurrentTileNumber.y + moveTo >= 0 && _player.CurrentTileNumber.y + moveTo <= 6 && CanMove && !IsMove;

	public event Action<Vector2> OnMovement;

	public bool CanMove { get; set; }
	public bool IsMove { get; set; }
	public bool CanCasting { get; set; }
	public bool OnCasting { get; set; }

	private PlayerMain _player;
	private PlayerAnimator _playerAnimator;

	public void Initialize(PlayerMain player)
	{
		_player = player;
		_player.OnSetUpPlayer += SetUpPlayerMovement;

		CanMove = true;
		IsMove = false;
		CanCasting = true;
		OnCasting = false;

		_playerAnimator = _player.GetCompo<PlayerAnimator>();
	}

	private void Update()
	{
		HandleInput();
	}

	private void SetUpPlayerMovement()
	{
		//Addind Activate Method's when Player Reset
		CheckAndMoveToTile(false);
	}

	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.W) && CanMoveX(-1))
		{
			MovePlayer(Vector2Int.left);
		}

		if (Input.GetKeyDown(KeyCode.S) && CanMoveX(1))
		{
			MovePlayer(Vector2Int.right);
		}

		if (Input.GetKeyDown(KeyCode.A) && CanMoveY(-1))
		{
			MovePlayer(Vector2Int.down);
		}

		if (Input.GetKeyDown(KeyCode.D) && CanMoveY(1))
		{
			MovePlayer(Vector2Int.up);
		}
	}

	private void MovePlayer(Vector2Int moveTo)
	{
		MapManager.Instance.SettedTiles[_player.CurrentTileNumber].SetOnCharacter(null);
		_player.CurrentTileNumber.x += moveTo.x;
		_player.CurrentTileNumber.y += moveTo.y;

		OnMovement?.Invoke(moveTo);

		CheckAndMoveToTile();
	}

	public void CheckAndMoveToTile(bool UseDotween = true)
	{
		if (MapManager.Instance.SettedTiles.TryGetValue(_player.CurrentTileNumber, out TileBase movedTile))
		{
			if (UseDotween)
			{
				MoveWithDotween(movedTile);
			}
			else
			{
				transform.position = movedTile.ReturnTilePosition();
			}
			MapManager.Instance.SettedTiles[_player.CurrentTileNumber].SetOnCharacter(_player);
		}
	}

	private void MoveWithDotween(TileBase movedTile)
	{
		transform.DOMove(movedTile.ReturnTilePosition(), MoveDuration)
			.SetEase(Ease.OutBack)
			.OnStart(() =>
			{
				IsMove = true;
			})
			.OnComplete(() =>
			{
				IsMove = false;
				Debug.Log($"Move End : CanMove[{CanMove}]");
			});
	}
}
