using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetAreaManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform greenArea;
    [SerializeField] 
    private RectTransform redArea;

    public void SetRange(AllRange newRage)
    {
        float nodeValue = gameObject.GetComponent<RectTransform>().rect.xMax;
        float greenMax = nodeValue * newRage.green.right;
        float greenMin = nodeValue * newRage.green.left;
        float redMax = (greenMax - greenMin) * newRage.red.right;
        float redMin = (greenMax - greenMin) * newRage.red.left;

        greenArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, greenMax - greenMin);
        greenArea.localPosition = new Vector3(greenMin, 0, 0);
        redArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, redMax - redMin);
        redArea.localPosition = new Vector3(redMin, 0, 0);
    }
}
