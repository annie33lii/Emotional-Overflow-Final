using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorialTrigger : MonoBehaviour
{
    public GameObject instructionUI; // 引导界面3
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

            Debug.Log("引导界面1 triggered.");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /// detect mouse click while UI is active
        if (UIManager.isUIOpen && Input.GetMouseButtonDown(0)) // 0 = left mouse button
        {
            UIManager.CloseUI(instructionUI);
            Debug.Log("引导界面1 closed (mouse click).");
        }
    }
}
