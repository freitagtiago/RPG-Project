using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Saving;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.Core;
using RPG.Stats;
using GameDevTV.Utils;
using System;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
       LazyValue<float> health;
       [SerializeField] private bool _isDead = false;
       [SerializeField] private float regeneratePercentage = 75f;
       [SerializeField] private TakeDamageEvent takeDamage;
       [SerializeField] private UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        
        }
        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start()
        {
            health.ForceInit();
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().OnLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public float GetPercentage()
        {
            if (gameObject.tag == "Player")
            {
                Debug.Log("Current Health: " + health.value);
            }
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void  RegenerateHealth()
        {
            float fullHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            float regeneratePoints = fullHealth * (regeneratePercentage / 100);
            float value = health.value + regeneratePoints;
            health.value = Mathf.Max(health.value, regeneratePoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            bool isDead = false;

            Debug.Log(gameObject.name + " took damage of " + damage);
            
            health.value = Mathf.Max(health.value - damage,0);
            takeDamage.Invoke(damage);
            if (health.value == 0 && _isDead == false)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }

        }

        public void Heal(float healthToRestore)
        {
            health.value = Mathf.Min(health.value + healthToRestore, GetMaxHealthPoints());
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die()
        {
            GetComponent<Animator>().SetTrigger("isAlive");
            _isDead = true;
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health.value;
        }
        public void RestoreState(object state)
        {
            health.value = (float)state;
            if (health.value == 0 && _isDead == false)
            {
                Die();
            }
        }
    }
}