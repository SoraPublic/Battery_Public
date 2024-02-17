using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryColor : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image gauge;
    [SerializeField]
    private Color color_1;
    [SerializeField]
    private Color color_2;
    [SerializeField] 
    private Color color_3;
    [SerializeField] 
    private Color color_4;

    private void Start()
    {
        SetGaugeColor();
    }

    public void SetGaugeColor()
    {
        if (slider.value > 0.75f)
        {
            gauge.color = Color.Lerp(color_2, color_1, (slider.value - 0.75f) * 4f);
        }
        else if (slider.value > 0.25f)
        {
            gauge.color = Color.Lerp(color_3, color_2, (slider.value - 0.25f) * 4f);
        }
        else
        {
            gauge.color = Color.Lerp(color_4, color_3, slider.value * 4f);
        }
    }
}
