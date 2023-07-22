using System.Collections.Generic;

public interface ILevelManager
{
    event LevelLoadedHandler LevelLoaded;
    List<LevelData> GetLevels();

    void LoadLevel(int index);
}

public delegate void LevelLoadedHandler(LevelData level);