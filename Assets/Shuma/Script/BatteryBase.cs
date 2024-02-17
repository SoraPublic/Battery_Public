using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class BatteryBase : MonoBehaviour
{
    [SerializeField] Image Charge;
    [SerializeField] GameObject ChargedElovta;

    [SerializeField] GameObject red, green;
    [SerializeField] GameObject Battery;

    [SerializeField] Transform sporn;

    private int before;
    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.instance.score
            .Subscribe(x =>
            {
                
                Charge.fillAmount = (x % 100) / 100f;
            })
            .AddTo(this);

        ScoreManager.instance.score
            .Where(x  => (int)(x / 100) - before > 0)
            .Subscribe(x =>
            {
                StartCoroutine(PushBattery((int)(x / 100) - before));
                before = (int)(x / 100);
            })
            .AddTo(this);
    }

    IEnumerator PushBattery(int x)
    {
        for(int i = 0;i < x;i++)
        {
            red.SetActive(false);
            green.SetActive(true);
            Instantiate(ChargedElovta, sporn.position, Quaternion.identity);

            Battery.transform.DOScale(1.1f, 0.08f).OnComplete(() => Battery.transform.DOScale(1, 0.3f));

            yield return new WaitForSeconds(0.1f);

            red.SetActive(true);
            green.SetActive(false);

            yield return new WaitForSeconds(0.1f);

        }
    }
}
