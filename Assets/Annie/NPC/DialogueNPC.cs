using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueNPC : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;
    public TextAsset dialogueFile;
    public Sprite[] portraits;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.C;
    public GameObject interactUI;
    [Tooltip("If true, this NPC appears on the left side of the dialogue UI.")]
    public bool isLeftSpeaker = true;

    [Header("Item Reward Settings")]
    public List<GameObject> itemPrefabs = new List<GameObject>(); // multiple prefabs
    public Transform spawnPoint; // optional
    public float spawnSpacing = 0.5f; // spacing between items
    private bool hasGivenItems = false;

    
    protected bool playerInRange = false;
    protected bool isTalking = false;
    protected int currentLine = 0;
    protected string[] dialogueLines;

    protected virtual void Start()
    {
        if (dialogueFile != null)
        {
            dialogueLines = dialogueFile.text.Split('\n');
            for (int i = 0; i < dialogueLines.Length; i++)
                dialogueLines[i] = dialogueLines[i].Trim();
        }

        if (interactUI != null)
            interactUI.SetActive(false);
    }

    protected virtual void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    public virtual void Interact()
    {
        // Handle basic dialogue
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.Log($"{npcName}: 没有对话文件。");
            return;
        }

        if (!isTalking)
        {
            isTalking = true;
            currentLine = 0;
        }

        if (currentLine < dialogueLines.Length)
        {
            Sprite portraitToShow = (portraits.Length > 0)
                ? portraits[currentLine % portraits.Length]
                : null;

            DialogueUIManager.Instance.ShowDialogue(dialogueLines[currentLine], portraitToShow);
            currentLine++;
        }
        else
        {
            DialogueUIManager.Instance.HideDialogue();
            isTalking = false;
            currentLine = 0;
            OnDialogueEnd();
        }
    }

    protected virtual void OnDialogueEnd()
    {
        if (hasGivenItems || itemPrefabs.Count == 0) return;

        Vector3 basePos = spawnPoint ? spawnPoint.position : transform.position + Vector3.down * 0.5f;

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            Vector3 spawnPos = basePos + new Vector3(i * spawnSpacing, 0, 0); // offset horizontally
            Instantiate(itemPrefabs[i], spawnPos, Quaternion.identity);
        }

        hasGivenItems = true;
        Debug.Log($"{npcName} 掉落了 {itemPrefabs.Count} 个物品!");
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactUI != null)
                interactUI.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactUI != null)
                interactUI.SetActive(false);

            DialogueUIManager.Instance.HideDialogue();
        }
    }
}
