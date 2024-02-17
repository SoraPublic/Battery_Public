using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBatteries : MonoBehaviour
{
    [SerializeField]
    private GameObject walls;
    [SerializeField]
    private float wallBreakTime;

    private void OnEnable()
    {
        StartCoroutine(WallBreak(wallBreakTime));
    }

    private IEnumerator WallBreak(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(walls);
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
