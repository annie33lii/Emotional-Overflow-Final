using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealth : MonoBehaviour
{
    // Health Settings
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private MudNPC mudNPC;
    private Animator animator;

    // Optional Effects
    public GameObject itemDropPrefab;     // e.g. "生命果"
    public AudioClip deathSound;          // optional
    public SpriteRenderer spriteRenderer; // for flash effect
    public float flashDuration = 0.1f;

    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        mudNPC = GetComponent<MudNPC>();
        animator = GetComponent<Animator>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Apply damage to NPC
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{name} took {amount} damage (HP: {currentHealth}/{maxHealth})");

        // Visual flash
        if (spriteRenderer != null)
            StartCoroutine(FlashRed());

        // 🔹 Trigger hurt animation
        if (animator != null)
            animator.SetTrigger("getHurt");

        // 🔹 Notify MudNPC (optional logic change)
        if (mudNPC != null)
            mudNPC.OnTakeDamage();

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log($"{name} has died.");

        // 🔹 Trigger death animation
        if (animator != null)
            animator.SetBool("Die", true);

        // 🔹 Notify MudNPC
        if (mudNPC != null)
            mudNPC.OnDeath();

        // Optional: play death sound
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // Optional: drop item
        if (itemDropPrefab != null)
            Instantiate(itemDropPrefab, transform.position, Quaternion.identity);

        // Delay destruction (so animation/sound can finish)
        Destroy(gameObject, 2.0f);
    }
}
