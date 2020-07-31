using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control
{
    
    public class PlayerController : MonoBehaviour
    {
        Health _health;

        // Start is called before the first frame update
        void Awake()
        {
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_health.IsDead()) { return; }
            if (InteractWithCombat()) return;
            if (!InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit [] hits =   Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true; 
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    GetComponent<Mover>().StartMoveAction(hit.point,1f);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

