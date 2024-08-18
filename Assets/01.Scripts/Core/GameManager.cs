using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
	public void StartGame()
	{
	
	}

	public void StartTurn()
	{
		UpdateTurnCount();

		MonstersPatience();

		PlayerHandUpdate();

		ResetTurnTime();
	}

	private void UpdateTurnCount()
	{

	}

	private void MonstersPatience()
	{

	}

	private void PlayerHandUpdate()
	{

	}

	private void ResetTurnTime()
	{

	}
}
