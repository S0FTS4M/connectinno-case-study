using System;
using System.Collections.Generic;

public interface ILevelManager
{
    event LevelLoadedHandler LevelLoaded;
    event PlayerMadeAMoveHandler PlayerMadeMove;
    event Action<LevelData> LevelFailed;
    event Action<LevelData,bool> LevelCompleted;
    List<LevelData> GetLevels();

    void LoadLevel(int index);
    void PlayerMadeAMove();
    void AddScore(int score);
    void GoalsAchived();
    
}

public delegate void LevelLoadedHandler(LevelData level);
public delegate void PlayerMadeAMoveHandler(int remainingMoves);