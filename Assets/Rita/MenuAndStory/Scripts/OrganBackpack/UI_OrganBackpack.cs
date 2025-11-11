


using UnityEngine;

public class UI_OrganBackpack : MonoBehaviour
{
    [Header("UI 根节点")]
    public GameObject root;

    [Header("两页")]
    public GameObject panelStatus;   // 当前装备状态页
    public GameObject panelSelect;   // 选择页

    [Header("控制脚本引用")]
    public UI_OrganStatusPanel statusPanel;
    public UI_OrganSelectPanel selectPanel;

    void Start()
    {
        // 确保 root 存在
        if (root == null)
        {
            Debug.LogWarning("Backpack root 未绑定！");
            root = this.gameObject;
        }

        // 启动时默认隐藏整个背包UI
        if (root != null)
            root.SetActive(false);

        // 默认进入状态页
        ShowStatusPage();
    }

    // =============================
    // 给外部（例如 Trigger 或按钮）调用的接口
    // =============================

    /// <summary>
    /// 打开“当前装备状态”页
    /// </summary>
    public void OpenStatus()
    {
        if (root != null && !root.activeSelf)
            root.SetActive(true);

        ShowStatusPage();

        Debug.Log("打开五官状态页");
        if (statusPanel != null)
            statusPanel.Refresh();
        else
            Debug.LogWarning(" statusPanel 未绑定！");
    }

    /// <summary>
    /// 打开“调整装备选择”页
    /// </summary>
    public void OpenSelect()
    {
        if (root != null && !root.activeSelf)
            root.SetActive(true);

        ShowSelectPage();

        Debug.Log("打开五官选择页");
        if (selectPanel == null)
            Debug.LogWarning("⚠selectPanel 未绑定！");
    }

    /// <summary>
    /// 完全关闭背包
    /// </summary>
    public void Close()
    {
        if (root != null)
            root.SetActive(false);

        Debug.Log("✅ 关闭五官背包");
    }

    // =============================
    // 内部页面切换逻辑
    // =============================

    void ShowStatusPage()
    {
        if (panelStatus) panelStatus.SetActive(true);
        if (panelSelect) panelSelect.SetActive(false);
    }

    void ShowSelectPage()
    {
        if (panelStatus) panelStatus.SetActive(false);
        if (panelSelect) panelSelect.SetActive(true);
    }
}
