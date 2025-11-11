using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillTutorialTrigger : MonoBehaviour
{
    public GameObject instructionUI; // 引导界面2
    private bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (instructionUI != null)
            {
                if (instructionUI != null)
                UIManager.OpenUI(instructionUI);
            }

            Debug.Log("引导界面2 triggered.");
        }
    }

    public void ClosePanel()
    {
        if (instructionUI.activeSelf)
        {
            UIManager.CloseUI(instructionUI);
            Debug.Log("引导界面2 closed by clicking the panel.");
        }
    }
}
