using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackpackUIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject backpackPanel;   // whole UI panel
    public TextMeshProUGUI inventoryText; // text box that shows item list

    private PlayerInventory playerInventory;
    private bool isOpen = false;

    void Start()
    {
        if (inventoryText != null)
            inventoryText.text = "Hello Backpack!";
        
        if (backpackPanel != null)
            backpackPanel.SetActive(false);

        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory == null)
            Debug.LogWarning("⚠️ No PlayerInventory found in scene.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToggleBackpack();
        }
    }

    private void ToggleBackpack()
    {
        isOpen = !isOpen;
        if (backpackPanel != null)
            backpackPanel.SetActive(isOpen);

        if (isOpen)
            UpdateInventoryDisplay();
    }

    // ✅ Called by pickup logic (optional) or when backpack is opened
    public void UpdateInventoryDisplay()
    {
        if (inventoryText == null)
        {
            Debug.LogWarning("⚠️ BackpackUIManager: inventoryText not assigned.");
            return;
        }

        // ✅ Always ensure we have the player's inventory before accessing
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            if (playerInventory == null)
            {
                inventoryText.text = "⚠️ 找不到 PlayerInventory (玩家未加载)";
                Debug.LogWarning("⚠️ BackpackUIManager: No PlayerInventory found in scene.");
                return;
            }
        }

        // ✅ Safe now
        var items = playerInventory.GetAllItems();

        if (items == null || items.Count == 0)
        {
            inventoryText.text = "（Backpack is Empty）";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Currently in the Backpack: \n");
        foreach (var kvp in items)
        {
            sb.AppendLine($"{kvp.Key} ×{kvp.Value}");
        }

        inventoryText.text = sb.ToString();
        Debug.Log("[BackpackUIManager] Inventory display updated.");
    }
}