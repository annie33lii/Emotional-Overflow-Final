using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_OrganButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Image References")]
    public Image imgIcon;
    public Image imgFrame;

    [Header("所属按钮组（可选）")]
    public UI_OrganButtonGroup buttonGroup;

    private bool isSelected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 如果有组，就让组来统一管理选中状态
        if (buttonGroup != null)
        {
            buttonGroup.OnButtonClicked(this);
        }
        else
        {
            // 否则自己单独切换
            ToggleSelection();
        }
    }

    public void ToggleSelection()
    {
        SetSelected(!isSelected);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;

        if (imgFrame == null)
        {
            Debug.LogWarning("[UI_OrganButton] imgFrame is null on " + name);
            return;
        }

        imgFrame.gameObject.SetActive(isSelected);
        if (isSelected) imgFrame.transform.SetAsLastSibling();

        var c = imgFrame.color; c.a = 1f; imgFrame.color = c;
    }

    public bool IsSelected() => isSelected;
}
