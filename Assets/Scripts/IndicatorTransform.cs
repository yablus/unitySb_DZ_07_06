using UnityEngine;
using UnityEngine.UI;

public class IndicatorTransform : MonoBehaviour
{
    [SerializeField] private Image indicatorBgImg;
    [SerializeField] private Image indicatorIconImg;
    [SerializeField] private Sprite indicatorIcon;
    [SerializeField] private Text indicatorText;

    [SerializeField] private float toDeltaX;
    [SerializeField] private float toDeltaY;

    private float fromX;
    private float fromY;

    private RectTransform indicatorTransform;

    private float timer = 0;
    private float maxTime = 1;

    private int count = 0;
    private bool startShow;

    private void Start()
    {
        indicatorTransform = indicatorBgImg.GetComponent<RectTransform>();

        indicatorIconImg.sprite = indicatorIcon;
        indicatorIconImg.SetNativeSize();
        fromX = indicatorBgImg.rectTransform.localPosition.x;
        fromY = indicatorBgImg.rectTransform.localPosition.y;
        indicatorText.color = new Color(0, 0, 0, 0);
        indicatorIconImg.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        if (startShow)
        {
            if (count > 0)
                indicatorText.text = "+" + count;
            else if (count < 0)
                indicatorText.text = count.ToString();
            timer = maxTime;

            startShow = false;
        }
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (count != 0)
            {
                if (count > 0)
                    indicatorText.color = new Color(0, 1, 0, timer / maxTime);
                else
                    indicatorText.color = new Color(1, 0, 0, timer / maxTime);
                indicatorIconImg.color = new Color(1, 1, 1, timer / maxTime);

                indicatorTransform.anchoredPosition =
                    new Vector3(fromX + toDeltaX * (1 - timer), fromY + toDeltaY * (1 - timer));
            }
        }
    }

    public void Show(int c)
    {
        count = c;
        startShow = true;
    }

    public void Hide()
    {
        timer = 0;
        indicatorText.color = new Color(0, 0, 0, 0);
        indicatorIconImg.color = new Color(1, 1, 1, 0);
    }
}
