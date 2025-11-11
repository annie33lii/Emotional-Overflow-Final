using UnityEngine;

public class UI_OrganButtonGroup : MonoBehaviour
{
    [Header("左右两个按钮")]
    public UI_OrganButton leftButton;
    public UI_OrganButton rightButton;

    [Header("当前选中索引 (0=左, 1=右, -1=未选)")]
    public int selectedIndex = -1;

    void Start()
    {
        // 初始化时全都取消选中
        leftButton.SetSelected(false);
        rightButton.SetSelected(false);
    }

    // 在 Unity Inspector 里让两个按钮的点击事件调用这个方法
    public void OnButtonClicked(UI_OrganButton clicked)
    {
        // 如果重复点击同一个按钮：可视为取消选中（或忽略）
        if (clicked == leftButton)
        {
            leftButton.SetSelected(true);
            rightButton.SetSelected(false);
            selectedIndex = 0;
        }
        else if (clicked == rightButton)
        {
            rightButton.SetSelected(true);
            leftButton.SetSelected(false);
            selectedIndex = 1;
        }

        Debug.Log($"[UI_OrganButtonGroup] Selected Index = {selectedIndex}");
    }
}
