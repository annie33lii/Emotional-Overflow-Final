using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardNPC : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;
    public int attackDamage = 20;
    public float detectionRange = 5f;
    public float attackRange = 1.8f;
    public float attackCooldown = 1.2f;

    private bool isAggro = false;
    private bool isDead = false;
    private float nextAttackTime = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // If player is within detection range
        if (distance < detectionRange && !isAggro)
        {
            CheckPlayerDisguise();
        }

        // If aggro, follow and attack
        if (isAggro)
        {
            FacePlayer();

            if (distance > attackRange)
            {
                animator.SetBool("isWalking", true);
                MoveTowardsPlayer();
            }
            else
            {
                animator.SetBool("isWalking", false);

                if (Time.time > nextAttackTime)
                {
                    animator.SetTrigger("Attack");
                    AttackPlayer();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void CheckPlayerDisguise()
    {
        PlayerAbilityManager ability = player.GetComponent<PlayerAbilityManager>();
        if (ability != null && ability.isDisguised == false)
        {
            Debug.Log("守卫: 有怪物！");
            animator.SetBool("isAggro", true);
            isAggro = true;
        }
        else
        { 
            Debug.Log("守卫: 无异常，继续巡逻。");
            animator.SetBool("isAggro", false);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, 1.5f * Time.deltaTime);
    }

    void FacePlayer()
    {
        if (player == null) return;
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void AttackPlayer()
    {
        Debug.Log("守卫攻击玩家！");

        Playerhealth health = player.GetComponent<Playerhealth>();
        if (health != null)
        {
            health.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"守卫受到攻击！剩余血量: {currentHealth}");
        animator.SetTrigger("getHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("守卫死亡，掉落生命果1个，白水晶1个。");
        animator.SetTrigger("Die");
        rb.simulated = false;
        GetComponent<Collider2D>().enabled = false;

        // optional: destroy after animation
        Destroy(gameObject, 2f);
    }
}
