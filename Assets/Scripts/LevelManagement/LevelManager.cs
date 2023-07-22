using System.Collections.Generic;

public class LevelManager : ILevelManager
{
    private List<ILevelData> levels;

    public LevelManager(List<ILevelData> levels)
    {
        this.levels = levels;
    }

    // Implement methods to retrieve levels, update highest scores, etc.

    // In the real implementation, you would have methods to fetch levels from your level data source.
    public List<ILevelData> GetLevels()
    {
        return levels;
    }
}
