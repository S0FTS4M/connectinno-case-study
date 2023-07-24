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

    public event Action PlayerFailed;

    private int _remainingMoves;

    private int _score;

    private int _highestScore;

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
        if (_remainingMoves <= 0)
        {
            PlayerFailed?.Invoke();
            return;
        }

        PlayerMadeMove?.Invoke(_remainingMoves);
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public void LoadLevel(int levelNumber)
    {
        var levelData = GetLevels()[levelNumber - 1];

        _remainingMoves = levelData.totalMoves;
        _highestScore = levelData.highScore;
        _score = 0;

        LevelLoaded?.Invoke(levelData);
    }
}
