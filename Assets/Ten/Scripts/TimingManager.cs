using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine.Serialization;

public class TimingManager : MonoBehaviour
{
    [Header("�n�_����I�_�܂ł̎��ԁA�X�N���v�g����̕ύX���")]
    public float second;
    [Header("Max����,Min����")]
    private float MAX = 4f;
    private float MIN = 1.0f;
    float ChangeTime = 0.3f;

    [SerializeField, Header("���_�͈�(�� �� �S�� �~ �����A�� �� �΂͈̔� �~ ����)")]
    private AllRange range;

    [SerializeField]
    private TargetAreaManager targetAreaManager;
    [SerializeField]
    private CursorController cursorController;

    [SerializeField] private GameObject cursorPoint;
    [SerializeField] private TextMeshProUGUI justText, goodText;
    [SerializeField] private GameObject slider;

    // �O�̂��ߗp�ӂ����̂ŁA�V�[���J�ڂ��鎞�Ƃ���Dispose���Ă����Ă��������B
    private IDisposable rangeDispose;
    private IDisposable secondDispose;

    [SerializeField, Header("�����l�m�F�p�A�������݋֎~")]
    private Timing timing;

    [SerializeField, Header("�f�o�b�O�p�A�K�v�Ȃ��Ȃ�Ώ���")]
    private Button btn;

    private Coroutine cursorCoroutine;

    private bool isRunning = false;

    public MMFeedbacks redFeedback;

    [FormerlySerializedAs("SE")] [SerializeField] private AudioSource justSE;
    [SerializeField] private AudioSource goodSE;

    private bool isSlideAnimation = false;

    /// <summary>
    /// ���_�͈͂�ς������Ƃ��Ɏg���֐�
    /// �΃G���A�̎n�_�ƏI�_�A�ԃG���A�̎n�_�ƏI�_�̌v4���w�肷��
    /// </summary>
    /// <param name="newRange"></param>
    public void SetNewRange(AllRange newRange)
    {
        range.green = newRange.green;
        range.red = newRange.red;
    }
    
    /// <summary>
    /// ���_���Ԃ��v�Z
    /// </summary>
    /// <param name="nowRange"></param>
    private void SetNewTiming(AllRange nowRange)
    {
        timing.greenStart = second * nowRange.green.left;
        timing.greenEnd = second * nowRange.green.right;
        timing.redStart = timing.greenStart + (timing.greenEnd - timing.greenStart) * nowRange.red.left;
        timing.redEnd = timing.greenStart + (timing.greenEnd - timing.greenStart) * nowRange.red.right;
    }

    /// <summary>
    /// �^�C�~���O�Q�[�����n��������
    /// </summary>
    public void CursorStart()
    {
        cursorCoroutine = StartCoroutine(cursorController.CursorMove(second));
        isRunning = true;
    }

    public void CursorStop()
    {
        try
        {
            StopCoroutine(cursorCoroutine);
        }
        catch
        {

        }
    }

    private void Start()
    {
        rangeDispose = gameObject.ObserveEveryValueChanged(_ => range) // ���_�͈͂��ύX���ꂽ�Ƃ�UI�̒����Ɠ��_���Ԃ̌v�Z������
            .Subscribe(_ =>
            {
                targetAreaManager.SetRange(range);
                SetNewTiming(range);
            }).AddTo(this);

        secondDispose = gameObject.ObserveEveryValueChanged(_ => second) // �n�_����I�_�܂ł̎��Ԃ��ύX���ꂽ�Ƃ����_���Ԃ��v�Z����
            .Subscribe(_ =>
            {
                SetNewTiming(range);
            }).AddTo(this);

        SetNewRange(range); // ������

        second = 2.5f;

        TimeManager.instance.time
            .Subscribe(x =>
            {
                if (x == 18)
                {
                    MIN = 0.8f;
                    //Debug.Log("MIN更新");
                    range.green.left = 0.55f;
                    range.green.right = 0.95f;
                    SetNewRange(range);
                }

                if (x == 12)
                {
                    MIN = 0.6f;
                    //Debug.Log("MIN更新");
                    // range.red.left = 0.2f;
                    // range.green.left = 0.5f;
                    // range.red.right = 0.8f;
                    // SetNewRange(range);
                }

                if (x == 7)
                {
                    MIN = 0.5f;
                    //Debug.Log("MIN更新");
                    range.green.left = 0.3f;
                    range.red.left = 0.5f;
                    range.red.right = 0.75f;
                    SetNewRange(range);
                }

                if (x == 2)
                {
                    MIN = 0.7f;
                    second = 0.7f;
                    //Debug.Log("MIN更新");
                    range.green.left = 0f;
                    range.green.right = 1f;
                    range.red.left = 0f;
                    range.red.right = 1f;
                    SetNewRange(range);
                }

                if (x == 0)
                {
                    Debug.Log("終了");
                    CursorStop();
                    isRunning = false;
                }
            });

        justText.DOFade(0f, 0f);
        goodText.DOFade(0, 0);
        
        //仮
        // IsRunningOn(3);
    }

