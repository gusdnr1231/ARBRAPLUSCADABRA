using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ScrollCard : MonoBehaviour
{
    
    [Header("ScrollCard Infomation Objects")]
    [SerializeField] private TMP_Text SpellName;
    [SerializeField] private SpriteRenderer SpellIcon;
    [SerializeField] private TMP_Text SpellMP;
    [SerializeField] private TMP_Text SpellDamage;

    [Header("ScrollCard Transform Datas")]
    public PRS originPRS;

    public MonoSpellBase ScrollSpellData;

    private StringBuilder FigureBuilder = new StringBuilder();

	public void InitSpellData(MonoSpellBase InitData)
    {
		ScrollSpellData = InitData;

        SetSpellDefaultData();
		
		switch (ScrollSpellData.SpellType)
        {
            case SpellTypeEnum.Low:
				FigureBuilder.Clear();
                FigureBuilder.Append("-");
				FigureBuilder.Append(ScrollSpellData.Damage);
                SpellDamage.text = FigureBuilder.ToString();
				break;
            case SpellTypeEnum .High:
				SpellDamage.text = "";
				break;

            default:
                break;
        }

	}

    private void SetSpellDefaultData()
    {
        SpellName.text = ScrollSpellData.SpellName;
        SpellIcon.sprite = ScrollSpellData.SpellIcon;
		FigureBuilder.Clear();
		FigureBuilder.Append("+");
		FigureBuilder.Append(ScrollSpellData.CollectMP);
		SpellMP.text = FigureBuilder.ToString();

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
	}

	private void OnMouseDown()
	{
		SpellManager.Instance.ScrollMouseDown(this.ScrollSpellData);
	}

	#endregion
}
