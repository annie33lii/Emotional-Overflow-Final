using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerInventory inventory; //trade items
    private PlayerBackpack backpack; //sense items
    private Animator animator;

    // player ability states
    public bool eyesOpen;
    public bool noseActive;
    public bool isDisguised = false;
    public bool hasWeapon = false; // toggled when player equips a weapon such as the stick

    [Header("Attack Settings")]
    public float attackRange = 1f;
    public int attackDamage = 20;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackDelay = 0.3f;  // time before damage applies
    public float attackCooldown = 1.0f;
    private float nextAttackTime = 0f;

    // If disguise comes from an item or buff, you can check it dynamically:
    public bool IsDisguised()
    {
        return isDisguised;
    }
    
        public void ActivateDisguise(float duration)
    {
        StartCoroutine(DisguiseRoutine(duration));
    }

    private IEnumerator DisguiseRoutine(float duration)
    {
        isDisguised = true;
        Debug.Log("玩家已伪装！");
        yield return new WaitForSeconds(duration);
        isDisguised = false;
        Debug.Log("伪装效果结束。");
    }

    void Start()
    {
        // link to PlayerInventory and backpack
        backpack = GetComponent<PlayerBackpack>();
        inventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        Debug.Log("Animator Controller at Start: " + animator.runtimeAnimatorController?.name);
    }

    void Update()
    {
        //Prevent attacks if a UI panel is open
        if (UIManager.isUIOpen)
            return;

        if (Input.GetMouseButton(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // =====================
    //  ABILITY CHECKS
    // =====================

    public bool CanSee() => backpack.HasEyes();
    public bool CanSmell() => backpack.HasNose();
    public bool CanAttack()
    {
        bool backpackWeapon = backpack != null && backpack.HasWeapon();
        return hasWeapon || backpackWeapon;
    }
    public bool CanSteal() => backpack.HasStealSkill();

    // =====================
    //  ATTACK LOGIC
    // =====================

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        if (hitEnemies.Length == 0)
        {
            // no enemies nearby — do nothing
            Debug.Log("No enemies in range — no attack triggered.");
            return;
        }

        // 1️⃣ Trigger correct animation
        if (CanAttack())  // weapon equipped
        {
            animator.SetTrigger("AttackStick");
            Debug.Log("Player attacking with stick.");
        }
        else  // bare hands
        {
            animator.SetTrigger("AttackFist");
            Debug.Log("Player attacking with fist.");
        }

        // 2️⃣ Apply damage slightly delayed to match animation timing
        StartCoroutine(DealDamageAfterDelay(attackDelay, hitEnemies));
    }

    private IEnumerator DealDamageAfterDelay(float delay, Collider2D[] hitEnemies)
    {
        yield return new WaitForSeconds(delay);


        foreach (Collider2D enemy in hitEnemies)
        {
            NPCHealth npcHealth = enemy.GetComponent<NPCHealth>();
            if (npcHealth != null)
            {
                npcHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy hit successfully!");
            }
        }
    }

    // =====================
    //  COMPATIBILITY METHOD
    // =====================
    // This is just to stay compatible with your existing PickUpItem script.
    // It forwards AddItem calls to the PlayerInventory.
    public void AddItem(string itemName, int amount)
{
    // Try to treat it as a SenseItem first
    SenseItem senseItem = TryFindSenseItem(itemName);

    if (senseItem != null)
    {
        if (backpack != null && backpack.TryEquip(senseItem))
        {
            Debug.Log($"Equipped Sense Item: {itemName}");
            return;
        }
        else
        {
            Debug.Log("装不上了！");
            return;
        }
    }

    // Otherwise, treat as normal pickup
    if (inventory != null)
    {
        inventory.AddItem(itemName, amount);
    }
    else
    {
        Debug.LogWarning("No PlayerInventory found on player!");
    }
}

// Helper function (temporary replacement for ItemDatabase)
private SenseItem TryFindSenseItem(string itemName)
{
    // You can later replace this with a real ItemDatabase lookup
    SenseItem[] allSenseItems = Resources.LoadAll<SenseItem>("");
    foreach (var item in allSenseItems)
    {
        if (item.itemName == itemName)
            return item;
    }
    return null;
}

}
