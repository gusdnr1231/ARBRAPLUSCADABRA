using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoSingleton<MapManager>
{
	[Header("Containers")]
	[SerializeField] private Transform TileContainer;

	[Header("Tile Values")]
    [SerializeField] private TileBase TileObject;
	[SerializeField] private Vector2 StartPosition;
	[SerializeField] private int MapSize = 7;

	public Dictionary<Vector2Int, TileBase> SettedTiles = new Dictionary<Vector2Int, TileBase>();
	public static Action OnCompleteLoadingMap;

	private bool isActiveMap;
	private Coroutine MappingCoroutine;

	private void Start()
	{
		MappingCoroutine = StartCoroutine(SetMapTile());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q) && MappingCoroutine == null)
		{
			MappingCoroutine = StartCoroutine(SetActiveMapTile(!isActiveMap));
		}
	}

	private IEnumerator SetMapTile()
	{
		Vector2 TilePosition;
		Vector2Int TileArrayNumber;
		for (int countX = 0; countX < MapSize; countX++)
		{
			for (int countY = 0; countY < MapSize; countY++)
			{
				TilePosition = new Vector2(StartPosition.x - countX + countY, StartPosition.y - (0.5f * countX) - (0.5f * countY));
				TileBase NewTile = Instantiate(TileObject, TilePosition, Quaternion.identity);

				TileArrayNumber = new Vector2Int(countX, countY);
				NewTile.SetUpTileData(TileArrayNumber.x, TileArrayNumber.y, TileState.None);

				NewTile.transform.SetParent(TileContainer, false);
				NewTile.gameObject.name = NewTile.gameObject.name.Replace("(Clone)", "");
				
				SettedTiles.TryAdd(TileArrayNumber, NewTile);

				yield return new WaitForSeconds(0.001f);
			}
		}

		isActiveMap = true;
		
		OnCompleteLoadingMap?.Invoke();
		MappingCoroutine = null;
	}

	private IEnumerator SetActiveMapTile(bool isActive)
	{
		Vector2Int TileArrayNumber;
		for (int countX = MapSize - 1; countX >= 0; countX--)
		{
			for (int countY = MapSize - 1; countY >= 0; countY--)
			{
				TileArrayNumber = new Vector2Int(countX, countY);
				SettedTiles[TileArrayNumber].ActionSetActiveTile(isActive);
				yield return new WaitForSeconds(0.01f);
			}
		}

		isActiveMap = isActive;
		MappingCoroutine = null;
	}
}
