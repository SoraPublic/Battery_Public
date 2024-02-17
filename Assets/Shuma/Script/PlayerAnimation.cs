using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator Conver;
    [SerializeField] Animator Meter1, Meter2;
    Animator animator;

    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        Conver.SetFloat("Speed", speed);

        animator = GetComponent<Animator>();

        TimeManager.instance.time.
            Subscribe(x =>
            {
                if(x == 18 || x == 12 || x == 7 || x == 2 || x == 0)
                {
                    speed += 3;
                    animator.SetTrigger("Test");
                    Meter1.SetTrigger("Test");
                    Meter2.SetTrigger("Test");
                    Conver.SetFloat("Speed", speed);
                }
            })
            .AddTo(this);
    }
}
