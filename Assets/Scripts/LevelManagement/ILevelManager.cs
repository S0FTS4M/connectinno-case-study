using System.Collections.Generic;

public interface ILevelManager
{
    event LevelLoadedHandler LevelLoaded;
    event PlayerMadeAMoveHandler PlayerMadeMove;

    List<LevelData> GetLevels();

    void LoadLevel(int index);
    void PlayerMadeAMove();
}

public delegate void LevelLoadedHandler(LevelData level);
public delegate void PlayerMadeAMoveHandler(int remainingMoves);