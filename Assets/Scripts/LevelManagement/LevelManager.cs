using System;
using System.Collections.Generic;

public class LevelManager : ILevelManager
{
    private ILevelDataManager levelDataManager;

    public event LevelLoadedHandler LevelLoaded;

    public event PlayerMadeAMoveHandler PlayerMadeMove;

    public event Action LevelFailed;

    public event Action<bool> LevelCompleted;

    private int _remainingMoves;

    private int _highScore;

    private bool _isHighScoreSet;

    private int _score;

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
            LevelFailed?.Invoke();
            return;
        }

        PlayerMadeMove?.Invoke(_remainingMoves);
    }

    public void AddScore(int score)
    {
        _score += score;

        if(_score > _highScore)
        {
            _highScore = _score;
            _isHighScoreSet = true;
        }
    }
    

    public void GoalsAchived()
    {
        LevelCompleted?.Invoke(_isHighScoreSet);
    }

    public void LoadLevel(int levelNumber)
    {
        var levelData = GetLevels()[levelNumber - 1];

        _remainingMoves = levelData.totalMoves;
        _score = 0;
        _isHighScoreSet = false;
        _highScore = levelData.highScore;

        LevelLoaded?.Invoke(levelData);
    }
}
