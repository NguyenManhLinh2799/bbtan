using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void CheckGameOver(Collider2D blockCollider)
    {
        var collider2DComponent = GetComponent<Collider2D>();
        if (collider2DComponent.bounds.Intersects(blockCollider.bounds))
        {
            playerController.drawable = false;
            playerController.canShoot = false;
            gameOverPanel.SetActive(true);
        }
    }
}
