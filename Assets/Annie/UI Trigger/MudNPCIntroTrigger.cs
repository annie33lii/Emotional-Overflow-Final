using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudNPCIntroTrigger : MonoBehaviour
{
    public MudNPC mudNPC;
    public GameObject instructionUI; // 引导界面3
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (instructionUI != null)
            {
                if (instructionUI != null)
                UIManager.OpenUI(instructionUI);
            }

            Debug.Log("引导界面3 triggered.");
        }
    }

    public void ClosePanel()
    {
        if (instructionUI.activeSelf)
        {
            UIManager.CloseUI(instructionUI);
            Debug.Log("引导界面3 closed by clicking the panel.");
        }
    }
}
