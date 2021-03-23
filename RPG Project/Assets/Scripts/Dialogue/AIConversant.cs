using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Dialogue;
using RPG.Combat;
public class AIConversant : MonoBehaviour, IRaycastable
{
    [SerializeField] Dialogue dialogue = null;
    [SerializeField] string conversantName;

    private void Start()
    {
        CombatTarget combat = GetComponent<CombatTarget>();
        if(combat != null)
        {
            combat.enabled = false;
        }
    }

    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
        if(dialogue == null)
        {
            return false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
        }
        return true;
    }

    public string GetConversantName()
    {
        return conversantName;
    }
}
