using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChargedElovta : MonoBehaviour
{
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(Random.Range(0.3f, 1f), Random.Range(0.6f, 1f)) * Random.Range(70, 130f), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(-100f, 100f));
    }
}
