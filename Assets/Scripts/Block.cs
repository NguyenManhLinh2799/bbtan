using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : Shootable
{
    int hp;
    [SerializeField] TextMeshProUGUI hpText;
    public bool isDoubled = false;

    SpriteRenderer spriteRenderer;

    [SerializeField] GameObject destroyedVFX;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = FindObjectOfType<Level>().level;
        if (isDoubled)
        {
            hp *= 2;
        }
        hpText.text = hp.ToString();
    }

    protected override void Update()
    {
        base.Update();
        var currentColor = spriteRenderer.color;
        float newG = 1f - (float)hp / (float)FindObjectOfType<Level>().level;
        var newColor = new Color(currentColor.r, newG, currentColor.b);
        spriteRenderer.color = newColor;
    }

    public void DecreaseHpBy1()
    {
        hp--;
        hpText.text = hp.ToString();
        if (hp <= 0)
        {
            Instantiate(destroyedVFX, transform.position, destroyedVFX.transform.rotation);
            Destroy(gameObject);
        }
    }
}
