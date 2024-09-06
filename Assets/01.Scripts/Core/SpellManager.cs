using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class SpellManager : MonoSingleton<SpellManager>, IManagerComponent
{
	private readonly string ScrollPoolName = "ScrollCard";

	[Header("ScrollCard Values")]
	[SerializeField][Range(0.1f, 2f)] private float CardSize = 1f;
	[SerializeField][Range(1f, 3f)] private float EnLargeCardSize = 2f;
	[SerializeField][Range(0f, 1f)] private float CardMoveDuration = 0.3f;

	[Header("Transforms")]
	[Tooltip("스크롤 생성 위치")]
	[SerializeField] private Transform CardSpawnPosition;
	[Space]
	[Tooltip("스크롤 정렬 - High")]
	[SerializeField] private Transform HighSpellTop;
	[Tooltip("스크롤 정렬 - High")]
	[SerializeField] private Transform HighSpellBottom;
	[Space]
	[Tooltip("스크롤 정렬 - Low")]
	[SerializeField] private Transform LowSpellTop;
	[Tooltip("스크롤 정렬 - Low")]
	[SerializeField] private Transform LowSpellBottom;

	[Header("Using ScrollCard Values")]
	[SerializeField] private string ExceptionLayerName = "ScrollZone";
	[SerializeField] private float SpellActiveDuration = 0.2f;
	[SerializeField] private float SpellInactiveDuration = 0.5f;

	[Header("Spell Datas")]
	[SerializeField] private int MaxHighSpell = 5;
	[SerializeField] private int MaxLowSpell = 5;
	public List<HighSpellBase> HighSpellDeck = new List<HighSpellBase>();
	public List<LowSpellBase> LowSpellDeck = new List<LowSpellBase>();
	public List<ScrollCard> LowSpellHand = new List<ScrollCard>();
	public List<ScrollCard> HighSpellHand = new List<ScrollCard>();

	[Header("Spell UI Elements")]
	[SerializeField] private TMPro.TMP_Text SpellSentenceTxt;
	public int ManaOverlapFigure = 0;

	public LowSpellBase UsedLowSpell { get; set; }
	public HighSpellBase UsedHighSpell { get; set; }

	private ScrollCard SelectedScroll = null;

	private bool isScrollDraging = false;
	private bool onPlayerScrollZone = false;

	private PlayerMain Player;

	private Managers _mngs;
	private GameManager _gameMng;
	private MapManager _mapMng;

	public void Initialize(Managers managers)
	{
		_mngs = managers;

		Player = FindObjectOfType<PlayerMain>().GetComponent<PlayerMain>();

		_mapMng = _mngs.GetManager<MapManager>();

		_gameMng = _mngs.GetManager<GameManager>();
		_gameMng.OnUpdatePlayerHand += DrawScrollPair;
		_gameMng.OnEndTurn += CompleteClear;

		ManaOverlapFigure = 0;
	}

	private void Update()
	{
		if(_gameMng.IsPassingNextTurn == true) return;
		
		if (isScrollDraging)
		{
			ScrollDrag();
		}

		DetectCardArea();
	}

	private void ScrollDrag()
	{
		if (onPlayerScrollZone == false)
		{
			SelectedScroll.MoveScrollTransform(new PRS(Utils.MousePosition, Quaternion.identity, SelectedScroll.originPRS.Scale), false);
		}
	}

	private void DetectCardArea()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePosition, Vector3.forward);
		int exceptionLayer = LayerMask.NameToLayer("ExceptionLayerName");
		onPlayerScrollZone = Array.Exists(hits, x => x.collider.gameObject.layer.Equals(exceptionLayer));
	}

	private string ExtractSingleSpellName(MonoSpellBase initSpell)
	{
		StringBuilder spellName = new StringBuilder();
		spellName.Append("\"");
		spellName.Append(initSpell.SpellName);
		spellName.Append("\"");
		return spellName.ToString();
	}

	private string MixSpellSentence()
	{
		StringBuilder sentence = new StringBuilder();
		sentence.Append("\"");
		sentence.Append(UsedHighSpell.SpellName);
		sentence.Append(" ");
		sentence.Append(UsedLowSpell.SpellName);
		sentence.Append("\"");
		return sentence.ToString();
	}

	#region Player Hand Methods

	public void CompleteClear()
	{
		ClearHand(LowSpellHand);
		ClearHand(HighSpellHand);
		
		if(UsedLowSpell == null) ResetTileSprite();
	}

	private void ClearHand(List<ScrollCard> hand) // 해당하는 리스트의 현재 보유 중인 카드 초기화
	{
		if (hand != null)
		{
			for (int count = 0; count < hand.Count; count++)
			{
				PoolManager.Instance.Push(hand[count]);
			}
			hand.Clear();
		}
		else
		{
			hand = new List<ScrollCard>();
		}
	}

	public void DrawScrollPair(int ScrollCount)
	{
		for(int count = 0; count < ScrollCount; count++)
		{
			DrawOneScroll(SpellTypeEnum.Low);
			DrawOneScroll(SpellTypeEnum.High);
		}
	}

	public void DrawOneScroll(SpellTypeEnum AddScrollType)
	{
		PoolManager.Instance.Pop(ScrollPoolName, CardSpawnPosition.position).TryGetComponent(out ScrollCard DrawScroll);

		switch (AddScrollType)
		{
			case SpellTypeEnum.Low:
				DrawScroll.InitSpellData(PickOneSpellInDeck(LowSpellDeck));
				LowSpellHand.Add(DrawScroll);
				break;
			case SpellTypeEnum.High:
				DrawScroll.InitSpellData(PickOneSpellInDeck(HighSpellDeck));
				HighSpellHand.Add(DrawScroll);
				break;
			default: break;
		}

		SetSortingOrder(AddScrollType);
		ScrollAlignment(AddScrollType);
	}

	public T PickOneSpellInDeck<T>(List<T> Deck) where T : class
	{
		return Deck[Random.Range(0, Deck.Count)];
	}

	public void RemoveCard(ScrollCard RemoveCard)
	{
		SpellTypeEnum InitSpellType = RemoveCard.ScrollSpellData.SpellType;

		switch (InitSpellType)
		{
			case SpellTypeEnum.Low: LowSpellHand.Remove(RemoveCard); break;
			case SpellTypeEnum.High: HighSpellHand.Remove(RemoveCard); break;
			default: break;
		}

		PoolManager.Instance.Push(RemoveCard);

		SetSortingOrder(InitSpellType);
		ScrollAlignment(InitSpellType);
	}

	#endregion

	#region Scroll Alignment Methods

	private void SetSortingOrder(SpellTypeEnum initSpellType)
	{
		int HandCount = initSpellType == SpellTypeEnum.Low ? LowSpellHand.Count : HighSpellHand.Count;

		for (int forCount = 0; forCount < HandCount; forCount++)
		{
			ScrollCard targetCard = initSpellType == SpellTypeEnum.Low ? LowSpellHand[forCount] : HighSpellHand[forCount];
			targetCard?.GetComponent<Order>().SetOriginOrder(forCount);
		}
	}

	private void ScrollAlignment(SpellTypeEnum InitSpellType)
	{
		List<PRS> originCardPRSs = new List<PRS>();

		if (InitSpellType == SpellTypeEnum.Low)
		{
			originCardPRSs = RoundAligement(LowSpellTop, LowSpellBottom, LowSpellHand.Count, 0.6f, Vector3.one * CardSize);
		}
		else if (InitSpellType == SpellTypeEnum.High)
		{
			originCardPRSs = RoundAligement(HighSpellTop, HighSpellBottom, HighSpellHand.Count, 0.6f, Vector3.one * CardSize);
		}

		var targetHands = InitSpellType == SpellTypeEnum.Low ? LowSpellHand : HighSpellHand;
		for (int count = 0; count < targetHands.Count; count++)
		{
			ScrollCard targetCard = targetHands[count];

			targetCard.originPRS = originCardPRSs[count];
			targetCard.MoveScrollTransform(targetCard.originPRS, true, CardMoveDuration);
		}
	}

	private List<PRS> RoundAligement(Transform TopTrm, Transform BottomTrm, int objCount, float height, Vector3 scale)
	{
		float[] objLerps = new float[objCount];
		List<PRS> results = new List<PRS>(objCount);

		switch (objCount)
		{
			case 1:
				objLerps = new float[] { 0.5f }; break;
			case 2:
				objLerps = new float[] { 0.27f, 0.73f }; break;
			case 3:
				objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
			default:
				float interval = 1f / (objCount - 1);
				for (int count = 0; count < objCount; count++) objLerps[count] = interval * count;
				break;
		}

		for (int count = 0; count < objCount; count++)
		{
			var targetPos = Vector3.Lerp(TopTrm.position, BottomTrm.position, objLerps[count]);
			var targetRot = Quaternion.identity;

			if (objCount >= 4)
			{
				float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[count] - 0.5f, 2));
				targetPos.y = targetPos.y + curve;
				targetRot = Quaternion.Slerp(TopTrm.rotation, BottomTrm.rotation, objLerps[count]);
			}
			results.Add(new PRS(targetPos, targetRot, scale));
		}

		return results;
	}

	#endregion

	#region Scroll Events

	public void ScrollMouseOver(ScrollCard initScroll)
	{
		SelectedScroll = initScroll;
		EnLargeScroll(true, initScroll);

		if (initScroll.ScrollSpellData.SpellType == SpellTypeEnum.Low && UsedLowSpell == null)
		{
			ChangeTileToAttack((LowSpellBase)initScroll.ScrollSpellData, TileState.PlayerAttack);
		}
	}

	public void ScrollMouseExit(ScrollCard initScroll)
	{
		EnLargeScroll(false, initScroll);
		
		if(UsedLowSpell == null) ResetTileSprite();
	}

	public void ScrollMouseUp()
	{
		isScrollDraging = false;

		Player.GetCompo<PlayerMovement>().CanMove = !isScrollDraging;
		Player.GetCompo<PlayerMovement>().OnCasting = isScrollDraging;

		if (onPlayerScrollZone == false)
		{
			CastSpellBase(SelectedScroll.ScrollSpellData);
		}
		else if(onPlayerScrollZone == true)
		{
			ResetTileSprite();
		}
	}

	public void ScrollMouseDown(MonoSpellBase initSpell)
	{
		isScrollDraging = true;

		Player.GetCompo<PlayerMovement>().CanMove = !isScrollDraging;
		Player.GetCompo<PlayerMovement>().OnCasting = isScrollDraging;
	}

	private void EnLargeScroll(bool isEnLarge, ScrollCard initScroll)
	{
		if (isEnLarge == true)
		{
			Vector3 enlargePos = new Vector3(initScroll.originPRS.Position.x, -3f, -10f);
			initScroll.MoveScrollTransform(new PRS(enlargePos, Quaternion.identity, Vector3.one * EnLargeCardSize), false);
		}
		else if (isEnLarge == false)
		{
			initScroll.MoveScrollTransform(initScroll.originPRS, false);
		}

		initScroll.GetComponent<Order>().SetMostFrontOrder(isEnLarge);
	}

	#endregion

	#region Use Scroll

	public void CastSpellBase(MonoSpellBase CastingSpell)
	{
		if(Player.GetCompo<PlayerMovement>().CanCasting == false) return;

		switch (CastingSpell.SpellType)
		{
			case SpellTypeEnum.High:
				if (UsedHighSpell == null)
				{
					UsedHighSpell = (HighSpellBase)CastingSpell;
					StartCoroutine(ShowSpellSentence(ExtractSingleSpellName(CastingSpell)));
					RemoveCard(SelectedScroll);
				}
				break;
			case SpellTypeEnum.Low:
				if (UsedLowSpell == null)
				{
					UsedLowSpell = (LowSpellBase)CastingSpell;
					ChangeTileToAttack(UsedLowSpell, TileState.PlayerAttack);
					StartCoroutine(ShowSpellSentence(ExtractSingleSpellName(CastingSpell)));
					RemoveCard(SelectedScroll);
				}
				break;
			default:
				break;
		}

		if (UsedHighSpell != null && UsedLowSpell != null)
		{
			ActivePlayerSpell();
			ManaOverlapFigure = ManaOverlapFigure + UsedHighSpell.CollectMP + UsedLowSpell.CollectMP;
			Debug.Log(ManaOverlapFigure);
		}
	}

	private void ActivePlayerSpell()
	{
		Player.GetCompo<PlayerMovement>().CanCasting = false;

		_gameMng.UseMoveable();
		ActiveAttack(UsedLowSpell.AttackZone, TileState.PlayerAttack);
	}

	public void ActiveAttack(List<AttackZoneElement> AttackZone, TileState AttackBy)
	{
		Vector2Int AttackedTile = Vector2Int.zero;
		for (int XCount = 0; XCount < _mapMng.MapSize; XCount++)
		{
			AttackedTile.x = XCount;
			for (int YCount = 0; YCount < _mapMng.MapSize; YCount++)
			{
				AttackedTile.y = YCount;
				if (AttackZone[XCount].Line[YCount] == true)
				{
					if(AttackBy == TileState.PlayerAttack)
					_mapMng.SettedTiles[AttackedTile].ActiveDamageable(AttackBy, UsedLowSpell.Damage, UsedHighSpell.HighSpellType);
					if(AttackBy == TileState.EnemyAttack)
					_mapMng.SettedTiles[AttackedTile].ActiveDamageable(AttackBy, 1, HighSpellTypeEnum.Normal);
				}
			}
		}

		ResetTileSprite();

		if (AttackBy == TileState.PlayerAttack) StartCoroutine(ShowSpellSentence(MixSpellSentence(), true));
	}

	public void ChangeTileToAttack(List<AttackZoneElement> AttackZone, TileState AttackBy)
	{
		Vector2Int AttackedTilePosition = Vector2Int.zero;
		for (int XCount = 0; XCount < _mapMng.MapSize; XCount++)
		{
			for (int YCount = 0; YCount < _mapMng.MapSize; YCount++)
			{
				if (AttackZone[XCount].Line[YCount] == true)
				{
					AttackedTilePosition.Set(XCount, YCount);
					_mapMng.SettedTiles[AttackedTilePosition].UpdateTileState(AttackBy);
				}
			}
		}
	}
	public void ChangeTileToAttack(LowSpellBase AttackData, TileState AttackBy)
	{
		Vector2Int AttackedTilePosition = Vector2Int.zero;
		for (int XCount = 0; XCount < _mapMng.MapSize; XCount++)
		{
			for (int YCount = 0; YCount < _mapMng.MapSize; YCount++)
			{
				if (AttackData.AttackZone[XCount].Line[YCount] == true)
				{
					AttackedTilePosition.Set(XCount, YCount);
					_mapMng.SettedTiles[AttackedTilePosition].UpdateTileState(AttackBy);
				}
			}
		}
	}


	private void ResetTileSprite()
	{
		foreach (TileBase TileData in _mapMng.SettedTiles.Values)
		{
			TileData.ResetTileRender();
		}
	}

	private IEnumerator ShowSpellSentence(string Sentence, bool isAttack = false)
	{
		SpellSentenceTxt.text = "";
		SpellSentenceTxt.transform.localScale = Vector3.zero;

		SpellSentenceTxt.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack)
			.OnStart(() =>
			{
				SpellSentenceTxt.text = Sentence;
			});

		yield return new WaitForSeconds(1f);

		if (isAttack == true)
		{
			SpellSentenceTxt.transform.DOScale(0, 0.5f).SetEase(Ease.OutQuint)
				.OnStart(() =>
				{
					SpellSentenceTxt.text = "";

					UsedLowSpell = null;
					UsedHighSpell = null;

					Player.GetCompo<PlayerMovement>().CanCasting = true;
				});
		}
	}

	#endregion

}
