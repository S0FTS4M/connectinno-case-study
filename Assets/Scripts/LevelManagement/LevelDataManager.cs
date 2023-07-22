using System.Collections.Generic;
using UnityEngine;

// ILevelDataManager.cs
public interface ILevelDataManager
{
    List<LevelData> GetLevelDataList();
}
// LevelDataManager.cs
public class LevelDataManager : ILevelDataManager
{
    private const string levelDataFilePath = "levelData";

    private List<LevelData> levelDataList;

    public LevelDataManager()
    {
        LoadLevelData();
    }

    private void LoadLevelData()
    {
        TextAsset levelDataFile = Resources.Load<TextAsset>(levelDataFilePath);

        if (levelDataFile != null)
        {
            levelDataList = JsonUtility.FromJson<LevelDataList>(levelDataFile.text).levels;
        }
        else
        {
            Debug.LogError("Level data file not found!");
            levelDataList = new List<LevelData>(); // Create an empty list to avoid null references
        }
    }

    public List<LevelData> GetLevelDataList()
    {
        return levelDataList;
    }
}

