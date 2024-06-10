using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Text freeTextField;
    [SerializeField] private Text hoveredTextField;

    private Button button;

    private bool onTarget;

    private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onTarget = true;
        if (button.interactable)
            HoveredTextActive();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onTarget = false;
        FreeTextActive();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FreeTextActive();
    }

    public void FreeTextActive()
    {
        freeTextField.gameObject.SetActive(true);
        hoveredTextField.gameObject.SetActive(false);
    }

    public void HoveredTextActive()
    {
        freeTextField.gameObject.SetActive(false);
        hoveredTextField.gameObject.SetActive(true);
    }

    public bool IsInteractable()
    {
        return button.interactable;
    }

    public void SetInteractable(bool val)
    {
        button.interactable = val;
    }

    public bool IsOnTarget()
    {
        return onTarget;
    }

    public void SetOnTarget(bool hovered)
    {
        onTarget = hovered;
    }
}
