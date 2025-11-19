using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExitInteraction : MonoBehaviour
{
    [Header("结局视觉元素")]
    public Image fadeImage;            // 🖤 黑幕
    public VideoPlayer endingVideo;    // 🎬 结局视频（直接播放）

    [Header("UI 提示")]
    public GameObject notEnoughPanel;  // 🚫 材料不足提示图片（默认隐藏）

    private bool isPlayerNear = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            playerInventory = null;
        }
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.X))
        {
            TryActivateExit();
        }
    }

    void TryActivateExit()
    {
        if (playerInventory == null) return;

        bool hasAll = playerInventory.HasItem("Calm Stone", 2)
                   && playerInventory.HasItem("Flow Stone", 1)
                   && playerInventory.HasItem("Void Stone", 2);

        if (hasAll)
        {
            // ✅ 扣除材料
            playerInventory.RemoveItem("Calm Stone", 2);
            playerInventory.RemoveItem("Flow Stone", 1);
            playerInventory.RemoveItem("Void Stone", 2);

            // ✅ 播放结局流程（直接黑幕→视频）
            StartCoroutine(PlayEndingSequence());
        }
        else
        {
            // ❌ 材料不足，显示提示图片
            if (notEnoughPanel != null)
            {
                StartCoroutine(ShowNotEnoughPanel());
            }
        }
    }

    IEnumerator PlayEndingSequence()
    {
        // 1️⃣ 黑幕渐黑
        float duration = 2.5f;
        float t = 0;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / duration);
            fadeImage.color = c;
            yield return null;
        }

        // 2️⃣ 播放结局视频
        if (endingVideo != null)
        {
            endingVideo.Play();
            yield return new WaitForSeconds((float)endingVideo.length);
        }

        // 3️⃣ 退出游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator ShowNotEnoughPanel()
    {
        notEnoughPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        notEnoughPanel.SetActive(false);
    }
}
