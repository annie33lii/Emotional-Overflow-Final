using UnityEngine;
using UnityEngine.UI;

public class UI_OrganSelectPanel : MonoBehaviour
{
    [Header("两个底图")]
    public GameObject backgroundLeft;
    public GameObject backgroundRight;

    [Header("调试输出")]
    public Text debugText; // 可选，用于显示当前状态

    private bool hasInitialized = false;

    void OnEnable()
    {
        // 避免第一次加载时空引用
        if (!hasInitialized)
        {
            Initialize();
            hasInitialized = true;
        }

        UpdateBackground();
    }

    void Initialize()
    {
        // 确保 PlayerProfile 存在
        if (PlayerProfile.Instance == null)
        {
            Debug.LogWarning("⚠️ PlayerProfile.Instance 为空，尝试自动查找");
            PlayerProfile found = FindObjectOfType<PlayerProfile>();
            if (found != null) PlayerProfile.Instance = found;
        }
    }

    void UpdateBackground()
    {
        if (PlayerProfile.Instance == null)
        {
            Debug.LogWarning("⚠️ PlayerProfile.Instance 仍为空，无法更新底图");
            if (debugText) debugText.text = "⚠️ PlayerProfile 未找到";
            return;
        }

        int choice = PlayerProfile.Instance.faceChoice; // 0=左,1=右
        bool isLeft = (choice == 0);

        // 控制底图显示
        if (backgroundLeft) backgroundLeft.SetActive(isLeft);
        if (backgroundRight) backgroundRight.SetActive(!isLeft);

        // 控制台打印
        Debug.Log($"✅ Panel_Selected 启动：显示{(isLeft ? "左" : "右")}边底图");

        // 可选 UI 调试文本
        if (debugText)
        {
            debugText.text = $"当前角色：{(isLeft ? "左" : "右")}";
        }
    }
}
