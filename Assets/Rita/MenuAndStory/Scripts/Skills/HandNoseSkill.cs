using UnityEngine;

public class HandNoseSkill : MonoBehaviour
{
    [Header("检测气体障碍范围")]
    public float detectRadius = 3f;

    [Header("连接线材质")]
    public Material lineMaterial;

    private LineRenderer lineRenderer;
    private bool isNoseActive = false;

    void Start()
    {
        // 创建LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // 按下右键 -> 开启嗅觉模式
        if (Input.GetMouseButtonDown(1))
        {
            isNoseActive = true;
            lineRenderer.enabled = true;
            Debug.Log("👃 鼻子技能激活");
            ActivateGasBarriers(true); // ✅ 解除障碍
        }

        // 松开右键 -> 关闭嗅觉模式
        if (Input.GetMouseButtonUp(1))
        {
            isNoseActive = false;
            lineRenderer.enabled = false;
            lineRenderer.positionCount = 0;
            Debug.Log("❌ 鼻子技能关闭");
            ActivateGasBarriers(false); // ✅ 恢复障碍
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

    // ✅ 控制所有气体障碍的状态
    void ActivateGasBarriers(bool allowPass)
    {
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("GasBarrier");
        foreach (GameObject barrier in barriers)
        {
            Collider2D col = barrier.GetComponent<Collider2D>();
            SpriteRenderer sr = barrier.GetComponent<SpriteRenderer>();

            if (col != null)
                col.enabled = !allowPass; // 鼻子开启时关闭碰撞器

            if (sr != null)
                sr.color = allowPass ? new Color(1, 1, 1, 0.3f) : Color.white; // 鼻子开启时变半透明
        }
    }
}
