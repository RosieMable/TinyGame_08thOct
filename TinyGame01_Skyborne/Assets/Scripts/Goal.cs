using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    Collider2D collider;


    private void Awake()
    {
        if (this.GetComponent<Collider2D>() == null)
        {
            collider = gameObject.AddComponent<Collider2D>();
            collider.isTrigger = true;
        }
        else
        {
            collider = GetComponent<Collider2D>();
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {

        }
    }
}
