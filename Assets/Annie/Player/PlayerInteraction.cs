using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
    {
        Collider2D npcCollider = Physics2D.OverlapCircle(transform.position, 1.5f, LayerMask.GetMask("NPC"));
        if (npcCollider != null)
        {
            ThiefNPC thief = npcCollider.GetComponent<ThiefNPC>();
            if (thief != null)
            {
                thief.Interact();
            }
        }
    }
    }
}
