[System.Serializable]
public class PlayerData
{
    public int highestUnlockedLevel; // Represents the highest level unlocked by the player
    public int[] highScores; // Array to store high scores for each level

    public PlayerData(int totalLevels)
    {
        highestUnlockedLevel = 1; // Start with level 1 unlocked
        highScores = new int[totalLevels];
    }

    public int GetHighScore(int level)
    {
        return highScores[level];
    }
}
