using UnityEngine;
using UnityEngine.UI;

public class UI_SetButton : MonoBehaviour
{
    public Image previewImage;     // ÏÔÊ¾×éºÏÍ¼
    public GameObject checkMark;   // ¹´Í¼²ã£¨Ä¬ÈÏÒþ²Ø£©

    int myIndex;
    System.Action<int> onClicked;

    public void Setup(int index, Sprite preview, System.Action<int> clicked)
    {
        myIndex = index;
        onClicked = clicked;
        if (previewImage) previewImage.sprite = preview;
        SetSelected(false);
    }

    public void SetSelected(bool sel)
    {
        if (checkMark) checkMark.SetActive(sel);
    }

    public void OnClick()
    {
        onClicked?.Invoke(myIndex);
    }
}
