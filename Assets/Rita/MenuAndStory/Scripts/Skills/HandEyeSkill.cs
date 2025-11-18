using UnityEngine;

public class HandEyeSkill : MonoBehaviour
{
    [Header("技能光控制器（挂在 Player 上）")]
    public SkillLightController skillLight;

    public float chargeSpeed = 1f;
    private bool isExpanding = false;
    private float charge = 0f;

    void Start()
    {
        // 自动找到 SkillLightController
        if (skillLight == null)
            skillLight = FindObjectOfType<SkillLightController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isExpanding = true;
            charge = 0f;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isExpanding = false;
            skillLight.StopEyeSkill();
        }

        if (isExpanding)
        {
            charge += Time.deltaTime * chargeSpeed;
            charge = Mathf.Clamp01(charge);

            skillLight.EyeSkill(charge);
        }
    }
}
