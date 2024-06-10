using UnityEngine;
using UnityEngine.UI;

public class ImageOnOff : MonoBehaviour
{
    [SerializeField] private Sprite oldSprite;
    [SerializeField] private Sprite newSprite;

    private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        img.sprite = oldSprite;
    }

    public void ToNew()
    {
        img.sprite = newSprite;
    }

    public void ToOld()
    {
        img.sprite = oldSprite;
    }
}
