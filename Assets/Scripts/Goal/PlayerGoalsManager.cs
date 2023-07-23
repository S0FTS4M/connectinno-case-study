// PlayerGoalData.cs
// PlayerGoalsManager.cs
using System.Collections.Generic;

public class PlayerGoalsManager
{
    private List<LevelData> levelDataList;
    private Dictionary<int, List<PlayerGoalData>> playerGoalsDictionary;

    public PlayerGoalsManager(ILevelDataManager levelDataManager)
    {
        levelDataList = levelDataManager.GetLevelDataList();
        playerGoalsDictionary = new Dictionary<int, List<PlayerGoalData>>();

        InitializePlayerGoals();
    }

    private void InitializePlayerGoals()
    {
        foreach (var levelData in levelDataList)
        {
            int levelNumber = levelData.levelNumber;
            List<PlayerGoalData> playerGoals = new List<PlayerGoalData>();

            foreach (var targetObjective in levelData.targetObjectives)
            {
                playerGoals.Add(new PlayerGoalData
                {
                    itemName = targetObjective.name,
                    targetCount = targetObjective.count,
                });
            }

            playerGoalsDictionary.Add(levelNumber, playerGoals);
        }
    }

    public List<PlayerGoalData> GetPlayerGoalsForLevel(int levelNumber)
    {
        if (playerGoalsDictionary.TryGetValue(levelNumber, out var playerGoals))
        {
            return playerGoals;
        }

        return null; // Level number not found
    }
}
