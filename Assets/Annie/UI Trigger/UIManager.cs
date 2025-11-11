using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static bool isUIOpen = false;

    public static void OpenUI(GameObject panel)
    {
        panel.SetActive(true);
        isUIOpen = true;
    }

    public static void CloseUI(GameObject panel)
    {
        panel.SetActive(false);
        isUIOpen = false;
    }
}
