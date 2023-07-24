// LevelManager.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ILevelManager
{
    private ILevelDataManager levelDataManager;

    public event LevelLoadedHandler LevelLoaded;
    
    public event PlayerMadeAMoveHandler PlayerMadeMove;

    private int _remainingMoves;

    public LevelManager(ILevelDataManager levelDataManager)
    {
        this.levelDataManager = levelDataManager;
    }

    public List<LevelData> GetLevels()
    {
        return levelDataManager.GetLevelDataList();
    }

    public void PlayerMadeAMove()
    {
        _remainingMoves--;
        PlayerMadeMove?.Invoke(_remainingMoves);
    }

    public void LoadLevel(int levelNumber)
    {
        var levelData = GetLevels()[levelNumber - 1];

        _remainingMoves = levelData.totalMoves;

        LevelLoaded?.Invoke(levelData);
    }
}
