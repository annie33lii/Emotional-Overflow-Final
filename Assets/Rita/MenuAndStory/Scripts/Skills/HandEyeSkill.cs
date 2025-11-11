using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HandEyeSkill : MonoBehaviour
{
    [Header("光圈参数")]
    public Light2D playerLight;           // 手动或自动获取 Light2D
    public float normalRadius = 1.5f;
    public float extendedRadius = 4.0f;
    public float transitionSpeed = 3f;

    public float normalIntensity = 1.5f;
    public float extendedIntensity = 3f;

    private bool isExpanding = false;

    void Start()
    {
        // 自动查找 Light2D（防止手动漏绑）
        if (playerLight == null)
            playerLight = GetComponentInChildren<Light2D>();

        if (playerLight == null)
        {
            Debug.LogWarning("❌ 没找到 Light2D，请在 Inspector 里手动拖入。");
            this.enabled = false; // 防止 Update 出错
            return;
        }
    }

    void Update()
    {
        // 鼠标左键按下 / 松开控制状态
        if (Input.GetMouseButtonDown(0))
            isExpanding = true;
        if (Input.GetMouseButtonUp(0))
            isExpanding = false;

        // 如果光源存在，更新参数
        if (playerLight != null)
        {
            float targetRadius = isExpanding ? extendedRadius : normalRadius;
            float targetIntensity = isExpanding ? extendedIntensity : normalIntensity;

            playerLight.pointLightOuterRadius = Mathf.Lerp(
                playerLight.pointLightOuterRadius,
                targetRadius,
                Time.deltaTime * transitionSpeed
            );
            playerLight.intensity = Mathf.Lerp(
                playerLight.intensity,
                targetIntensity,
                Time.deltaTime * transitionSpeed
            );
        }
    }
}
