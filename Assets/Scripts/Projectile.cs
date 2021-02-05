using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 5f;
    PlayerController playerController;
    bool hasReturned = false;

    bool firstCollision = true;
    Vector2 lastCollisionPos;
    Vector2 currentCollisionPos;

    [SerializeField] GameObject vectorRandomizerPrebfab;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        GetComponent<Rigidbody2D>().velocity = playerController.GetVelocityNormalized() * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Prevent from inconsistent velocity
        if (GetComponent<Rigidbody2D>() != null)
        {
            var vConsistent = GetComponent<Rigidbody2D>().velocity;
            vConsistent = vConsistent.normalized * speed;
            GetComponent<Rigidbody2D>().velocity = vConsistent;
        }

        if (transform.position.y < playerController.transform.position.y && !hasReturned)
        {
            hasReturned = true;
            if (!playerController.relocating && !playerController.relocated)
            {
                playerController.Relocate(transform.position.x);
            }
        }

        if (hasReturned)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                Destroy(GetComponent<Rigidbody2D>());
            }

            var factor = transform.position.x < playerController.transform.position.x ? 1 : -1;
            var deltaX = factor * speed * Time.deltaTime;
            transform.position = new Vector2(transform.position.x + deltaX, playerController.transform.position.y);

            var factor2 = transform.position.x < playerController.transform.position.x ? 1 : -1;
            if (factor * factor2 < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            collision.gameObject.GetComponent<Block>().DecreaseHpBy1();
        }

        // Prevent from endless collision
        if (firstCollision)
        {
            firstCollision = false;
            lastCollisionPos = currentCollisionPos = transform.position;
        }
        else
        {
            lastCollisionPos = currentCollisionPos;
            currentCollisionPos = transform.position;
            if (Mathf.Abs(currentCollisionPos.y-lastCollisionPos.y) <= 0.1f && Mathf.Abs(currentCollisionPos.x-lastCollisionPos.x) >= 5f)
            {
                var vr = Instantiate(vectorRandomizerPrebfab, (currentCollisionPos + lastCollisionPos) / 2, Quaternion.identity);
                vr.GetComponent<VectorRandomizer>().lifeSpan = 1;
            }
        }
    }
}
