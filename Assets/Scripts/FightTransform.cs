using UnityEngine;
using UnityEngine.UI;

public class FightTransform : MonoBehaviour
{
    [SerializeField] private Image cloudImg;
    [SerializeField] private Text cloudText;

    [SerializeField] private Image axeImg;
    private float axToX;
    private float axToY;
    private float axFromDeltaX;
    private float axFromDeltaY;

    [SerializeField] private Image swordImg;
    private float swToX;
    private float swToY;
    private float swFromDeltaX;
    private float swFromDeltaY;

    private RectTransform axeTransform;
    private RectTransform swordTransform;

    private float timer = 0;
    private float maxTime = 2;

    private bool startShow;

    private void Start()
    {
        axeTransform = axeImg.GetComponent<RectTransform>();
        swordTransform = swordImg.GetComponent<RectTransform>();

        SetDefaultXY();
        SetDefaultColors();
    }

    private void Update()
    {
        if (startShow)
        {
            timer = maxTime;
            startShow = false;
        }
        if (timer >= 0) {
            timer -= Time.deltaTime;
            // if (timer >= from && timer < (from + step))
            if (timer >= 1.125 && timer < 1.25)
                {
                    // a = 1 - (timer - from) / (timerMax - step)
                    axeImg.color = new Color(1, 1, 1, (1 - ((timer - 1.125f) / (maxTime - 1.875f))));
                    swordImg.color = new Color(1, 1, 1, (1 - ((timer - 1.125f) / (maxTime - 1.875f))));

                }
                if (timer >= 1 && timer < 1.25)
                {
                    // x = to + delta * ((timer - from) / (timerMax - step))
                    axeTransform.anchoredPosition =
                        new Vector3(axToX + axFromDeltaX * ((timer - 1f) / (maxTime - 1.75f)),
                                    axToY + axFromDeltaY * ((timer - 1f) / (maxTime - 1.75f)));
                    swordTransform.anchoredPosition =
                        new Vector3(swToX + swFromDeltaX * ((timer - 1f) / (maxTime - 1.75f)),
                                    swToY + swFromDeltaY * ((timer - 1f) / (maxTime - 1.75f)));
                }
                if (timer >= 0 && timer < 1)
                {
                    // a = (timer - from) / (timerMax - step)
                    cloudImg.color = new Color(1, 1, 1, (timer / (maxTime - 1)));
                    axeImg.color = new Color(1, 1, 1, (timer / (maxTime - 1)));
                    swordImg.color = new Color(1, 1, 1, (timer / (maxTime - 1)));
                    cloudText.color = new Color(1, 0, 0, (timer / (maxTime - 1)));
                }
        }
    }

    private void SetDefaultXY()
    {
        axToX = axeImg.rectTransform.localPosition.x;
        axToY = axeImg.rectTransform.localPosition.y;
        axFromDeltaX = 480;
        axFromDeltaY = 0;

        swToX = swordImg.rectTransform.localPosition.x;
        swToY = swordImg.rectTransform.localPosition.y;
        swFromDeltaX = -480;
        swFromDeltaY = 0;
    }

    private void SetDefaultColors()
    {
        cloudImg.color = new Color(1, 1, 1, 0);
        axeImg.color = new Color(1, 1, 1, 0);
        swordImg.color = new Color(1, 1, 1, 0);
        cloudText.color = new Color(1, 0, 0, 0);
    }

    public void Show()
    {
        startShow = true;
    }

    public void Hide()
    {
        timer = 0;
        SetDefaultColors();
    }
}
