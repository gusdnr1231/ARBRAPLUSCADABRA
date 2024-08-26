using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerComponent
{
	//Player Componets
	private Animator Animator;
	private SpriteRenderer Renderer;

	private PlayerMain _player;
	private PlayerMovement _playerMovement;

	public void Initialize(PlayerMain player)
	{
		_player = player;

		Animator = GetComponent<Animator>();
		Renderer = GetComponent<SpriteRenderer>();

		_playerMovement = _player.GetCompo<PlayerMovement>();
		_playerMovement.OnMovement += FlipPlayer;
	}

	private void FlipPlayer(Vector2 movement)
	{
		Renderer.flipX = movement.x - movement.y > 0;
	}
}
