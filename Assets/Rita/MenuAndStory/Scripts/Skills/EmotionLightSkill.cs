using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EmotionLightSkill : MonoBehaviour
{
    public Light2D skillLight;
    public float maxScale = 2.5f;   // 光圈最大缩放
    public float fadeSpeed = 4f;    // 淡入淡出速度

    private float targetIntensity = 0;
    private float targetScale = 0.1f;

    void Update()
    {
        // 平滑改变光圈大小（Freeform Light 用 transform.localScale）
        float s = Mathf.Lerp(skillLight.transform.localScale.x, targetScale, Time.deltaTime * fadeSpeed);
        skillLight.transform.localScale = new Vector3(s, s, 1);

        // 平滑改变亮度
        skillLight.intensity = Mathf.Lerp(skillLight.intensity, targetIntensity, Time.deltaTime * fadeSpeed);
    }

    // 💚 鼻子技能光（绿色）
    public void ActivateNoseLight()
    {
        skillLight.color = new Color(0f, 1f, 0.4f); // 绿色光
        targetScale = maxScale;
        targetIntensity = 1f;
    }

    public void DeactivateNoseLight()
    {
        targetScale = 0.1f;
        targetIntensity = 0f;
    }

    // 💛 眼睛技能光（黄色）
    public void ActivateEyeLight(float charge)
    {
        skillLight.color = new Color(1f, 0.9f, 0f); // 黄色光
        targetScale = Mathf.Lerp(0.3f, maxScale, charge);
        targetIntensity = 1f;
    }

    public void DeactivateEyeLight()
    {
        targetScale = 0.1f;
        targetIntensity = 0f;
    }
}
