using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//equipable SenseItems (body parts, weapons)
public class PlayerBackpack : MonoBehaviour
{
    public List<SenseItem> equippedItems = new List<SenseItem>();
    public List<GameItem> collectedItems = new List<GameItem>();
    public int maxTotalItems = 5;
    public int maxHeadItems = 5;
    public int maxHandItems = 1;


    // EQUIP / UNEQUIP LOGIC

    public bool TryEquip(SenseItem item)
    {
        if (equippedItems.Count >= maxTotalItems)
        {
            Debug.Log("装不上了！");
            return false;
        }

        int currentInSlot = CountInSlot(item.slotType);
        int limit = (item.slotType == BodyPartType.Head) ? maxHeadItems : maxHandItems;

        if (currentInSlot >= limit)
        {
            Debug.Log("这里装不上了！");
            return false;
        }

        equippedItems.Add(item);
        Debug.Log($"已装备：{item.itemName}");
        return true;
    }

    public void Unequip(SenseItem item)
    {
        if (equippedItems.Contains(item))
        {
            equippedItems.Remove(item);
            Debug.Log($"已卸下：{item.itemName}");
        }
    }

    private int CountInSlot(BodyPartType slotType)
    {
        int count = 0;
        foreach (var i in equippedItems)
        {
            if (i.slotType == slotType) count++;
        }
        return count;
    }

    // ABILITY CHECK HELPERS

    public bool HasEyes()
    {
        foreach (var i in equippedItems)
        {
            if (i.senseType == SenseType.Eye || i.grantsVision)
                return true;
        }
        return false;
    }

    public bool HasNose()
    {
        foreach (var i in equippedItems)
        {
            if (i.senseType == SenseType.Nose || i.grantsSmell)
                return true;
        }
        return false;
    }

    public bool HasWeapon()
    {
        foreach (var i in equippedItems)
        {
            if (i.grantsWeapon)
                return true;
        }
        return false;
    }

    public bool HasStealSkill()
    {
        foreach (var i in equippedItems)
        {
            if (i.grantsStealSkill)
                return true;
        }
        return false;
    }
}
