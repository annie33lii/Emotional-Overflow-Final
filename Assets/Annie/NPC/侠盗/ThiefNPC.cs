using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefNPC : DialogueNPC
{
    [Header("Thief Special Settings")]
    public bool hasPuzzleTriggered = false;
    
    [Header("Thief Skill Teaching")]
    public GameObject miniGameUI;   // 小游戏界面
    public GameObject successUI;    // 成功界面
    public GameObject guideUI;   

    private bool skillLearned = false;

    public override void Interact()
    {
        // ✅ First, run base dialogue logic
        base.Interact();

        if (skillLearned)
        {
            Debug.Log($"{npcName}: 你已经学会【偷盗】技能，快去试试看吧！");
            return;
        }
    }

    protected override void OnDialogueEnd()
    {
        base.OnDialogueEnd(); // keep base behavior (e.g., open exchange)
        
        if (!hasPuzzleTriggered)
        {
            hasPuzzleTriggered = true;
            TriggerThiefPuzzle();
        }
    }

    private void TriggerThiefPuzzle()
    {
        if (miniGameUI)
        {
            miniGameUI.SetActive(true);
            Debug.Log(">>> 小游戏界面已打开：开始偷盗学习流程");
        }
    }

    public void OnMiniGameResult(bool success)
{
    if (miniGameUI)
            miniGameUI.SetActive(false);

        if (success)
        {
            skillLearned = true;
            Debug.Log($"{npcName}: 哇哦！你成功掌握了【偷盗】技能！");
            if (successUI) successUI.SetActive(true);
            if (guideUI) guideUI.SetActive(true);
        }
        else
        {
            Debug.Log($"{npcName}: 差一点点，再试一次吧！");
            TriggerThiefPuzzle();
        }
}
}
