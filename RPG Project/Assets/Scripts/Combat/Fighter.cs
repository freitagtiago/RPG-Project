using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;
using RPG.Stats;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        Health target;

        [SerializeField]private float _timeBetweenAttacks = 1.21f;
        float _timeSinceLastAttack = Mathf.Infinity;
        [SerializeField] Transform _rightHandTransform = null;
        [SerializeField] Transform _leftHandTransform = null;
        //[SerializeField] Weapon _currentWeapon = null;
        [SerializeField] Weapon _defaultWeapon = null;
        [SerializeField] string _defaultWeaponName = "Unarmed";
        LazyValue<Weapon> currentWeapon;
         
        private void Awake()
        {
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
            EquipWeapon(_defaultWeapon);
        }

        private Weapon SetDefaultWeapon()
        {
            AttachWeapon(_defaultWeapon);

            return _defaultWeapon;
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetRange();
            if (!isInRange)
            {
                GetComponent<Mover>().MoveTo(target.transform.position,1);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        { 
            transform.LookAt(target.transform); 
            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stoppingAttack");
            //This will trigger the hit event
            GetComponent<Animator>().SetTrigger("isAttacking");
        }

        //Animation Event
        void Hit()
        {
            if(target == null) { return; }
            float damage = GetComponent<BaseStats>().GetStat(Stat.Attack);
            if (currentWeapon.value.HasProjectile() == true)
            {
                currentWeapon.value.LauchProjectile(_rightHandTransform, _leftHandTransform, target, gameObject, damage);
            }else
            {
                target.TakeDamage(this.gameObject, damage);
            }
        }
        //Animation Event
        void Shoot()
        {
            Hit();
        }

        public Health GetTarget()
        {
            return target;
        }

        public bool CanAttack(GameObject combatTarget) 
        {
            if (combatTarget == null) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();

            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("isAttacking");
            GetComponent<Animator>().SetTrigger("stoppingAttack");
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Attack)
            {
                yield return currentWeapon.value.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Attack)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        private void AttachWeapon(Weapon weapon)
        {
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(_rightHandTransform, _leftHandTransform, anim);
        }
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(currentWeapon.value);
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

