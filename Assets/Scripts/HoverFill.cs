using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverFill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image fillImage;
    public float hoverTime = 0.5f;

    float timer;
    bool hovering;

    void Update()
    {
        if (hovering)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        timer = Mathf.Clamp(timer, 0f, hoverTime);
        fillImage.fillAmount = timer / hoverTime;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}