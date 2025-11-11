using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudNPCAnimation : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalking(bool walking)
    {
        animator.SetBool("isWalking", walking);
    }

    public void PlayAttack()
    {
        animator.SetBool("isAttacking", true);
        Invoke(nameof(StopAttack), 0.5f); // adjust to match animation length
    }

    private void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void PlayHurt()
    {
        animator.SetTrigger("isHurt");
    }

    public void PlayDeath()
    {
        animator.SetBool("isDead", true);
    }
}
