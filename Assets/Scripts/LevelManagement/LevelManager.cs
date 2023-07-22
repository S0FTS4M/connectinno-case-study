// LevelManager.cs
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ILevelManager
{
    private ILevelDataManager levelDataManager;

    public event LevelLoadedHandler LevelLoaded;

    public LevelManager(ILevelDataManager levelDataManager)
    {
        this.levelDataManager = levelDataManager;
    }

    public List<LevelData> GetLevels()
    {
        return levelDataManager.GetLevelDataList();
    }

    public void LoadLevel(int index)
    {
        LevelLoaded?.Invoke(GetLevels()[index]);
    }
}
