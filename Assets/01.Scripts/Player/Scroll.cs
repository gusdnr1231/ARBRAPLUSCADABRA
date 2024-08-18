using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public MonoSpellBase ScrollSpellData { get; set; }

    public PRS originPRS;

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
	}

    public void MoveCartTransform(PRS prs, bool useDotween, float durtation = 0f)
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
}
