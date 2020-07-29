using RPG.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float _experiencePoints = 0;

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
        }

        public float GetExperience()
        {
            return _experiencePoints;
        }
        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}

