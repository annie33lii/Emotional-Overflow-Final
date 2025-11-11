using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : MonoBehaviour
{
    //base stats 
    public string npcName;
    public bool canTransform = true;
    public bool hasDialogue = true;

    protected bool playerInRange = false;

    public virtual void Interact()
    {
        Debug.Log($"{npcName} interacted.");
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log($"{npcName}: Player entered interaction range.");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log($"{npcName}: Player left range.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        } 
    }
}
