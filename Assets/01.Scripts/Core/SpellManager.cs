using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SpellManager : MonoSingleton<SpellManager>
{
	[Header("Card Datas")]
	[SerializeField] private Scroll CardPrefab;
	[SerializeField][Range(0.1f, 2f)] private float CardSize = 1f;

	[Header("Transforms")]
	[SerializeField] private Transform CardSpawnPosition;
	[SerializeField] private Transform HighSpellTop;
	[SerializeField] private Transform HighSpellBottom;
	[SerializeField] private Transform LowSpellTop;
	[SerializeField] private Transform LowSpellBottom;
	
	[Header("Spell Datas")]
	public List<MonoSpellBase> PlayerDeck = new List<MonoSpellBase>();
	public List<Scroll> LowSpellHand = new List<Scroll>();
	public List<Scroll> HighSpellHand = new List<Scroll>();

	public LowSpellBase UsedLowSpell { get; set; }
	public HighSpellBase UsedHighSpell { get; set; }

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
	}

	public string SpellSentence()
	{
		StringBuilder sentence = new StringBuilder();
		sentence.Append("\"");
		sentence.Append(UsedHighSpell.SpellName);
		sentence.Append(" ");
		sentence.Append(UsedLowSpell.SpellName);
		sentence.Append("\"");
		return sentence.ToString();
	}



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

		CardAlignment(InitSpellType);
	}

	#region Card Alignment Methods

	private void CardAlignment(SpellTypeEnum InitSpellType)
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
			targetCard.MoveCartTransform(targetCard.originPRS, true, 0.7f);
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

}
