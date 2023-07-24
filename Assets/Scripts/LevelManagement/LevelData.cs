using System.Collections.Generic;
[System.Serializable]
public class LevelData
{
    public int highScore;
    public int levelNumber;
    public int totalMoves;
    public int row;
    public int column;
    public List<TargetObjective> targetObjectives;
}
