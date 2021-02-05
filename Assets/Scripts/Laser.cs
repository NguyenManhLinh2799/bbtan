using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float lifeSpan = 0.1f;

    private void Start()
    {
        var allBlocks = FindObjectsOfType<Block>();
        var collider2DComponent = GetComponent<Collider2D>();
        foreach (var block in allBlocks)
        {
            if (collider2DComponent.bounds.Intersects(block.GetComponent<Collider2D>().bounds))
            {
                block.DecreaseHpBy1();
            }
        }
        Destroy(gameObject, lifeSpan);
    }
}
