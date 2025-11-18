using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkillLightController : MonoBehaviour
{
    [Header("形状光（Freeform Additive）")]
    public Light2D shapeLight;

    [Header("真实照明光（Spot Light Multiply）")]
    public Light2D revealLight;

    public float transitionSpeed = 4f;

    private float targetIntensity = 0f;
    private float targetShapeScale = 0.1f;
    private float targetRevealRadius = 0.1f;
    private Color targetColor = Color.white;

    void Start()
    {
        shapeLight.intensity = 0;
        shapeLight.transform.localScale = Vector3.one * 0.1f;

        revealLight.intensity = 0;
        revealLight.pointLightOuterRadius = 0.1f;
    }

    void Update()
    {
        // 形状光大小
        float s = Mathf.Lerp(shapeLight.transform.localScale.x, targetShapeScale, Time.deltaTime * transitionSpeed);
        shapeLight.transform.localScale = new Vector3(s, s, 1);

        shapeLight.intensity = Mathf.Lerp(shapeLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);
        shapeLight.color = Color.Lerp(shapeLight.color, targetColor, Time.deltaTime * transitionSpeed);

        // 实际照明光
        revealLight.intensity = Mathf.Lerp(revealLight.intensity, targetIntensity, Time.deltaTime * transitionSpeed);
        revealLight.pointLightOuterRadius = Mathf.Lerp(revealLight.pointLightOuterRadius, targetRevealRadius, Time.deltaTime * transitionSpeed);
    }

    // =======================
    // 🟡 眼睛技能（黄色）
    // =======================
    public void EyeSkill(float charge)
    {
        targetColor = new Color(1f, 0.9f, 0f); // yellow
        targetIntensity = 1f;

        targetShapeScale = Mathf.Lerp(0.1f, 3f, charge);
        targetRevealRadius = Mathf.Lerp(1f, 4f, charge);
    }

    public void StopEyeSkill()
    {
        targetIntensity = 0;
        targetShapeScale = 0.1f;
        targetRevealRadius = 0.1f;
    }

    // =======================
    // 💚 鼻子技能（绿色）
    // =======================
    public void NoseSkill()
    {
        targetColor = new Color(0f, 1f, 0.4f); // green
        targetIntensity = 1f;

        targetShapeScale = 2f;
        targetRevealRadius = 2.5f;
    }

    public void StopNoseSkill()
    {
        targetIntensity = 0;
        targetShapeScale = 0.1f;
        targetRevealRadius = 0.1f;
    }
}
