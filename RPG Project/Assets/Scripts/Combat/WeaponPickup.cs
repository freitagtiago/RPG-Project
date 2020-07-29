using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon _weapon;
        [SerializeField] float _respawnTime = 5f;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                StartCoroutine(HideForSeconds(_respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            HideShowPickup(false);
            yield return new WaitForSeconds(seconds);
            HideShowPickup(true);
        }

        private void HideShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}

