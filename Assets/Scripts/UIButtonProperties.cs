using UnityEngine;
using UnityEngine.UI;

public class UIButtonProperties : MonoBehaviour
{
    private Image img;
    private void Start()
    {
        img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = 0.5f;
    }
}
