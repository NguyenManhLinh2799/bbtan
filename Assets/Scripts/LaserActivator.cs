using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActivator : Shootable
{
    [SerializeField] GameObject laserPrefab;
    int lifeSpan = 5;

    private void Start()
    {
        if (Random.value <= 0.5f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 90f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(laserPrefab, transform.position, transform.rotation);
    }

    public override void GoDown()
    {
        base.GoDown();
        DecreaseLifeSpanBy1();
    }

    void DecreaseLifeSpanBy1()
    {
        lifeSpan--;
        if (lifeSpan == 0)
        {
            Destroy(gameObject);
        }
    }
}
