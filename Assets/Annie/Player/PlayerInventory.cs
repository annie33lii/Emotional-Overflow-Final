using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//collectible / exchange system for things like 药草、白水晶、整容药、变形药 etc
public class PlayerInventory : MonoBehaviour
{
    private Dictionary<string, int> itemStorage = new Dictionary<string, int>();
    private const string HealthFruitName = "生命果"; // heals the player when picked
    private const string WoodenStickName = "木棍";   // equips a stick weapon
    private const string DisguisePotionName = "整容药"; // toggles disguise state

    private Playerhealth playerHealth;
    private Animator playerAnimator;
    private PlayerAbilityManager abilityManager;

    private void Awake()
    {
        playerHealth = GetComponent<Playerhealth>();
        playerAnimator = GetComponent<Animator>();
        abilityManager = GetComponent<PlayerAbilityManager>();
    }

    public void AddItem(string itemName, int amount = 1)
    {
        if (!itemStorage.ContainsKey(itemName))
            itemStorage[itemName] = 0;

        itemStorage[itemName] += amount;
        Debug.Log($"✅ 已添加: {itemName} ×{amount}, 当前总量: {itemStorage[itemName]}");

        if (itemName == HealthFruitName)
        {
            int healAmount = 20 * Mathf.Max(1, amount);

            if (playerHealth != null)
                playerHealth.Heal(healAmount);
            else
                Debug.LogWarning("⚠️ Playerhealth component not found; cannot heal.");

            if (playerAnimator != null)
                playerAnimator.SetTrigger("Heal");
            else
                Debug.LogWarning("⚠️ Animator component not found; cannot play heal animation.");
        }

        if (itemName == WoodenStickName)
        {
            if (abilityManager != null)
            {
                abilityManager.hasWeapon = true;
                Debug.Log("玩家已装备木棍，切换为武器攻击。");
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerAbilityManager component not found; cannot flag weapon state.");
            }
        }

        if (itemName == DisguisePotionName && abilityManager != null)
        {
            abilityManager.isDisguised = true;
            Debug.Log("玩家使用整容药，进入伪装状态。");
        }
        else if (itemName == DisguisePotionName)
        {
            Debug.LogWarning("⚠️ PlayerAbilityManager component not found; 无法进入伪装状态。");
        }
    }

    public bool HasItem(string itemName, int amount = 1)
    {
        return itemStorage.ContainsKey(itemName) && itemStorage[itemName] >= amount;
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        if (HasItem(itemName, amount))
        {
            itemStorage[itemName] -= amount;
            if (itemStorage[itemName] <= 0)
                itemStorage.Remove(itemName);
        }

        Debug.Log($"Removed {itemName} x{amount}");
        PrintInventory();
    }

    public void UseItem(string itemName)
    {
        if (HasItem(itemName))
        {
            itemStorage[itemName]--;
            Debug.Log($"Used one {itemName}");
            PrintInventory();
        }

    }

    public void PrintInventory()
    {
        if (itemStorage.Count == 0)
        {
            Debug.Log("（背包为空）");
            return;
        }

        Debug.Log($"当前背包共有 {itemStorage.Count} 种物品：");
        foreach (var kvp in itemStorage)
        {
            Debug.Log($"   {kvp.Key} ×{kvp.Value}");
        }
    }
    
    // ✅ Add this method for Backpack UI
    public Dictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(itemStorage);
    }
}
