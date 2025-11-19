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

    [Header("Visual Feedback")]
    [Tooltip("Purple fear particle effect that plays while the player is disguised.")]
    [SerializeField] private ParticleSystem disguiseFearEffect;
    [Tooltip("Optional anchor transform for the instantiated disguise effect.")]
    [SerializeField] private Transform disguiseVFXAnchor;
    private ParticleSystem disguiseFearEffectInstance;
    private Coroutine disguiseRoutine;

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
        if (disguiseRoutine != null)
            StopCoroutine(disguiseRoutine);

        disguiseRoutine = StartCoroutine(DisguiseRoutine(duration));
    }

    public void SetDisguised(bool disguised)
    {
        if (disguiseRoutine != null)
        {
            StopCoroutine(disguiseRoutine);
            disguiseRoutine = null;
        }

        ApplyDisguiseState(disguised, true);
    }

    private IEnumerator DisguiseRoutine(float duration)
    {
        ApplyDisguiseState(true, true);
        yield return new WaitForSeconds(duration);
        ApplyDisguiseState(false, true);
        disguiseRoutine = null;
    }

    void Start()
    {
        // link to PlayerInventory and backpack
        backpack = GetComponent<PlayerBackpack>();
        inventory = GetComponent<PlayerInventory>();
        animator = GetComponent<Animator>();
        Debug.Log("Animator Controller at Start: " + animator.runtimeAnimatorController?.name);
        InitializeDisguiseVFX();
        SyncDisguiseVisuals();
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

    private void ApplyDisguiseState(bool disguised, bool logToConsole)
    {
        if (isDisguised == disguised)
        {
            UpdateDisguiseVFX(isDisguised);
            return;
        }

        isDisguised = disguised;

        if (logToConsole)
            Debug.Log(disguised ? "玩家已伪装！" : "伪装效果结束。");

        UpdateDisguiseVFX(isDisguised);
    }

    private void InitializeDisguiseVFX()
    {
        if (disguiseFearEffect == null)
        {
            Debug.LogWarning("Disguise fear effect is not assigned. Please create a particle child and drag it onto PlayerAbilityManager.");
            return;
        }

        if (disguiseFearEffectInstance == null)
        {
            disguiseFearEffectInstance = disguiseFearEffect;

            Transform parent = disguiseVFXAnchor != null ? disguiseVFXAnchor : transform;
            if (disguiseFearEffectInstance.transform.parent != parent)
            {
                disguiseFearEffectInstance.transform.SetParent(parent);
                disguiseFearEffectInstance.transform.localPosition = Vector3.zero;
                disguiseFearEffectInstance.transform.localRotation = Quaternion.identity;
                disguiseFearEffectInstance.transform.localScale = Vector3.one;
            }
        }

        disguiseFearEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        disguiseFearEffectInstance.gameObject.SetActive(false);
    }

    private void UpdateDisguiseVFX(bool disguised)
    {
        if (disguiseFearEffectInstance == null)
        {
            InitializeDisguiseVFX();

            if (disguiseFearEffectInstance == null)
                return;
        }

        if (disguised)
        {
            if (!disguiseFearEffectInstance.gameObject.activeSelf)
                disguiseFearEffectInstance.gameObject.SetActive(true);

            if (!disguiseFearEffectInstance.isPlaying)
                disguiseFearEffectInstance.Play();
        }
        else
        {
            if (disguiseFearEffectInstance.isPlaying)
                disguiseFearEffectInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            disguiseFearEffectInstance.gameObject.SetActive(false);
        }
    }

    private void SyncDisguiseVisuals()
    {
        UpdateDisguiseVFX(isDisguised);
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
