using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefMiniGame : MonoBehaviour
{
    public ThiefNPC npc;            // Reference to the 侠盗 NPC
    public RectTransform fairy;     // 蓝妖精 object
    public RectTransform targetZone; // 成功捕捉区域
    public float moveSpeed = 200f;   // 像素/秒
    public KeyCode catchKey = KeyCode.Space;

    private bool movingRight = true;
    private bool gameActive = true;

    void Start()
    {
        ResetFairy();
    }

    void Update()
    {
        if (!gameActive) return;

        // Move fairy horizontally back and forth
        float move = moveSpeed * Time.deltaTime * (movingRight ? 1 : -1);
        fairy.anchoredPosition += new Vector2(move, 0);

        // Reverse direction if hitting edges
        if (fairy.anchoredPosition.x > 250) movingRight = false;
        if (fairy.anchoredPosition.x < -250) movingRight = true;

        // Player tries to catch
        if (Input.GetKeyDown(catchKey))
        {
            TryCatch();
        }
    }

    void TryCatch()
    {
        float distance = Mathf.Abs(fairy.anchoredPosition.x - targetZone.anchoredPosition.x);
        bool success = distance < 40f; // within 40px of the target center

        if (success)
        {
            Debug.Log("捕捉成功！");
            npc.OnMiniGameResult(true);
        }
        else
        {
            Debug.Log("捕捉失败！");
            npc.OnMiniGameResult(false);
        }

        // End game and hide UI
        gameActive = false;
        gameObject.SetActive(false);
    }

    public void ResetFairy()
    {
        fairy.anchoredPosition = new Vector2(-200, fairy.anchoredPosition.y);
        movingRight = true;
        gameActive = true;
    }
}
