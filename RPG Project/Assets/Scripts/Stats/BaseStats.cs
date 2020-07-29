using RPG.Resources;
using System;
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
        [SerializeField] GameObject levelUpParticleEffect = null;
        Experience experience;
        int currentLevel = 0;
        public Action OnLevelUp;

        private void Start()
        {
            currentLevel = CalculateLevel();
            experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                LevelUpEffect();
                OnLevelUp();
                currentLevel = newLevel;
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startingLevel);
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }
        public int CalculateLevel()
        {

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

