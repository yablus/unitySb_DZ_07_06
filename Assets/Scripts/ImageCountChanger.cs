using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCountChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();

    private float fill;

    private Image img;
    private Sprite currentSprite;

    private bool wheatCountChanged;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (wheatCountChanged)
            ChangeSprite();
    }

    private void ChangeSprite()
    {
        
        if (fill >= 1)
            currentSprite = spriteList[5];
        else if (fill >= 0.8)
            currentSprite = spriteList[4];
        else if (fill >= 0.6)
            currentSprite = spriteList[3];
        else if (fill >= 0.4)
            currentSprite = spriteList[2];
        else if (fill >= 0.2)
            currentSprite = spriteList[1];
        else
            currentSprite = spriteList[0];

        img.sprite = currentSprite;
        wheatCountChanged = false;
    }

    public void ToCurrent(float f)
    {
        fill = f;
        wheatCountChanged = true;
    }

    public void ToEmpty()
    {
        fill = 0;
        ChangeSprite();
    }
}
