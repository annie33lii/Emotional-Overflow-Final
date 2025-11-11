using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameItem", menuName = "Items/GameItem")]
public class GameItem : ScriptableObject
{
    public string itemName;        // 药草, 白水晶, 紫水晶, etc.
    public string itemType;        // 宝石, 药品, 生物材料...
    public string description;     // 用途或说明
    public Sprite icon;            // 显示图标
    public bool isConsumable;  

    [Header("Backpack UI")]
    public Sprite descriptionImage; 
}
