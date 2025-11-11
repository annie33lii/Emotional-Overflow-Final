using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverLayer;
    public GameObject normalLayer;   // 👈 新增：Normal Layer 引用

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverLayer != null)
            hoverLayer.SetActive(true);

        if (normalLayer != null)
            normalLayer.SetActive(false); // 👈 悬停时隐藏 Normal 层
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoverLayer != null)
            hoverLayer.SetActive(false);

        if (normalLayer != null)
            normalLayer.SetActive(true); // 👈 移开时恢复 Normal 层
    }
}
