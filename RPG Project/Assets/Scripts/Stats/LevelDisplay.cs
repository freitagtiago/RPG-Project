using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        int level;
        BaseStats playerStats;

        private void Awake()
        {
            playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        void Update()
        {
            GetComponent<Text>().text = System.String.Format("{0:0}", playerStats.GetLevel());
        }
    }
}

