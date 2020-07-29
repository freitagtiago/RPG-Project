using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System.Runtime.Serialization.Formatters.Binary;
using RPG.Core;
using RPG.Stats;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
       private float _health = -1f;
       [SerializeField] private bool _isDead = false;
       [SerializeField] private float regeneratePercentage = 75f;

        private void Start()
        {
            GetComponent<BaseStats>().OnLevelUp += RegenerateHealth;
            if(_health < 0)
            {
                _health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public float GetPercentage()
        {
            if (gameObject.tag == "Player")
            {
                Debug.Log("Current Health: " + _health);
            }
            return 100 * (_health / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public float GetHealthPoints()
        {
            return _health;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void  RegenerateHealth()
        {
            float regeneratePoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            _health += Mathf.Max(_health, regeneratePoints);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            bool isDead = false;

            Debug.Log(gameObject.name + " took damage of " + damage);

            _health = Mathf.Max(_health - damage,0);
            if (_health == 0 && _isDead == false)
            {
                Die();
                AwardExperience(instigator);
            }
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
            return _health;
        }
        public void RestoreState(object state)
        {
            _health = (float)state;
            if (_health == 0 && _isDead == false)
            {
                Die();
            }
        }
    }
}