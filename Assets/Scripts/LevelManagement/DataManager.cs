using UnityEngine;

public class DataManager : IDataManager
{
    private const string PLAYER_DATA_KEY = "PlayerData";
    private PlayerData playerData;

    private int totalLevelCount ;

    public DataManager(ILevelManager levelManager)
    {
        var levels = levelManager.GetLevels();
        totalLevelCount = levels.Count;
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        string serializedPlayerData = PlayerPrefs.GetString(PLAYER_DATA_KEY, string.Empty);

        if (!string.IsNullOrEmpty(serializedPlayerData))
        {
            playerData = JsonUtility.FromJson<PlayerData>(serializedPlayerData);
        }
        else
        {
            playerData = new PlayerData(totalLevelCount);
            SavePlayerData();
        }
    }

    private void SavePlayerData()
    {
        string serializedPlayerData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA_KEY, serializedPlayerData);
    }

    public int GetHighestUnlockedLevel()
    {
        return playerData.highestUnlockedLevel;
    }

    public void SetHighestUnlockedLevel(int level)
    {
        playerData.highestUnlockedLevel = Mathf.Max(playerData.highestUnlockedLevel, level);
        SavePlayerData();
    }

    public int GetHighScore(int level)
    {
        return playerData.highScores[level];
    }

    // Rest of the code remains unchanged.
}
