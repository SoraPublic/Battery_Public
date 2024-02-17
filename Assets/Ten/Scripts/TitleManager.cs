using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private TutorialSlides slides;
    [SerializeField]
    private AudioSource submit;
    [SerializeField]
    private AudioSource cancel;
    [SerializeField]
    private ArrowMover rightArrow;
    [SerializeField]
    private ArrowMover leftArrow;
    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private GameObject batteriesParent;
    [SerializeField]
    private GameObject batteries;
    [SerializeField]
    private float batteriesResetTime;

    [SerializeField] Button start, right, left;

    private float time;
    private int slidesCount;

    private void Start()
    {
        slidesCount = 0;
        time = 0;

        gameObject.ObserveEveryValueChanged(_ => slidesCount)
            .Subscribe(_ =>
            {
                slides.SetPanel(slidesCount);
            })
            .AddTo(this);

        start.onClick.AddListener(() =>
        {
            slidesCount++;
            rightArrow.Submit();
            submit.Play();
        });

        right.onClick.AddListener(() =>
        {
            slidesCount++;
            rightArrow.Submit();
            submit.Play();
        });

        left.onClick.AddListener(() =>
        {
            slidesCount--;
            leftArrow.Submit();
            cancel.Play();
        });
    }

    private void Update()
    {
        /*time += Time.deltaTime;
        if (time > batteriesResetTime)
        {
            time = 0;
            Instantiate(batteries, batteriesParent.transform);
        }*/

        if (slides.flag)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            slidesCount++;
            rightArrow.Submit();
            submit.Play();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            slidesCount = 0;
            leftArrow.Submit();
            cancel.Play(); 
            return;
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && slidesCount > 0)
        {
            slidesCount++;
            rightArrow.Submit();
            submit.Play();
            return;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            slidesCount--;
            leftArrow.Submit();
            cancel.Play(); 
            return;
        }
    }

    public void OnAudioValueChanged()
    {
        if (!audioManager.entryFlag)
        {
            return;
        }

        submit.Play();
    }
}
