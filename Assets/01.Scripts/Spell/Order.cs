using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField]
    private Renderer[] middleRenderers;
    [SerializeField]
    private Renderer[] backRenderers;
    [SerializeField]
    private string sortingLayerName;

    private int originOrder;

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    public void SetOrder(int order)
    {
        int mulOrder = order * 2;

        foreach (var renderer in middleRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }

		foreach (var renderer in backRenderers)
		{
			renderer.sortingLayerName = sortingLayerName;
			renderer.sortingOrder = mulOrder;
		}
	}

}