    /// <summary>
    /// ���_�v�Z�֐�
    /// </summary>
    /// <param name="time"></param>
    public void CheckTiming(double time) // ����Debug.Log()�𗬂������Ȃ̂ŁA���_�̃V�X�e�����ł����炻��ɒu�������Ă��������B
    {
        ChangeTime = second > 1.0f ? 0.3f : 0.1f;
        if (time < timing.greenStart)
        {
            //Debug.Log("遅い");
            second = Mathf.Clamp(second + ChangeTime, MIN, MAX);
            ScoreManager.instance.combo.Value = 0;
        }
        else if ((time >= timing.greenStart) && (time < timing.redStart))
        {
            //Debug.Log("ちょっと遅い");
            ScoreManager.instance.AddScore(60);
            DispText(goodText);
            goodSE.Play();
            ScoreManager.instance.ScoreTextAnimation();
            ScoreManager.instance.combo.Value = 0;
        }
        else if ((time >= timing.redStart) && (time <= timing.redEnd))
        {
            //Debug.Log("ぴったり");
            second = Mathf.Clamp(second - ChangeTime, MIN, MAX);
            ScoreManager.instance.AddScore(125);
            PlayRedFeedback();
            justSE.Play();
            if (!isSlideAnimation)
            {
                isSlideAnimation = true;
                slider.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.2f)
                    .OnComplete(() =>
                    {
                        isSlideAnimation = false;
                    });
            }
            DispText(justText);
            ScoreManager.instance.ScoreTextAnimation();
            ScoreManager.instance.combo.Value++;
        }
        else if ((time > timing.redEnd) && (time <= timing.greenEnd))
        {
            //Debug.Log("ちょっと早い");
            ScoreManager.instance.AddScore(60);
            DispText(goodText);
            goodSE.Play();
            ScoreManager.instance.ScoreTextAnimation();
            ScoreManager.instance.combo.Value = 0;
        }
        else if ((time > timing.greenEnd) && (time <= second))
        {
            //Debug.Log("早い");
            second = Mathf.Clamp(second + ChangeTime, MIN, MAX);
            ScoreManager.instance.combo.Value = 0;
        }
        else if (time > second)
        {
            //Debug.Log("行き過ぎた");
            second = Mathf.Clamp(second + ChangeTime, MIN, MAX);
            ScoreManager.instance.combo.Value = 0;
        }
        // Debug.Log("���� : " + time + "�b");
        CursorStart();
        //btn.interactable = true; //�f�o�b�O�p�A�K�v�Ȃ��Ȃ�Ώ����B
    }

    private void Update()
    {
        if(!isRunning) return;
        
        ScoreManager.instance.AddScore(100/Mathf.Pow(second, 2) * Time.deltaTime);
    }

    private async void IsRunningOn(int time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        Debug.Log("isRunningON");
        isRunning = true;
    }

    private void PlayRedFeedback()
    {
        redFeedback?.PlayFeedbacks();
    }

    private void DispText(TextMeshProUGUI text)
    {
        text.transform.DOMoveX(cursorPoint.transform.position.x, 0);
        text.DOFade(0, 0);
        text.transform.DOScale(new Vector3(1, 1, 1), 0);
        text.DOFade(1, 0.3f);
        text.transform.DOScale(new Vector3(2, 2, 2), 0.3f)
            .OnComplete(() =>
            {
                text.DOFade(0, 0.2f);
            });
    }
}

[System.Serializable]
public struct AllRange
{
    public Range green;
    public Range red;
}

[System.Serializable]
public struct Range
{
    [Range(0,1)]
    public float left;
    [Range(0,1)]
    public float right;
}

[System.Serializable]
public struct Timing
{
    public float greenStart;
    public float redStart;
    public float redEnd;
    public float greenEnd;
}
