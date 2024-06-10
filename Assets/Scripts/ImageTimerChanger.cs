using UnityEngine;
using UnityEngine.UI;

public class ImageTimerChanger : MonoBehaviour
{
    [SerializeField] private Image parentImg;

    private Image img;
    private float fillAmount;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (parentImg.IsActive())
            img.fillAmount = fillAmount;
        else if (img.fillAmount != 0)
            img.fillAmount = 0;
    }

    public bool ParentIsActive()
    {
        return parentImg.IsActive();
    }

    public void ParentSetActive(bool act)
    {
        parentImg.gameObject.SetActive(act);
    }

    public void SetFillAmount(float f)
    {
        fillAmount = f;
    }
}
