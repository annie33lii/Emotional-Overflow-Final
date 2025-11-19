using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 关键：跨场景不销毁
        }
        else
        {
            Destroy(gameObject); // 防止重复音乐
        }
    }
}
