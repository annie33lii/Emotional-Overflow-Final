using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sense Item", menuName = "Items/Sense Item")]
public class SenseItem : ScriptableObject
{
    public string itemName;
    public SenseType senseType;      // Which sense it belongs to (Eye, Ear, Nose...)
    public BodyPartType slotType;    // Which body slot it can be equipped to
    public Sprite icon;
    [TextArea] public string description;

    public bool grantsVision;
    public bool grantsSmell;
    public bool grantsWeapon;
    public bool grantsStealSkill;
}
// Supporting Enums
public enum BodyPartType
    {
        Head,
        LeftHand,
        RightHand
    }

    public enum SenseType
    {
        Eye,
        Ear,
        Mouth,
        Nose
    }
