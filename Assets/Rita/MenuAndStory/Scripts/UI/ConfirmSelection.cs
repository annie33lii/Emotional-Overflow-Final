using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfirmSelection : MonoBehaviour
{
    [Header("二选一按钮组")]
    public UI_OrganButtonGroup organGroup;

    [Header("下一个场景名")]
    public string nextSceneName = "StoryAfterOrgan";

    public void OnConfirmClicked()
    {
        if (organGroup == null)
        {
            Debug.LogError("ConfirmSelection: 没绑定按钮组！");
            return;
        }

        int choice = organGroup.selectedIndex;  // 0 = 左, 1 = 右
        Debug.Log($"当前选择 = {(choice == 0 ? "左边" : "右边")}");

        // 保存到 PlayerProfile
        if (PlayerProfile.Instance != null)
        {
            PlayerProfile.Instance.faceChoice = choice;
            PlayerProfile.Instance.SaveToPrefs();
            Debug.Log($"已保存玩家选择: faceChoice = {choice}");
        }
        else
        {
            Debug.LogWarning("PlayerProfile.Instance 为空，无法保存");
            PlayerPrefs.SetInt("FaceChoice", choice);
        }

        // 切换场景
        SceneManager.LoadScene(nextSceneName);
    }
}
