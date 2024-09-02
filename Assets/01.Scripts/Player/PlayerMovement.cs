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
	private bool OnCharacter(int x = 0, int y = 0) =>
		_mapMng.SettedTiles[new Vector2Int(_player.CurrentTileNumber.x + x, _player.CurrentTileNumber.y + y)].isOnCharacter;

	private bool CanMoveX(int moveTo) =>
		_player.CurrentTileNumber.x + moveTo >= 0 && _player.CurrentTileNumber.x + moveTo <= 6 && CanMove && !IsMove && OnCharacter(moveTo, 0) == false;

	private bool CanMoveY(int moveTo) =>
		_player.CurrentTileNumber.y + moveTo >= 0 && _player.CurrentTileNumber.y + moveTo <= 6 && CanMove && !IsMove && OnCharacter(0, moveTo) == false;

	public event Action<Vector2> OnMovement;

	public bool CanMove { get; set; }
	public bool IsMove { get; set; }
	public bool CanCasting { get; set; }
	public bool OnCasting { get; set; }

	private PlayerMain _player;
	private PlayerAnimator _playerAnimator;

	private MapManager _mapMng;
	private GameManager _gameMng;

	public void Initialize(PlayerMain player)
	{
		_player = player;
		_player.OnSetUpPlayer += SetUpPlayerMovement;

		CanMove = true;
		IsMove = false;
		CanCasting = true;
		OnCasting = false;

		_mapMng = _player.mngs.GetManager<MapManager>();
		_gameMng = _player.mngs.GetManager<GameManager>();
		_playerAnimator = _player.GetCompo<PlayerAnimator>();
	}

	private void Update()
	{
		if(_gameMng.IsPassingNextTurn == false && _mapMng.isActiveMap == true) HandleInput();
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
		_mapMng.SettedTiles[_player.CurrentTileNumber].SetOnCharacter(null);
		_player.CurrentTileNumber.x += moveTo.x;
		_player.CurrentTileNumber.y += moveTo.y;

		OnMovement?.Invoke(moveTo);

		CheckAndMoveToTile();
	}

	public void CheckAndMoveToTile(bool UseDotween = true)
	{
		if (_mapMng.SettedTiles.TryGetValue(_player.CurrentTileNumber, out TileBase movedTile))
		{
			if (UseDotween)
			{
				MoveWithDotween(movedTile);
				_gameMng.UseMoveable();
			}
			else
			{
				transform.position = movedTile.ReturnTilePosition();
			}
			_mapMng.SettedTiles[_player.CurrentTileNumber].SetOnCharacter(_player);
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
			});
	}
}
