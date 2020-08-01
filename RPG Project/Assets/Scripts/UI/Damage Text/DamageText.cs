using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText = null;
        public void SetValue(float value)
        {
            damageText.text = System.String.Format("{0:0}",value);
        }
    }
}
