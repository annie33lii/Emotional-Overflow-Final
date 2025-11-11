using UnityEngine;
using UnityEngine.UI;

public class ClickToCloseImage : MonoBehaviour
{
    public GameObject devTeamPanel;  // 拖入整个面板（不是图片）

    public void OnClickClose()
    {
        if (devTeamPanel != null)
        {
            devTeamPanel.SetActive(false);
            Debug.Log("🖼️ 点击开发团队图片关闭面板");
        }
    }
}
