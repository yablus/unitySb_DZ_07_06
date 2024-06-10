using UnityEngine;
using UnityEngine.UI;

public class ImageHider : MonoBehaviour
{
    private Image img;

    private bool hide;
    private bool peasantsCountChanged;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (!hide && peasantsCountChanged)
        {
            img.color = new Color(1, 1, 1, 0);
            hide = true;
            peasantsCountChanged = false;
        }
    }

    public void ToHide()
    {
        peasantsCountChanged = true;
    }

    public void ToVisible()
    {
        img.color = new Color(1, 1, 1, 1);
        hide = false;
    }
}
