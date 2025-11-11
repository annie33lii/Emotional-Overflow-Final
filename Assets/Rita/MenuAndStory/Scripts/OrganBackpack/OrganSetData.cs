using UnityEngine;

[System.Serializable]
public class OrganSet
{
    [Tooltip("五个槽位图标（眼/耳/鼻/口/眉）")]
    public Sprite[] slotIcons = new Sprite[5];

    [Tooltip("整行预览图（例如图2或图3里的一整行）")]
    public Sprite rowPreview;
}

[CreateAssetMenu(fileName = "OrganCatalog", menuName = "Game/Organ Catalog")]
public class OrganCatalog : ScriptableObject
{
    [Header("左边选择对应的组合页（图2）")]
    public OrganSet[] leftSets;

    [Header("右边选择对应的组合页（图3）")]
    public OrganSet[] rightSets;
}
