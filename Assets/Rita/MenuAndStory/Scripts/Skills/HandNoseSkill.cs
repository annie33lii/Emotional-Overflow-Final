using UnityEngine;

public class HandNoseSkill : MonoBehaviour
{
    [Header("检测气体障碍范围")]
    public float detectRadius = 3f;

    [Header("连接线材质（可空，会自动生成）")]
    public Material lineMaterial;

    [Header("情绪光（自动获取 Player 上的 EmotionLightSkill）")]
    public EmotionLightSkill emotionLight;   // ← 新增字段

    private LineRenderer lineRenderer;
    private bool isNoseActive = false;

    void Start()
    {
        // -----------------------------
        // 获取情绪光脚本
        // -----------------------------
        if (emotionLight == null)
            emotionLight = FindObjectOfType<EmotionLightSkill>();
        //（如果 Player 身上有就会自动找到）

        // -----------------------------
        // 确保 LineRenderer 存在
        // -----------------------------
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        if (lineMaterial == null)
            lineMaterial = new Material(Shader.Find("Sprites/Default"));

        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // 鼻子技能开启
        if (Input.GetMouseButtonDown(1))
        {
            isNoseActive = true;
            lineRenderer.enabled = true;
            Debug.Log("👃 鼻子技能激活");

            ActivateGasBarriers(true);

            // 💚 开启绿色情绪光
            if (emotionLight != null)
                emotionLight.ActivateNoseLight();
        }

        // 鼻子技能关闭
        if (Input.GetMouseButtonUp(1))
        {
            isNoseActive = false;
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            Debug.Log("❌ 鼻子技能关闭");

            ActivateGasBarriers(false);

            // 💚 关闭绿色情绪光
            if (emotionLight != null)
                emotionLight.DeactivateNoseLight();
        }

        if (isNoseActive)
            DrawConnectionToGas();
    }

    void DrawConnectionToGas()
    {
        GameObject[] gases = GameObject.FindGameObjectsWithTag("Gas");
        GameObject nearest = null;
        float nearestDist = float.MaxValue;

        foreach (GameObject gas in gases)
        {
            float dist = Vector2.Distance(transform.position, gas.transform.position);
            if (dist < nearestDist && dist <= detectRadius)
            {
                nearest = gas;
                nearestDist = dist;
            }
        }

        if (nearest != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nearest.transform.position);
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void ActivateGasBarriers(bool allowPass)
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("GasBarrier");

        foreach (GameObject barrier in barriers)
        {
            Collider2D col = barrier.GetComponent<Collider2D>();
            SpriteRenderer sr = barrier.GetComponent<SpriteRenderer>();

            if (col != null)
                col.enabled = !allowPass;

            if (sr != null)
                sr.color = allowPass ?
                    new Color(1, 1, 1, 0.3f) : // 半透明
                    Color.white;
        }
    }
}
