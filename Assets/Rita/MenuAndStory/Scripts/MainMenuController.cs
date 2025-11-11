using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button devTeamButton;
    public Button quitButton;

    [Header("Dev Team Panel")]
    public GameObject devTeamPanel;

    void Start()
    {
        // 默认隐藏开发团队面板
        if (devTeamPanel != null)
            devTeamPanel.SetActive(false);

        // 注册事件（防止忘记拖）
        if (devTeamButton != null)
            devTeamButton.onClick.AddListener(OpenDevTeamPanel);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    // 打开开发团队面板
    public void OpenDevTeamPanel()
    {
        if (devTeamPanel != null)
        {
            devTeamPanel.SetActive(true);
            Debug.Log("打开开发团队面板");
        }
    }

    // 关闭开发团队面板（如果你想用到）
    public void CloseDevTeamPanel()
    {
        if (devTeamPanel != null)
        {
            devTeamPanel.SetActive(false);
            Debug.Log("关闭开发团队面板");
        }
    }

    // 退出游戏
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器下退出
#else
        Application.Quit(); // 构建后退出
#endif
        Debug.Log("🚪 退出游戏");
    }
}
