using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>, IManagerComponent
{
	[HideInInspector] public PlayerMain Player;

	[Header("Player Values")]
	[Tooltip("�� ���� �� �÷��̾ ���� ī�� ��")]
	public int PlayerDrawSize = 5;
	[Tooltip("���� �÷��̾� �ൿ��")]
	public int CurrentMoveable = 0;
	[Tooltip("�ִ� �÷��̾� �ൿ��")]
	public int MaxMoveable = 999;
	[Tooltip("���� �Ѿ �� ��ٸ��� �ð�")]
	[Range(0.1f, 10f)] public float PassTime = 2.0f;

	[Header("UI Elements")]
	[SerializeField] private TMPro.TMP_Text TurnTxt;

	private int TurnCount;
	public bool IsPassingNextTurn { get; private set; } = false;

	public event Action<int> OnUpdatePlayerHand;
	public event Action EndTurn;

	private Managers _mngs;
	private SpellManager _spellMng;
	private MapManager _mapMng;

	public void Initialize(Managers managers)
	{
		_mngs = managers;

		Player = FindObjectOfType<PlayerMain>().GetComponent<PlayerMain>();

		_spellMng = _mngs.GetManager<SpellManager>();
			
		_mapMng = _mngs.GetManager<MapManager>();
		MapManager.OnCompleteLoadingMap += ResetTurn;
	}

	public void ResetTurn()
	{
		TurnCount = 0;

		IsPassingNextTurn = false;

		PlayerHandUpdate();
		UpdateValues();
	}

	public void StartTurn()
	{
		IsPassingNextTurn = false;

		UpdateValues();

		PlayerHandUpdate();
	}

	private void UpdateValues()
	{
		TurnCount = TurnCount + 1;

		StartCoroutine(ShowTurnText());

		CurrentMoveable = 3;
	}

	private void PlayerHandUpdate()
	{
		OnUpdatePlayerHand?.Invoke(PlayerDrawSize);
	}

	public void ActiveNextTurn() // �� ���� �Լ�
	{
		EndTurn?.Invoke();
		IsPassingNextTurn = true;

		Invoke(nameof(StartTurn), PassTime);
	}

	public void UseMoveable(int minus = 1) // �ൿ�� �Ҹ� �Լ�
	{
		CurrentMoveable = Mathf.Clamp(CurrentMoveable - minus, 0, MaxMoveable);
		if(CurrentMoveable <= 0) ActiveNextTurn();
	}

	private IEnumerator ShowTurnText()
	{
		TurnTxt.transform.localScale = Vector3.zero;
		TurnTxt.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack)
			.OnStart(() =>
			{
				TurnTxt.text = $"{TurnCount}��° ��";
			});
		yield return new WaitForSeconds(1f);
		TurnTxt.transform.DOScale(0, 0.5f).SetEase(Ease.OutBack)
			.OnStart(() =>
			{
				TurnTxt.text = string.Empty;
			});
	}
}
