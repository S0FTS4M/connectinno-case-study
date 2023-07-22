using System.Collections.Generic;

public class LevelManager : ILevelManager
{
    private List<ILevelData> _levels;
    private IDataManager _playerData;

    public LevelManager(List<ILevelData> levels, IDataManager playerData)
    {
        this._levels = levels;
        this._playerData = playerData;

        for(int i = 0; i < _levels.Count; i++)
        {
            _levels[i].HighestScore = _playerData.GetHighScore(i);
        }
    }
    

    // Implement methods to retrieve levels, update highest scores, etc.

    // In the real implementation, you would have methods to fetch levels from your level data source.
    public List<ILevelData> GetLevels()
    {
        return _levels;
    }
}
