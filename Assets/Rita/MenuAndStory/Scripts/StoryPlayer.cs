using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryPlayer : MonoBehaviour
{
    [Header("故事图片 UI")]
    public Image storyImage; // 显示故事图片的UI

    [Header("故事图片数组")]
    public Sprite[] storySprites; // 所有故事图

    [Header("下个场景")]
    public string nextSceneName = "New_Face";

    private int currentIndex = 0; // 当前播放的图片索引
    private bool canClick = true; // 防止连点

    void Start()
    {
        if (storySprites == null || storySprites.Length == 0)
        {
            Debug.LogError("[StoryPlayer] 没有设置故事图片");
            return;
        }

        currentIndex = 0;
        storyImage.sprite = storySprites[currentIndex];
        Debug.Log($"[StoryPlayer] 显示第 {currentIndex + 1}/{storySprites.Length} 张图");
    }

    void Update()
    {
        // 鼠标左键 或 任意键
        if (canClick && (Input.GetMouseButtonDown(0) || Input.anyKeyDown))
        {
            canClick = false;
            NextStory();
            Invoke(nameof(EnableClick), 0.2f);
        }
    }

    void EnableClick() => canClick = true;

    void NextStory()
    {
        // 在递增前检查是否还有图
        if (currentIndex + 1 < storySprites.Length)
        {
            currentIndex++;
            storyImage.sprite = storySprites[currentIndex];
            Debug.Log($"[StoryPlayer] 切换到第 {currentIndex + 1}/{storySprites.Length} 张图");
        }
        else
        {
            // 播放完最后一张才切换场景
            Debug.Log($"[StoryPlayer] 故事播放完毕，准备进入下一个场景：{nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
