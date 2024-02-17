using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TutorialSlides : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup parent;
    [SerializeField]
    private GameObject[] slides;
    [SerializeField]
    private AudioSource BGMSource;
    [SerializeField]
    private TextMeshProUGUI lastText;

    private SceneLoder sceneLoder;

    public bool flag
    {
        get;
        private set;
    }

    private void Start()
    {
        sceneLoder = new SceneLoder();
        parent.gameObject.SetActive(false);
        flag = false;
    }

    public void SetPanel(int slideCount)
    {
        slideCount--;

        Sequence sequence = DOTween.Sequence();

        sequence.OnStart(() =>
        {
            flag = true;
        }).OnComplete(() =>
        {
            flag = false;
        });

        if (slideCount == 0)
        {
            parent.gameObject.SetActive(true);
            sequence.Append(parent.DOFade(1, 0.5f));
        }
        else if (slideCount == -1)
        {
            sequence.Append(parent.DOFade(0, 0.2f).OnComplete(() => parent.gameObject.SetActive(false)));
        }
        else if (slideCount == 3)
        {
            sequence.OnComplete(() =>
            {
                // ‚È‚ñ‚©‰‰o“ü‚ê‚½‚¢
                Sequence lastSequence = DOTween.Sequence();
                lastSequence.OnComplete(() =>
                {
                    sceneLoder.LoadScene(Scene.Game);
                });

                lastSequence.Append(BGMSource.DOFade(0, 1.0f).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        BGMSource.Stop();
                    }))
                .Join(lastText.gameObject.transform.DOScale(new Vector3(2.0f, 2.0f, 2.0f), 0.5f)
                .SetEase(Ease.InBack)
                .SetDelay(0.5f)
                ).Join(lastText.DOFade(0, 0.5f).SetEase(Ease.InBack));

                lastSequence.Play();
            });
        }

        sequence.Join(gameObject.transform.DOLocalMoveX(-800 * slideCount, 0.5f));

        sequence.Play();
    }
}
