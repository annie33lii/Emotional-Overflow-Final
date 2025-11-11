using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerhealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform respawnPoint; // assign in Inspector

    private Animator animator;

    [Header("UI")]
    public Slider healthSlider;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // initialize the slider if assigned
        if (healthSlider != null)
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"玩家受到攻击！剩余血量: {currentHealth}");

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        // 🔹 Play hurt animation
        if (animator != null)
            animator.SetTrigger("GetHurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Player died. Respawning...");

        // optional: short delay to show animation
        StartCoroutine(RespawnAfterDelay(1.5f));

        if (healthSlider != null)
            healthSlider.value = currentHealth;
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.position = respawnPoint.position;
        currentHealth = maxHealth;
        if (animator != null)
            animator.ResetTrigger("Die");
    }

}
