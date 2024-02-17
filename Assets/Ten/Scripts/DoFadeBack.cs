using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoFadeBack : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private float fadeTime;

    private Sequence sequence;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0, fadeTime))
            .Append(image.DOFade(0.8f, fadeTime).SetDelay(delayTime))
            .SetLoops(-1);
        sequence.Play();
    }

    private void OnDisable()
    {
        sequence.Kill();
    }
}
