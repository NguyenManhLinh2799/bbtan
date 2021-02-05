using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : Shootable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<PlayerController>().ammo++;
        Destroy(gameObject);
    }
}
