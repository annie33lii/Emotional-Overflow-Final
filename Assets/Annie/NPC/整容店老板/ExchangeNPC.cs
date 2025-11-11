using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExchangeNPC : MonoBehaviour
{
    [System.Serializable]
    public class ItemRequirement
    {
        public string itemName;
        public int amount;
    }

    [System.Serializable]
    public class ExchangeRecipe
    {
        public string resultItem;
        public List<ItemRequirement> requiredItems;
    }

    public List<ExchangeRecipe> exchangeList = new List<ExchangeRecipe>();
    private PlayerInventory playerInventory;
    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            OpenExchangeMenu();
        }
    }

    public void OpenExchangeMenu()
    {
        Debug.Log("Opened Exchange Menu");
        for (int i = 0; i < exchangeList.Count; i++)
        {
            Debug.Log($"{i}. Can exchange for {exchangeList[i].resultItem}");
        }

        // Example: try first recipe automatically (replace with UI later)
        TryExchange(0);
    }

    public void TryExchange(int index)
    {
        if (index < 0 || index >= exchangeList.Count) return;
        var recipe = exchangeList[index];
        bool canTrade = true;

        foreach (var req in recipe.requiredItems)
        {
            if (!playerInventory.HasItem(req.itemName, req.amount))
            {
                canTrade = false;
                Debug.Log($"缺少 {req.itemName}");
                break;
            }
        }

        if (canTrade)
        {
            foreach (var req in recipe.requiredItems)
                playerInventory.RemoveItem(req.itemName, req.amount);

            playerInventory.AddItem(recipe.resultItem);
            Debug.Log($"交易成功！获得 {recipe.resultItem}");
        }
        else
        {
            Debug.Log("材料不足，无法交易。");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            Debug.Log("按 E 进行交易");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}
