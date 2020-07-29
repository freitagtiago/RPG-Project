using RPG.Combat;
using RPG.Resources;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter target;
        Health enemyHealth;

        private void Awake()
        {
            target = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            enemyHealth = target.GetTarget();
            if(enemyHealth == null)
            {
                GetComponent<Text>().text = "N/A";
            }
            else
            {
                if (!enemyHealth.IsDead())
                {
                    GetComponent<Text>().text = System.String.Format("{0:0}%", enemyHealth.GetPercentage());
                } 
            }
        }
    }
}

