using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    int best = 1;
    public int level = 1;

    [SerializeField] PlayerController playerController;

    [Header("Blocks")]
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject triangleBottomLeftBlockPrefab;
    [SerializeField] GameObject triangleBottomRightBlockPrefab;
    [SerializeField] GameObject triangleTopLeftBlockPrefab;
    [SerializeField] GameObject triangleTopRightBlockPrefab;

    [Header("Others")]
    [SerializeField] GameObject plusPrefab;
    [SerializeField] GameObject vectorRandomizerPrefab;
    [SerializeField] GameObject laserActivatorPrefab;
    int maxCol = 7;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI levelScore;
    [SerializeField] TextMeshProUGUI bestScore;

    public bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get prefs
        best = PlayerPrefs.GetInt("best", 1);
        bestScore.text = best.ToString();
        levelScore.text = level.ToString();

        Spawn();
        GoDown();
    }

    private void Update()
    {
        // Go to next level if there's no projectile left and player has relocated;
        if (isShooting)
        {
            if (FindObjectsOfType<Projectile>().Length == 0 && playerController.relocated)
            {
                isShooting = false;
                Next();
            }
        }
    }

    void Next()
    {
        level++;
        Spawn();
        GoDown();
    }

    void GoDown()
    {
        var shootableObjs = FindObjectsOfType<Shootable>();
        foreach(var shootable in shootableObjs)
        {
            shootable.GoDown();
        }
    }

    void Spawn()
    {
        // Setup
        var row = new int[maxCol];

        // Plus
        var plusPos = Random.Range(0, maxCol);
        row[plusPos] = 1;

        // Vector Randomizer
        float probability = 1 / 5f;
        if (Random.value <= probability)
        {
            while (true)
            {
                var vrPos = Random.Range(0, maxCol);
                if (row[vrPos] == 0)
                {
                    row[vrPos] = 2;
                    break;
                }
            }
        }

        // Laser Activator
        if (Random.value <= probability)
        {
            while (true)
            {
                var laPos = Random.Range(0, maxCol);
                if (row[laPos] == 0)
                {
                    row[laPos] = 3;
                    break;
                }
            }
        }

        // Block
        for (int i = 0; i < 7; i++)
        {
            if (row[i] == 0)
            {
                var rand = Random.Range(-1, 1);
                row[i] = rand;
            }
        }

        // Spawn
        SpawnRow(row);

        // Enable
        playerController.drawable = true;
        playerController.canShoot = true;
        playerController.relocated = false;

        // Update UI
        levelScore.text = level.ToString();
        if (level > best)
        {
            best = level;
            bestScore.text = best.ToString();
        }
    }

    private void SpawnRow(int[] row)
    {
        Vector2 col = new Vector2(-2.4f, 4.5f);
        for (int i = 0; i < maxCol; i++)
        {
            if (row[i] == -1)
            {
                SpawnBlock(col);
            }
            else if (row[i] == 1)
            {
                Instantiate(plusPrefab, col, Quaternion.identity);
            }
            else if (row[i] == 2)
            {
                Instantiate(vectorRandomizerPrefab, col, Quaternion.identity);
            }
            else if (row[i] == 3)
            {
                Instantiate(laserActivatorPrefab, col, Quaternion.identity);
            }
            col.x += 0.8f;
        }
    }

    private void SpawnBlock(Vector2 col)
    {
        var sample = Random.value;
        if (sample <= 0.1f)
        {
            Instantiate(triangleBottomLeftBlockPrefab, col, Quaternion.identity);
        }
        else if (sample <= 0.2f)
        {
            Instantiate(triangleBottomRightBlockPrefab, col, Quaternion.identity);
        }
        else if (sample <= 0.3f)
        {
            Instantiate(triangleTopLeftBlockPrefab, col, Quaternion.identity);
        }
        else if (sample <= 0.4f)
        {
            Instantiate(triangleTopRightBlockPrefab, col, Quaternion.identity);
        }
        else if (sample <= 0.5f)
        {
            var block = Instantiate(blockPrefab, col, Quaternion.identity);
            block.GetComponent<Block>().isDoubled = true;
            block.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            Instantiate(blockPrefab, col, Quaternion.identity);
        }
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("best", best);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("best", best);
    }
}
