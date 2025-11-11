using UnityEngine;

public class UI_OrganBackpackTrigger : MonoBehaviour
{
    [Header("要打开的五官背包 UI")]
    public UI_OrganBackpack organBackpack;

    [Header("是否打开中（可调试）")]
    public bool isOpen = false;

    void Update()
    {
        // 检测按键 —— 使用和你队友相同的方式
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (organBackpack == null)
            {
                organBackpack = FindObjectOfType<UI_OrganBackpack>();
                if (organBackpack == null)
                {
                    Debug.LogWarning("⚠️ 没找到 UI_OrganBackpack，无法打开背包");
                    return;
                }
            }

            // 开关逻辑
            isOpen = !isOpen;
            organBackpack.gameObject.SetActive(isOpen);

            Debug.Log(isOpen ? "🎒 打开五官背包" : "🎒 关闭五官背包");

            if (isOpen)
                organBackpack.OpenStatus();  // 调用 UI 内部刷新逻辑
        }
    }
}
