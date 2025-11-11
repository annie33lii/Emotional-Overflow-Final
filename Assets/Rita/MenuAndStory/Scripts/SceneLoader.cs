using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 点击“开始游戏”
    public void GoToStoryBeforeFace()
    {
        SceneManager.LoadScene("StoryBeforeFace");
    }

    public void GoToMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
