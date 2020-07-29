
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController _animatorOverride = null;
        [SerializeField] GameObject _equippedPrefab = null;
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _weaponDamage = 2f;
        [SerializeField] private float _maxLifeTime = 10f;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile projectile = null;
        const string weaponName = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator anim)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (_equippedPrefab != null)
            {
                GameObject weapon = Instantiate(_equippedPrefab, GetTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }

            var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride != null)
            {
                anim.runtimeAnimatorController = _animatorOverride;
            }
            else if (overrideController != null)
            {
                    anim.runtimeAnimatorController = overrideController;
            }
           
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);

        }

        private Transform GetTransform(Transform rightHand, Transform _leftHand)
        {
            Transform handTransform;
            if (_isRightHanded == true) handTransform = rightHand;
            else handTransform = _leftHand;
            return handTransform;
        }

        public void LauchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target,instigator, _weaponDamage);
            Destroy(projectileInstance.gameObject, _maxLifeTime);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetDamage()
        {
            return _weaponDamage;
        }

        public float GetRange()
        {
            return _weaponRange;
        }
    }
}

