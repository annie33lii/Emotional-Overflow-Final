using UnityEngine;
using UnityEngine.UI;

public class UI_OrganStatusPanel : MonoBehaviour
{
    [Header("左边小人图")]
    public Image characterImage;         // 左边小人 Image
    public Sprite leftCharacterSprite;   // 左选时显示
    public Sprite rightCharacterSprite;  // 右选时显示

    [Header("右边五官状态整图")]
    public Image organStatusImage;       // 右边五官 Image
    public Sprite leftOrganSprite;       // 左选时显示的五官图
    public Sprite rightOrganSprite;      // 右选时显示的五官图

    [Header("控制器引用（切换页）")]
    public UI_OrganBackpack backpack;    // 用于切换到选择页

    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (PlayerProfile.Instance == null)
        {
            Debug.LogWarning("[UI_OrganStatusPanel] PlayerProfile.Instance 为空");
            return;
        }

        int choice = PlayerProfile.Instance.faceChoice; // 0=左,1=右

        // 左边小人切换
        if (characterImage)
            characterImage.sprite = (choice == 0) ? leftCharacterSprite : rightCharacterSprite;

        // 右边整图切换
        if (organStatusImage)
            organStatusImage.sprite = (choice == 0) ? leftOrganSprite : rightOrganSprite;

        // 保持比例
        if (characterImage) characterImage.preserveAspect = true;
        if (organStatusImage) organStatusImage.preserveAspect = true;
    }

    public void OnClickAdjust()
    {
        if (backpack != null) backpack.OpenSelect();
    }
}
