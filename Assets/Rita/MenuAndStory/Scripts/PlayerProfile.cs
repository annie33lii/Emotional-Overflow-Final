//using UnityEngine;
//using System.Collections.Generic;

//public class PlayerProfile : MonoBehaviour
//{
//    public static PlayerProfile Instance;

//    [Header("五官选择结果")]
//    public int eyeIndex = -1;
//    public int mouthIndex = -1;
//    public int earIndex = -1;
//    public int noseIndex = -1;

//    [Header("剧情和进度")]
//    public bool storyCompleted = false;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void SaveChoice(string organName, int index)
//    {
//        switch (organName)
//        {
//            case "eye": eyeIndex = index; break;
//            case "mouth": mouthIndex = index; break;
//            case "ear": earIndex = index; break;
//            case "nose": noseIndex = index; break;
//        }
//    }

//    public void PrintStatus()
//    {
//        Debug.Log($"[Profile] eye={eyeIndex}, mouth={mouthIndex}, ear={earIndex}, nose={noseIndex}");
//    }
//}

using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile Instance;

    [Header("玩家在开头的选择：0 = 左边, 1 = 右边")]
    public int faceChoice = 0;

    [Header("当前套装索引（背包选择页中第几套）")]
    public int currentSetIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 保持场景切换时不销毁
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 从本地加载上次保存的数据
        faceChoice = PlayerPrefs.GetInt("FaceChoice", faceChoice);
        currentSetIndex = PlayerPrefs.GetInt("CurrentSetIndex", currentSetIndex);

        Debug.Log($"[PlayerProfile] 加载数据: faceChoice={faceChoice}, currentSetIndex={currentSetIndex}");
    }

    // 保存当前选择
    public void SaveToPrefs()
    {
        PlayerPrefs.SetInt("FaceChoice", faceChoice);
        PlayerPrefs.SetInt("CurrentSetIndex", currentSetIndex);
        PlayerPrefs.Save();

        Debug.Log($"[PlayerProfile] 已保存数据: faceChoice={faceChoice}, currentSetIndex={currentSetIndex}");
    }
}
