using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDeck : MonoBehaviour
{
	[SerializeField] private int MaxHighSpell = 5;
	[SerializeField] private int MaxLowSpell = 5;

	public List<HighSpellBase> HighSpellDeck = new List<HighSpellBase>();
	public List<LowSpellBase> LowSpellDeck = new List<LowSpellBase>();

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
}
