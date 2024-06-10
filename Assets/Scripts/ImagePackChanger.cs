using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePackChanger : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprSml = new List<Sprite>();
    [SerializeField] private List<Sprite> sprMed = new List<Sprite>();
    [SerializeField] private List<Sprite> sprBig = new List<Sprite>();
    [SerializeField] private List<Sprite> sprMax = new List<Sprite>();

    private Image img;

    private List<Sprite> sprCurrent = new List<Sprite>();

    private int currentPackIndex;
    private bool peasantsCountChanged;

    private float fill;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (peasantsCountChanged)
        {
            currentPackIndex++;
            ChangePack();
        }

        if (fill > 0.90)
            img.sprite = sprCurrent[3];
        else if (fill > 0.55)
            img.sprite = sprCurrent[2];
        else if (fill > 0.25)
            img.sprite = sprCurrent[1];
        else
            img.sprite = sprCurrent[0];

        if (peasantsCountChanged)
            img.SetNativeSize();

        peasantsCountChanged = false;
    }

    private void ChangePack()
    {
        switch (currentPackIndex)
        {
            case 1: sprCurrent = sprMed; break;
            case 2: sprCurrent = sprBig; break;
            case 3: sprCurrent = sprMax; break;
            default: sprCurrent = sprSml; break;
        }
    }

    public void ToCurrent()
    {
        peasantsCountChanged = true;
    }

    public void ToDefault()
    {
        fill = 0;
        currentPackIndex = 0;
        ChangePack();
        img.sprite = sprSml[0];
        img.SetNativeSize();
    }

    public void SetFill(float f)
    {
        fill = f;
    }
}
