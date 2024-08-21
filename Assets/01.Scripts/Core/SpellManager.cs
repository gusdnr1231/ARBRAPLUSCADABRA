using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpellManager : MonoSingleton<SpellManager>
{
	[Header("Scroll Datas")]
	[SerializeField] private Scroll CardPrefab;
	[SerializeField][Range(0.1f, 2f)] private float CardSize = 1f;
	[SerializeField][Range(1f, 3f)] private float EnLargeCardSize = 2f;
	[SerializeField][Range(0f, 1f)] private float CardMoveDuration = 0.3f;

	[Header("Transforms")]
	[SerializeField] private Transform CardSpawnPosition;
	[SerializeField] private Transform HighSpellTop;
	[SerializeField] private Transform HighSpellBottom;
	[SerializeField] private Transform LowSpellTop;
	[SerializeField] private Transform LowSpellBottom;

	[Header("Using Scroll Values")]
	[SerializeField] private LayerMask ExceptionLayer;
	[SerializeField] private float SpellActiveDuration = 0.2f;
	[SerializeField] private float SpellInactiveDuration = 0.5f;

	[Header("Spell Datas")]
	public List<MonoSpellBase> PlayerDeck = new List<MonoSpellBase>();
	public List<Scroll> LowSpellHand = new List<Scroll>();
	public List<Scroll> HighSpellHand = new List<Scroll>();

	public LowSpellBase UsedLowSpell { get; set; }
	public HighSpellBase UsedHighSpell { get; set; }

	private Scroll SelectedScroll = null;

	private bool isScrollDraging = false;
	private bool onPlayerScrollZone = false;

	private void Start()
	{
		ClearPlayerHand();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			AddCard();
		}

		if(isScrollDraging)
		{
			ScrollDrag();
		}

		DetectCardArea();
	}

	private void ScrollDrag()
	{
		if(onPlayerScrollZone == false)
		{
			SelectedScroll.MoveScrollTransform(new PRS(Utils.MousePosition, Quaternion.identity, SelectedScroll.originPRS.Scale), false);
		}
	}

	private void DetectCardArea()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePosition, Vector3.forward);
		int exceptionLayer = ExceptionLayer;
		onPlayerScrollZone = Array.Exists(hits, x => x.collider.gameObject.layer == exceptionLayer);
	}

	public string MixSpellSentence()
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

	public void ClearPlayerHand()
	{
		if (LowSpellHand != null)
		{
			for (int count = 0; count < LowSpellHand.Count; count++) Destroy(LowSpellHand[count].gameObject);
			LowSpellHand.Clear();
		}
		else if(LowSpellHand == null) LowSpellHand = new List<Scroll>();	

		if (HighSpellHand != null)
		{
			for (int count = 0; count < HighSpellHand.Count; count++) Destroy(HighSpellHand[count].gameObject);
			HighSpellHand.Clear();
		}
		else if(HighSpellHand == null) HighSpellHand = new List<Scroll>();	
	}

	public void AddCard()
	{
		Scroll addCardObject = Instantiate(CardPrefab, CardSpawnPosition.position, Quaternion.identity);

		addCardObject.InitSpellData(PlayerDeck[Random.Range(0, PlayerDeck.Count)]);

		SpellTypeEnum InitSpellType = addCardObject.ScrollSpellData.SpellType;
		
		switch (InitSpellType)
		{
			case SpellTypeEnum.Low: LowSpellHand.Add(addCardObject); break;
			case SpellTypeEnum.High: HighSpellHand.Add(addCardObject); break;
				default: break;	
		}

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
			Scroll targetCard = initSpellType == SpellTypeEnum.Low ? LowSpellHand[forCount] : HighSpellHand[forCount];
			targetCard?.GetComponent<Order>().SetOriginOrder(forCount);
		}
	}

	private void ScrollAlignment(SpellTypeEnum InitSpellType)
	{
		List<PRS> originCardPRSs = new List<PRS>();

		if(InitSpellType == SpellTypeEnum.Low)
		{
			originCardPRSs = RoundAligement(LowSpellTop, LowSpellBottom, LowSpellHand.Count, 0.6f, Vector3.one * CardSize);
		}
		else if(InitSpellType== SpellTypeEnum.High)
		{
			originCardPRSs = RoundAligement(HighSpellTop, HighSpellBottom, HighSpellHand.Count, 0.6f, Vector3.one * CardSize);
		}

		var targetHands = InitSpellType == SpellTypeEnum.Low ? LowSpellHand : HighSpellHand;
		for(int count = 0; count < targetHands.Count; count++)
		{
			Scroll targetCard = targetHands[count];

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
				objLerps =  new float[] { 0.5f };				break;
			case 2:
				objLerps =  new float[] { 0.27f, 0.73f };		break;
			case 3:
				objLerps =  new float[] { 0.1f, 0.5f, 0.9f };	break;
			default:
				float interval = 1f / (objCount - 1);
				for(int count = 0; count < objCount; count++) objLerps[count] = interval * count;
				break;
		}

		for (int count = 0; count < objCount; count++)
		{
			var targetPos = Vector3.Lerp(TopTrm.position, BottomTrm.position, objLerps[count]);
			var targetRot =  Quaternion.identity;
			
			if(objCount >= 4)
			{
				float curve =  Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[count] - 0.5f, 2));
				targetPos.y = targetPos.y + curve;
				targetRot = Quaternion.Slerp(TopTrm.rotation, BottomTrm.rotation, objLerps[count]);
			}
			results.Add(new PRS(targetPos, targetRot, scale));
		}

		return results;
	}

	#endregion

	#region Scroll Events

	public void ScrollMouseOver(Scroll initScroll)
	{
		SelectedScroll = initScroll;
		EnLargeScroll(true, initScroll);
	}

	public void ScrollMouseExit(Scroll initScroll)
	{
		EnLargeScroll(false, initScroll);
	}

	public void ScrollMouseUp()
	{
		isScrollDraging = false;
	}

	public void ScrollMouseDown()
	{
		isScrollDraging = true;
	}

	private void EnLargeScroll(bool isEnLarge, Scroll initScroll)
	{
		if (isEnLarge == true)
		{
			Vector3 enlargePos = new Vector3(initScroll.originPRS.Position.x, -3f, -10f);
			initScroll.MoveScrollTransform(new PRS(enlargePos, Quaternion.identity, Vector3.one * EnLargeCardSize), false);
		}
		else if(isEnLarge == false)
		{
			initScroll.MoveScrollTransform(initScroll.originPRS, false);
		}

		initScroll.GetComponent<Order>().SetMostFrontOrder(isEnLarge);
	}

	#endregion

	#region Use Scroll

	[Header("Testing Reference")]
	[SerializeField] private TMPro.TMP_Text TestingTMP;
	[SerializeField] private Color[] TestingGroundColor;

	public void CastSpellBase(MonoSpellBase CastingSpell)
	{
		switch (CastingSpell.SpellType)
		{
			case SpellTypeEnum.High:
				if(UsedHighSpell == null) UsedHighSpell = (HighSpellBase)CastingSpell;
				break;
			case SpellTypeEnum.Low:
				if(UsedLowSpell == null) UsedLowSpell = (LowSpellBase)CastingSpell;
				break;
			default:
				break;
		}

		if(UsedHighSpell != null && UsedLowSpell != null)
		{
			ActiveSpell();
		}
	}

	private void ActiveSpell()
	{
		Debug.Log(UsedHighSpell.HighSpellType.ToString() + " : " + (int)UsedHighSpell.HighSpellType);
		ActiveAttack();
		StartCoroutine(ShowSpellSentence());
	}

	private void ActiveAttack()
	{
		Vector2Int AttackedTilePosition = Vector2Int.zero;
		for (int XCount = 0; XCount < UsedLowSpell.AttackZone.Count; XCount++)
		{
			for(int YCount = 0; YCount < UsedLowSpell.AttackZone[XCount].Line.Count; YCount++)
			{
				if (UsedLowSpell.AttackZone[XCount].Line[YCount] == true)
				{
					AttackedTilePosition.Set(XCount, YCount);
					MapManager.Instance.SettedTiles[AttackedTilePosition]
						.ChangeColor(TestingGroundColor[(int)UsedHighSpell.HighSpellType], 2f);
				}
			}
		}
	}

	private IEnumerator ShowSpellSentence()
	{
		TestingTMP.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack)
			.OnStart(() =>
			{
				TestingTMP.text = MixSpellSentence();

				UsedLowSpell = null;
				UsedHighSpell = null;
			});
			yield return new WaitForSeconds(1f);
		TestingTMP.transform.DOScale(0, 0.5f).SetEase(Ease.OutQuint)
			.OnStart(() =>
			{
				TestingTMP.text = "";
			});
	}

	#endregion

}
