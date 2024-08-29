using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ScrollCard : PoolableMono
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

	private SpellManager spellMngs;

	public override void ResetPoolableMono()
	{
	}

	public override void EnablePoolableMono()
	{
		if (spellMngs == null) spellMngs = Managers.Instance.GetManager<SpellManager>();
	}


	public void InitSpellData(MonoSpellBase InitData)
    {
		ScrollSpellData = InitData;

        SetSpellDefaultData();

		if (ScrollSpellData.SpellType == SpellTypeEnum.Low)
		{
			FigureBuilder.Clear();
			FigureBuilder.Append("-");
			FigureBuilder.Append(ScrollSpellData.Damage);
			SpellDamage.text = FigureBuilder.ToString();
		}
		else if (ScrollSpellData.SpellType == SpellTypeEnum.High)
		{
			SpellDamage.text = string.Empty;
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
		spellMngs.ScrollMouseOver(this);		
	}

	private void OnMouseExit()
	{
		spellMngs.ScrollMouseExit(this);
	}

	private void OnMouseUp()
	{
		spellMngs.ScrollMouseUp();
	}

	private void OnMouseDown()
	{
		spellMngs.ScrollMouseDown(this.ScrollSpellData);
	}

	#endregion
}
