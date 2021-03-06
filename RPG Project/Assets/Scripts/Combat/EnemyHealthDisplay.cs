﻿using RPG.Combat;
using RPG.Attributes;
using RPG.Stats;
using System;
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
                 GetComponent<Text>().text = String.Format("{0:0}/{1:0}", enemyHealth.GetHealthPoints(), enemyHealth.GetMaxHealthPoints());
            }
        }
    }
}

