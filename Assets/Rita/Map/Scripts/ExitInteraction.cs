using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExitInteraction : MonoBehaviour
{
    [Header("结局视觉元素")]
    public GameObject endingImage;     // 🎨 静态结局图
    public Image fadeImage;            // 🖤 黑幕
    public VideoPlayer endingVideo;    // 🎬 结局视频

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

        bool hasAll = playerInventory.HasItem("白水晶", 2)
                   && playerInventory.HasItem("紫水晶", 1)
                   && playerInventory.HasItem("黑曜石", 2);

        if (hasAll)
        {
            // ✅ 扣除材料
            playerInventory.RemoveItem("白水晶", 2);
            playerInventory.RemoveItem("紫水晶", 1);
            playerInventory.RemoveItem("黑曜石", 2);

            // ✅ 播放结局流程
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
        // 1️⃣ 显示静态图
        endingImage.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        // 2️⃣ 画面渐黑
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

        // 3️⃣ 播放结局视频
        if (endingVideo != null)
        {
            endingVideo.Play();
            yield return new WaitForSeconds((float)endingVideo.length);
        }

        // 4️⃣ 结束游戏
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
