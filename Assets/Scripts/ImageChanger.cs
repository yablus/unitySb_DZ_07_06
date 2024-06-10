using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    [SerializeField] private Sprite oldSprite;
    [SerializeField] private Sprite newSprite;

    private Image img;

    private bool changed;
    private bool peasantsCountChanged;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (!changed && peasantsCountChanged)
        {
            img.sprite = newSprite;
            img.SetNativeSize();
            changed = true;
            peasantsCountChanged = false;
        }
    }

    public void ChangeSprite()
    {
        peasantsCountChanged = true;
    }

    public void DefaultSprite()
    {
        img.sprite = oldSprite;
        img.SetNativeSize();
        changed = false;
    }
}
