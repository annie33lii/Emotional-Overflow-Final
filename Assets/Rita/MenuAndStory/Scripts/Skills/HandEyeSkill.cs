using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HandEyeSkill : MonoBehaviour
{
    [Header("Spot Light 2D")]
    public Light2D spotLight;   // ← 这里绑定你的玩家聚光灯！

    [Header("观察光圈设置")]
    public float normalRadius = 1.5f;       // 默认黑暗里的小光圈
    public float extendedRadius = 4.0f;     // 长按后变大的光圈
    public float transitionSpeed = 3f;

    public Color normalColor = new Color(1f, 0.9f, 0f, 1f);  // 黄色
    public Color extendedColor = new Color(1f, 0.9f, 0f, 1f);

    private bool isExpanding = false;

    void Start()
    {
        if (spotLight == null)
        {
            spotLight = GetComponentInChildren<Light2D>();
            if (spotLight == null)
            {
                Debug.LogWarning("❌ 没找到 Spot Light，请手动绑定！");
                this.enabled = false;
                return;
            }
        }
    }

    void Update()
    {
        // 鼠标左键控制扩光
        if (Input.GetMouseButtonDown(0))
            isExpanding = true;

        if (Input.GetMouseButtonUp(0))
            isExpanding = false;

        // 根据状态调整光照
        float targetRadius = isExpanding ? extendedRadius : normalRadius;

        spotLight.pointLightOuterRadius = Mathf.Lerp(
            spotLight.pointLightOuterRadius,
            targetRadius,
            Time.deltaTime * transitionSpeed
        );

        // 颜色保持黄色主题
        spotLight.color = isExpanding ? extendedColor : normalColor;
    }
}
