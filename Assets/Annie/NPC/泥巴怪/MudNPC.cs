using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NPCHealth))]
public class MudNPC : MonoBehaviour
{
    // === Attack parameters ===
    public float attackRange = 1.2f;
    public int attackDamage = 20;
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private Transform player;
    private Playerhealth playerHealth;

    // // === Audio ===
    // public AudioSource screamSound;

    // === Animation ===
    private Animator animator;
    private NPCHealth health;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<NPCHealth>();
    }

    void Update()
    {
        if (health == null) return;

        // Stop logic if NPC is dead
        if (isDead) return;

        if (player == null)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        // Distance check
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            AttackPlayer();
        }
        else if (distance > attackRange)
        {
            // Example: walking towards player (if you add movement later)
            animator.SetBool("isWalking", false);
        }
    }

    void AttackPlayer()
    {
        lastAttackTime = Time.time;

        // 🎬 Play attack animation
        animator.SetBool("isAttacking", true);
        Invoke(nameof(ResetAttackAnimation), 0.5f); // adjust to match clip length

        // if (screamSound != null)
        //     screamSound.Play();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Mud NPC attacked player.");
        }
    }

    void ResetAttackAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerHealth = other.GetComponent<Playerhealth>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            playerHealth = null;
            animator.SetBool("isWalking", false);
        }
    }

    // public void Scream()
    // {
    //     // 🎬 Optional scream animation
    //     if (animator != null) animator.SetTrigger("isHurt"); 
    //     // if (screamSound != null) screamSound.Play();
    //     else Debug.Log($"{name} screamed (no AudioSource assigned).");
    // }

    // === NEW METHODS ===
    public void OnTakeDamage()
    {
        if (isDead) return;
        animator.SetTrigger("getHurt");
    }

    public void OnDeath()
    {
        isDead = true;
        animator.SetBool("Die", true);
        // Optionally disable collider/AI after short delay
        Destroy(gameObject, 2f);
    }
}
