using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
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
        [SerializeField] WeaponConfig _defaultWeapon = null;
        [SerializeField] string _defaultWeaponName = "Unarmed";
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;
         
        private void Awake()
        {
            currentWeaponConfig = _defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
    
        }

        private Weapon SetDefaultWeapon()
        {
            return AttachWeapon(_defaultWeapon);
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

            bool isInRange = Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.GetRange();
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

            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }

            if (currentWeaponConfig.HasProjectile() == true)
            {
                currentWeaponConfig.LauchProjectile(_rightHandTransform, _leftHandTransform, target, gameObject, damage);
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
                yield return currentWeaponConfig.GetDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Attack)
            {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator anim = GetComponent<Animator>();
            return weapon.Spawn(_rightHandTransform, _leftHandTransform, anim);
        }
        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

