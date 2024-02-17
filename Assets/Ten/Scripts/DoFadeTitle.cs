using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DoFadeTitle : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField]
    private float delayTime;
    [SerializeField]
    private float fadeTime;

    private Sequence sequence;

    private void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();
        sequence = DOTween.Sequence();
        sequence.Append(text.DOFade(0, fadeTime).SetDelay(delayTime))
            .Append(text.DOFade(1, fadeTime).SetDelay(delayTime))
            .SetLoops(-1);
        sequence.Play();
    }

    private void OnDisable()
    {
        sequence.Kill();
    }
}
