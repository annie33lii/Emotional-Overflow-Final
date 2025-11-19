using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    public GameObject bgmPrefab;
    private static bool hasBGM = false;

    void Awake()
    {
        if (!hasBGM)
        {
            Instantiate(bgmPrefab);
            hasBGM = true;
        }
    }
}
