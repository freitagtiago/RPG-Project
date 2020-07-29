using RPG.Resources;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                Debug.Log(GetLevel());
            }
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentXp = experience.GetExperience();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for(int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if(xpToLevelUp > currentXp)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}

