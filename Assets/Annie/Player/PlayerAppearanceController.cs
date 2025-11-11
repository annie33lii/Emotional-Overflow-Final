using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public RuntimeAnimatorController twoEyeController;
    public RuntimeAnimatorController oneEyeController;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        int faceChoice = 0;

        // Read from PlayerProfile if available
        if (PlayerProfile.Instance != null)
        {
            faceChoice = PlayerProfile.Instance.faceChoice;
            Debug.Log($"[PlayerAppearanceController] Loaded faceChoice={faceChoice} from PlayerProfile");
        }
        else
        {
            Debug.LogWarning("[PlayerAppearanceController] No PlayerProfile found, defaulting to Two-Eye");
        }

        // Switch Animator according to faceChoice
        if (faceChoice == 0)
            animator.runtimeAnimatorController = twoEyeController;
        else
            animator.runtimeAnimatorController = oneEyeController;

        Debug.Log($"[PlayerAppearanceController] Using controller: {(faceChoice == 0 ? "TwoEye" : "OneEye")}");
    }
}
