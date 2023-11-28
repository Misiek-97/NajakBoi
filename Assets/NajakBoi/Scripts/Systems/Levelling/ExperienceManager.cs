using NajakBoi.Scripts.Session;
using UnityEngine;

namespace NajakBoi.Scripts.Systems.Levelling
{
    public class ExperienceManager : MonoBehaviour
    {
        // Assuming these are public for inspection, you might want to encapsulate them
        private static int CurrentLevel
        {
            get => SessionManager.PlayerData.Stats.Level;
            set => SessionManager.PlayerData.Stats.Level = value;
        }

        private static int CurrentExperience
        {
            get => SessionManager.PlayerData.Stats.Experience;
            set => SessionManager.PlayerData.Stats.Experience = value;
        }

        private static int _experienceToNextLevel = 100;


        public static int GainExperience(int experienceGained)
        {
            CurrentExperience += experienceGained;

            while (CurrentExperience >= _experienceToNextLevel)
            {
                return LevelUp();
            }

            return 0;
        }

        private static int LevelUp()
        {
            CurrentLevel++;
            CurrentExperience -= _experienceToNextLevel;
            Debug.Log($"Level Up! Current Level: {CurrentLevel}, Current Saved Level: {SessionManager.PlayerData.Stats.Level} {SessionManager.PlayerData.Stats.Experience}");
            SessionManager.PlayerData.SavePlayerData();
            return CalculateExperienceToNextLevel(); // Calculate new experience requirement for the next level
        }

        public static int CalculateExperienceToNextLevel()
        {
            // Example: Linear progression, you can modify this formula as needed
            _experienceToNextLevel = Mathf.FloorToInt(100 * Mathf.Pow(1.2f, CurrentLevel));
            return _experienceToNextLevel;
        }

    }
}