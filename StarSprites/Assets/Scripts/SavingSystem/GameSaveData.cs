using System.Collections.Generic;

[System.Serializable]
public class GameSaveData
{
    public List<int> levelsCompleted = new List<int>();

    [System.Serializable]
    public class LevelProgress
    {
        public int levelIndex;
        public bool allEnemiesKilled;
        public bool bossKilled;
        public bool wardCollected;
    }
    public List<LevelProgress> levelProgress = new List<LevelProgress>();

    public int furnitureCollected = 0;

    [System.Serializable]
    public class FurniturePlacement
    {
        public string furnitureID; // or int, depending on your system
        public float x, y, z; // Position
    }
    public List<FurniturePlacement> furniturePlacements = new List<FurniturePlacement>();

    public List<int> lorePagesCollected = new List<int>(); // Could be string or int depending on your page IDs

    public int playerHealth = 100; // Default health
    public int playerHearts = 3; // Default hearts
    public int playerLevel = 1; // Default level
    public int playerXP = 0; // Default XP
}
