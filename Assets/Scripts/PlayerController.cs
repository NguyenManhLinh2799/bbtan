using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 startPoint;
    float vectorMinY = 0.5f;
    float vectorMinX = 0.1f;
    Vector2 directionalVector;
    float aspectRatio = 9 / 16f;

    [SerializeField] GameObject circlePrefab;
    int maxCircles = 50;
    GameObject[] circles;
    [SerializeField] GameObject path;
    [SerializeField] GameObject cannon;
    public bool drawable = false;

    [SerializeField] GameObject projectilePrefab;
    public bool canShoot = false;
    public int ammo = 1;
    float delayTime = 0.1f;

    Vector2 target;
    public bool relocating = false;
    public bool relocated = false;
    float moveSpeed = 5f;

    void Start()
    {
        circles = new GameObject[maxCircles];
        for (int i = 0; i < maxCircles; i++)
        {
            circles[i] = Instantiate(circlePrefab, path.transform.position, Quaternion.identity);
            circles[i].transform.parent = path.transform;
        }
        path.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (relocating)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, target) < 0.001f)
            {
                relocated = true;
                relocating = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0) && drawable)
        {
            Vector2 endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            directionalVector = startPoint - endPoint;
            DrawPath();
        }
        else if (Input.GetMouseButtonUp(0) && canShoot)
        {
            drawable = false;
            canShoot = false;
            StartCoroutine(ShootRepeatedly());
        }
    }

    void RotateCannon()
    {
        var dVector = directionalVector * 100f;
        var angle = Mathf.Atan(dVector.y / dVector.x) * Mathf.Rad2Deg;
        if (angle < 0)
        {
            angle += 180;
        }
        cannon.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    IEnumerator ShootRepeatedly()
    {
        path.SetActive(false);

        int n = ammo;
        for (int i = 0; i < n; i++)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(delayTime);
        }

        FindObjectOfType<Level>().isShooting = true;
    }

    public Vector2 GetVelocityNormalized()
    {
        return directionalVector.normalized;
    }

    float LinearFunctionX(float y)
    {
        if (directionalVector.y < vectorMinY)
        {
            directionalVector.y = vectorMinY;
        }

        if (Mathf.Abs(directionalVector.x) < vectorMinX)
        {
            RotateCannon();
            return transform.position.x + directionalVector.x / directionalVector.y * (y - transform.position.y);
        }

        float factor = directionalVector.x * directionalVector.y > 0 ? 1 : -1;

        float boundX = factor * (Camera.main.orthographicSize * aspectRatio - projectilePrefab.transform.localScale.x / 2);
        float boundY = transform.position.y + directionalVector.y / directionalVector.x * (boundX - transform.position.x);

        RotateCannon();

        return boundX - directionalVector.x / directionalVector.y * Mathf.Abs(y - boundY);
    }

    void DrawPath()
    {
        if (directionalVector.y < 0)
        {
            path.SetActive(false);
            canShoot = false;
            return;
        }

        path.SetActive(true);
        canShoot = true;

        float startY = transform.position.y;
        for (int i = 0; i < maxCircles; i++)
        {
            Vector2 pos = new Vector2(LinearFunctionX(startY), startY);
            circles[i].transform.position = pos;
            startY += 0.2f;
        }
    }

    public void Relocate(float x)
    {
        relocating = true;
        target = new Vector2(x, transform.position.y);
    }
}
