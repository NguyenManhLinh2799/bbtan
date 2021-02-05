using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorRandomizer : Shootable
{
    public int lifeSpan = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
        }
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
