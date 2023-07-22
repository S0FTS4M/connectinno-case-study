using System.Collections.Generic;

[System.Serializable]
public class TargetObjective
{
    public int count;
    public string name;
}

// LevelData.cs
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

[System.Serializable]
public class LevelDataList
{
    public List<LevelData> levels;
}