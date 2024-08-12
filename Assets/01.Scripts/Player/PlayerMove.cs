using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	private Vector2Int CurrentTileNumber;

	private bool CanMoveX => CurrentTileNumber.x - 1 >= 0 || CurrentTileNumber.x + 1 <= 6;
	private bool CanMoveY => CurrentTileNumber.y - 1 >= 0 || CurrentTileNumber.y + 1 <= 6;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			CurrentTileNumber = new Vector2Int(3, 3);

			transform.position = MapManager.Instance.SettedTiles[CurrentTileNumber].ReturnTilePosition();
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			if(CanMoveX) CurrentTileNumber.x = CurrentTileNumber.x - 1;
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.S))
		{
			if (CanMoveX) CurrentTileNumber.x = CurrentTileNumber.x + 1;
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			if (CanMoveY) CurrentTileNumber.y = CurrentTileNumber.y - 1;
			CheckAndMoveToTile();
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			if (CanMoveY) CurrentTileNumber.y = CurrentTileNumber.y + 1;
			CheckAndMoveToTile();
		}
	}

	private void CheckAndMoveToTile()
	{
		if (MapManager.Instance.SettedTiles.TryGetValue(CurrentTileNumber, out TileBase MovedTile))
		{
			transform.position = MovedTile.ReturnTilePosition();
		}
	}
}
