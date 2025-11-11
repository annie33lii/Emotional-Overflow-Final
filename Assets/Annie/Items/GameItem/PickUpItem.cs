using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject pickupUI;  // UI prompt, e.g. "Press R to pick up"
    public SenseItem senseItemData; // 五官装备
    public GameItem gameItemData;   // 普通拾取物 (药草、水晶、药品)
    public string itemName;      // fallback if no itemData
    public int amount = 1;

    private bool playerInRange = false;
    private PlayerAbilityManager abilityManager;

    private PlayerInventory inventory;


    void Start()
    {
        if (pickupUI != null)
            pickupUI.SetActive(false);

        //automatically show gameitem sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (gameItemData != null && gameItemData.icon != null) {
                sr.sprite = gameItemData.icon;
                Debug.Log($"✅ Sprite set to {gameItemData.icon.name} for {gameItemData.itemName}");
            }

            else if (senseItemData != null && senseItemData.icon != null)
                sr.sprite = senseItemData.icon;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"▶️ TriggerEnter detected with {other.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player entered pickup range");
            playerInRange = true;
            inventory = other.GetComponent<PlayerInventory>();

            if (inventory == null)
                Debug.LogWarning("⚠️ PlayerInventory not found on Player!");

            if (pickupUI != null)
                pickupUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (pickupUI != null)
                pickupUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.R))
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        string collectedName = "";

        // handle normal GameItem
        if (gameItemData != null)
        {
            collectedName = gameItemData.itemName;
            if (inventory != null)
            {
                inventory.AddItem(collectedName, amount);
                Debug.Log($"[拾取成功] 获得 {collectedName} ×{amount}");
            }
        }

        // 如果只是 itemName 文本
        else
        {
            var inventory = abilityManager.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemName, amount);
                Debug.Log($"[拾取成功] 获得 {itemName} ×{amount}");
                Debug.Log("📦 当前背包状态：");
                inventory.PrintInventory();

                BackpackUIManager ui = FindObjectOfType<BackpackUIManager>();
                if (ui != null)
                    ui.UpdateInventoryDisplay();
            }
        }

        if (pickupUI != null)
        {
            pickupUI.SetActive(false);
        }

        Destroy(gameObject);
        }
}
