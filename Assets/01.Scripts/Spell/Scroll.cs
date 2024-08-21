using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    
    [Header("Scroll Infomation Objects")]
    [SerializeField] private TMP_Text SpellName;

    [Header("Scroll Transform Datas")]
    public PRS originPRS;

    public MonoSpellBase ScrollSpellData;

    public void InitSpellData(MonoSpellBase InitData)
    {
		ScrollSpellData = InitData;

		switch (ScrollSpellData.SpellType)
        {
            case SpellTypeEnum.Low:
                break;
            case SpellTypeEnum .High:
                break;

            default:
                break;
        }

		SpellName.text = InitData.SpellName;
	}

    public void MoveScrollTransform(PRS prs, bool useDotween, float durtation = 0f)
    {
        if (useDotween)
        {
            transform.DOMove(prs.Position, durtation);
            transform.DORotateQuaternion(prs.Rotation, durtation);
            transform.DOScale(prs.Scale, durtation);
        }
        else
        {
            transform.position = prs.Position;
            transform.rotation = prs.Rotation;
            transform.localScale = prs.Scale;
        }
    }

    public void CastingSpell()
    {
        SpellManager.Instance.CastSpellBase(ScrollSpellData);
    }

	#region Scroll Events

	private void OnMouseOver()
	{
        SpellManager.Instance.ScrollMouseOver(this);		
	}

	private void OnMouseExit()
	{
        SpellManager.Instance.ScrollMouseExit(this);
	}

	private void OnMouseUp()
	{
        SpellManager.Instance.ScrollMouseUp();
		CastingSpell();
	}

	private void OnMouseDown()
	{
		SpellManager.Instance.ScrollMouseDown();
	}

	#endregion
}
