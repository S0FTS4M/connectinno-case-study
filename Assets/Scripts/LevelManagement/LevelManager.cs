using System;
using System.Collections.Generic;
using DG.Tweening;

public class LevelManager : ILevelManager
{
    private ILevelDataManager levelDataManager;

    public event LevelLoadedHandler LevelLoaded;

    public event PlayerMadeAMoveHandler PlayerMadeMove;

    public event Action<LevelData> LevelFailed;

    public event Action<LevelData,bool> LevelCompleted;

    public event Action PlayerOutOfMoves;

    public event Action NoAvailableSetFound;

    private int _remainingMoves;

    private bool _isHighScoreSet;

    private int _score;

    private LevelData _currentLevelData;

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
            _currentLevelData.highScore = 0;
            var seq = DOTween.Sequence();
            seq.AppendCallback(() => PlayerOutOfMoves?.Invoke());
            seq.AppendInterval(2f);
            seq.AppendCallback(()=> LevelFailed?.Invoke(_currentLevelData));
            return;
        }

        PlayerMadeMove?.Invoke(_remainingMoves);
    }

    public void AddScore(int score)
    {
        _score += score;

        if(_score > _currentLevelData.highScore)
        {
            _currentLevelData.highScore = _score;
            _isHighScoreSet = true;
        }
    }

    public void NoAvailableSet()
    {
        _currentLevelData.highScore = 0;
        var seq =DOTween.Sequence();
        seq.AppendCallback(()=> NoAvailableSetFound?.Invoke());
        seq.AppendInterval(2f);
        seq.AppendCallback(()=> LevelFailed?.Invoke(_currentLevelData));
    }
    

    public void GoalsAchived()
    {
        LevelCompleted?.Invoke(_currentLevelData, _isHighScoreSet);
    }

    public void LoadLevel(int levelNumber)
    {
        var levelData = GetLevels()[levelNumber - 1];

        _remainingMoves = levelData.totalMoves;
        _score = 0;
        _isHighScoreSet = false;

        _currentLevelData = levelData;

        LevelLoaded?.Invoke(levelData);
    }
}
