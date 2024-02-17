using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

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
    
    /* ----- 変数 ----- */
    public  ReactiveProperty<float> score;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button Ranking;
    [SerializeField] private TextMeshProUGUI comboText;

    public ReactiveProperty<int> combo;
    public int maxCombo = 0;

    private Tween scoreTween;
    private Sequence sequence;

    // Start is called before the first frame update
    void Start()
    {
        combo.Value = 0;
        Ranking.onClick.AddListener(() => naichilab.RankingLoader.Instance.SendScoreAndShowRanking(Mathf.RoundToInt(score.Value)));

        combo.Skip(1).Subscribe(x =>
        {
            if (x > maxCombo)
            {
                maxCombo = x;
            }
            
            comboText.text = x.ToString() + " combo";
            
            if(x == 0) return;
                
            comboText.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.2f).OnComplete(() =>
            {
                comboText.transform.DOScale(new Vector3(1f, 1f, 1), 0.2f);
            });
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(float num)
    {
        score.Value += num;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.DOComplete();
        scoreText.DOText(score.Value.ToString("0") + " %", 0.5f);
    }

    private bool isScoreAnimation = false;
    public void ScoreTextAnimation()
    {
        if(isScoreAnimation) return;

        isScoreAnimation = true;
        scoreText.transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.2f).OnComplete(() =>
        {
            scoreText.transform.DOScale(new Vector3(1f, 1f, 1), 0.2f).OnComplete(() => isScoreAnimation = false);
        });
    }
}
