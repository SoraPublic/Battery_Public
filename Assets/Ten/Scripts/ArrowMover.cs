using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowMover : MonoBehaviour
{
    [SerializeField]
    private Arraws arraw;
    [SerializeField]
    private float magnification;

    private int arrowValue;

    [SerializeField] private Vector3 arrowPosition, Scale;

    private void OnEnable()
    {
        transform.localPosition = arrowPosition;
        transform.localScale = Scale;

        if (arraw == Arraws.right)
        {
            arrowValue = 1;
        }
        else if(arraw == Arraws.left)
        {
            arrowValue = -1;
        }

        gameObject.transform.DOLocalMoveX(15 * arrowValue, 1.0f)
            .SetRelative(true)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Submit()
    {
        gameObject.transform.DOComplete();
        gameObject.transform.DOScale(new Vector3(magnification * arrowValue, magnification, magnification), 0.3f)
            .SetRelative(true)
            .SetLoops(2, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        gameObject.transform.DOKill();
    }
}

public enum Arraws
{
    right,
    left,
}
