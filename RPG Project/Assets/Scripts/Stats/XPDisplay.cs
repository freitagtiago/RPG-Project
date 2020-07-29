using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using RPG.Stats;

namespace Stats
{
    public class XPDisplay : MonoBehaviour
    {
        Experience xp;
        float xpValue;
    

        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = System.String.Format("{0:0}", xp.GetExperience());
        }
    }
}
