using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UniRx;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /* ----- UI系 ----- */
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button StartButton;

    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource EndBGM;

    [SerializeField] private TextMeshProUGUI rendaText;
    
    /* ----- result関連 ----- */
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI finishText, finishScoreText;
    [SerializeField] private Button gotoTitleButton, restartButton;

    [SerializeField] private GameObject comboPanel;
    [SerializeField] private TextMeshProUGUI maxComboText;

    [SerializeField] AudioSource whisle;
    
    /* ----- 変数 ----- */
    public ReactiveProperty<int> time;

    private bool inGame;

    // Start is called before the first frame update
    void Start()
    {
        time.Value = 30;
        comboPanel.SetActive(false);
        rendaText.gameObject.SetActive(false);
        inGame = true;
        
        //残り時間の変化の検知
        time.Subscribe(x =>
        {
            timeText.text = time.Value.ToString();
            //残り時間によって色を変える
            if (x > 10)
            {
                timeText.color = Color.black;
            }
            else
            {
                timeText.color = Color.red;
                timeText.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.2f).OnComplete(() =>
                {
                    timeText.transform.DOScale(new Vector3(1f, 1f, 1), 0.2f);
                });
            }
            
            //連打テキストの表示
            if (x == 2)
            {
                rendaText.gameObject.SetActive(true);
                rendaText.transform.DOScale(new Vector3(0, 0, 1), 0);
                rendaText.transform.DOScale(new Vector3(1, 1, 1), 0.5f);
                rendaText.transform.DOShakePosition(2.5f, 5f, 30, 2, false, false).OnComplete(() =>
                {
                    rendaText.gameObject.SetActive(false);
                });
            }
        }).AddTo(this);

        /*StartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                BGM.Play();
                Timer(30);
            }).AddTo(this);*/

        restartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                //もう一度同じシーンを開く
                SceneManager.LoadScene("TestScene");
            }).AddTo(this);

        gotoTitleButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                //タイトルへ戻る
                SceneManager.LoadScene("TitleScene");
            }).AddTo(this);
        
        //結果パネルを非表示
        resultPanel.SetActive(false);
        finishText.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        BGM.Play();
        Timer(30);
    }

    public void Timer(int time)
    {
        Debug.Log("タイマー開始");
        this.time.Value = time;
        Observable.Interval(System.TimeSpan.FromSeconds(1))
            .TakeWhile(_ => this.time.Value > 0)
            .Subscribe(_ =>
            {
                this.time.Value--;
            }, () =>
            {
                EndGame();
            });
    }

    /// <summary>
    /// ゲームの終了時
    /// </summary>
    public async void EndGame()
    {
        
        finishText.gameObject.SetActive(true);
        finishText.transform.DOScale(new Vector3(0, 0, 1), 0);
        finishText.transform.DORotate(new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360);
        finishText.transform.DOScale(new Vector3(1.8f, 1.8f, 1), 1);
        whisle.Play();

        await UniTask.Delay(TimeSpan.FromSeconds(3));
        ScoreManager.instance.AddScore(ScoreManager.instance.maxCombo * 100);
        ScoreManager.instance.ScoreTextAnimation();
        comboPanel.SetActive(true);
        maxComboText.text = "+ " + ScoreManager.instance.maxCombo.ToString() +" 個";
        
        
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        
        
        resultPanel.SetActive(true);
        resultPanel.transform.DOLocalMoveY(0, 5f);
        inGame = false;

        await UniTask.Delay(TimeSpan.FromSeconds(2));
         
        int score = 0;
        
        DOVirtual.Int(0, (int)ScoreManager.instance.score.Value/100, 5f, value =>
        {
            score = value;
            finishScoreText.text = score.ToString("0") + " 個";
        });
        
        
        await UniTask.Delay(TimeSpan.FromSeconds(5));
        finishScoreText.transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.3f).OnComplete(() =>
        {
            finishScoreText.transform.DOScale(new Vector3(1f, 1f, 1), 0.2f);
        })
            .OnComplete(() =>
            {
                EndBGM.Play();
            });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && inGame)
        {
            SceneManager.LoadScene("TestScene");
        }
    }
}
