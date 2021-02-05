using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    Vector2 target;
    bool goingDown = false;
    float moveSpeed = 5f;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (goingDown)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target) < 0.001f)
            {
                transform.position = target;
                goingDown = false;
                FindObjectOfType<GameOverTrigger>().CheckGameOver(GetComponent<Collider2D>());
            }
        }
    }

    public virtual void GoDown()
    {
        target = new Vector2(transform.position.x, transform.position.y - 0.8f);
        goingDown = true;
    }
}
