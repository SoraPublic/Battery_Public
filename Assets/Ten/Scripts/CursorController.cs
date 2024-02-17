using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CursorController : MonoBehaviour
{
    [SerializeField, Header("�J�[����̈ړ��̈�")]
    private RectTransform targetArea;
    [SerializeField]
    private TimingManager timingManager;

    private Vector3 startPoint;
    private float endPoint;

    private double startTime;
    private bool isRun;

    private void Start()
    {
        startPoint = gameObject.GetComponent<RectTransform>().localPosition;
        Vector3[] v = new Vector3[4];
        targetArea.GetWorldCorners(v);
        endPoint = v[3].x;
        isRun = false;
    }

    /// <summary>
    /// �^�C�~���O�Q�[���̃��C���V�X�e��
    /// DoTween�Ŏ���
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public IEnumerator CursorMove(float second)
    {
        if (isRun)
        {
            yield break;
        }

        gameObject.GetComponent<RectTransform>().localPosition = startPoint;
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // ���̓X�y�[�X�L�[���������ƂŃJ�[�\���������n�߂�悤�ɂ��Ă���B�K�v�ɉ����ďC�����ĉ������B
        isRun = true;
        startTime = AudioSettings.dspTime;
        gameObject.GetComponent<RectTransform>().DOMoveX(endPoint, second).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            startTime -= 99;
            isRun = false;
        });

        yield return null; // 1�t���[���҂�
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || !isRun); // �X�y�[�X�L�[�������ꂽ��A��������OnComplete()���Ă΂ꂽ��A�J�[�\���̓������~�߂ē_�����������B
        gameObject.transform.DOKill();
        isRun = false;
        timingManager.CheckTiming(AudioSettings.dspTime - startTime);

        yield break;
    }

    private void OnDisable()
    {
        gameObject.transform.DOKill();
    }
}
