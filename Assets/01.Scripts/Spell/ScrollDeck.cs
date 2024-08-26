using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollDeck : MonoBehaviour
{
	[Header("Scroll Deck Infomation")]
	[SerializeField] private int MaxHighSpell = 5;
	[SerializeField] private int MaxLowSpell = 5;
	public List<HighSpellBase> HighSpellDeck = new List<HighSpellBase>();
	public List<LowSpellBase> LowSpellDeck = new List<LowSpellBase>();

	[Header("ScrollCard Datas")]
	[SerializeField] private ScrollCard ScrollPrefab;

	[Header("Transforms")]
	[SerializeField] private Transform CardSpawnPosition;

	public void InitSpellToDeck(HighSpellBase highSpell)
	{
		if(HighSpellDeck.Count > MaxHighSpell)
		{
			HighSpellDeck.Add(highSpell);
		}
	}

	public void InitSpellToDeck(LowSpellBase lowSpell)
	{
		if (LowSpellDeck.Count > MaxLowSpell)
		{
			LowSpellDeck.Add(lowSpell);
		}
	}

	public void DrawScroll(SpellTypeEnum AddScrollType)
	{
		ScrollCard newCardObject = Instantiate(ScrollPrefab, CardSpawnPosition.position, Quaternion.identity);

		SpellTypeEnum InitSpellType = newCardObject.ScrollSpellData.SpellType;

		switch (AddScrollType)
		{
			case SpellTypeEnum.Low:
				newCardObject.InitSpellData(PickOneSpellInDeck(LowSpellDeck));
				break;
			case SpellTypeEnum.High:
				newCardObject.InitSpellData(PickOneSpellInDeck(HighSpellDeck));
				break;
			default: break;
		}

		//SetSortingOrder(InitSpellType);
		//ScrollAlignment(InitSpellType);
	}

	public T PickOneSpellInDeck<T> (List<T> Deck) where T : class
	{
		return Deck[Random.Range(0, Deck.Count)];
	}
}
