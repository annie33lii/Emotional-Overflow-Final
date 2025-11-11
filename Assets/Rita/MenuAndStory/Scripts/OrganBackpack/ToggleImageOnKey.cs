using UnityEngine;
using UnityEngine.UI;

public class ToggleImageOnKey : MonoBehaviour
{
    [Header("要显示/隐藏的图片")]
    public GameObject imageObject;  // 拖入那张 UI 图片

    [Header("触发按键")]
    public KeyCode toggleKey = KeyCode.E;

    void Start()
    {
        // 初始时隐藏
        if (imageObject != null)
            imageObject.SetActive(false);
    }

    void Update()
    {
        // 按下E键时切换显示状态
        if (Input.GetKeyDown(toggleKey))
        {
            if (imageObject != null)
            {
                bool isActive = imageObject.activeSelf;
                imageObject.SetActive(!isActive);
                Debug.Log(isActive ? "图片隐藏" : "图片显示");
            }
        }
    }
}
