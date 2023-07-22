using UnityEngine;

public class DataManager : IDataManager
{
    private const string PLAYER_DATA_KEY = "PlayerData";
    private PlayerData playerData;

    public DataManager()
    {
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
            playerData = new PlayerData(10); //FIXME: hardcoded value
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

    // Rest of the code remains unchanged.
}
