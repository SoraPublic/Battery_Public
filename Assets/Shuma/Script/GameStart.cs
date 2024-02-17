using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using DG.Tweening;

public class GameStart : MonoBehaviour
{
    [SerializeField] TimingManager timingManager;
    [SerializeField] TimeManager timeManager;
    [SerializeField] CinemachineVirtualCamera first;
    [SerializeField] CinemachineVirtualCamera last;

    [SerializeField] TextMeshProUGUI CountText;

    [SerializeField] Collider2D flore;

    [SerializeField] AudioSource cycle;
    [SerializeField] AudioSource CountDown;
    [SerializeField] AudioSource DramRoll;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        time = 0;
        yield return new WaitUntil(() => {
            time += Time.deltaTime;
            if (time > 4) return true;
            return Input.GetKey(KeyCode.Space);
        });

        first.Priority = 0;

        yield return new WaitForSeconds(2);

        CountDown.Play();
        CountText.transform.localScale = Vector3.zero;
        CountText.transform.DOScale(1, 0.9f);
        CountText.text = "3";

        yield return new WaitForSeconds(1);

        CountText.transform.localScale = Vector3.zero;
        CountText.transform.DOScale(1, 0.9f);
        CountText.text = "2";

        yield return new WaitForSeconds(1);

        CountText.transform.localScale = Vector3.zero;
        CountText.transform.DOScale(1, 0.9f);
        CountText.text = "1";

        yield return new WaitForSeconds(1);

        CountText.transform.localScale = Vector3.zero;
        CountText.transform.DOScale(1, 0.9f);
        CountText.text = "Start!";

        yield return new WaitForSeconds(1);

        CountText.transform.localScale = Vector3.zero;
        timingManager.CursorStart();
        timeManager.StartTimer();

        // yield return new WaitForSeconds(34);
        yield return new WaitForSeconds(39);
        
        last.Priority = 30;
        //cycle.DOFade(0, 1);

        yield return new WaitForSeconds(1);

        flore.enabled = false;
        DramRoll.Play();
    }
}
