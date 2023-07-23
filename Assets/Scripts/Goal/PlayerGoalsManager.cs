// PlayerGoalData.cs
// PlayerGoalsManager.cs
using System.Collections.Generic;

public delegate void PlayerGoalUpdated(PlayerGoalData playerGoalData);

public class PlayerGoalsManager
{
    private List<LevelData> levelDataList;
    List<PlayerGoalData> playerGoals;

    public event PlayerGoalUpdated PlayerGoalUpdated;

    public PlayerGoalsManager(ILevelDataManager levelDataManager)
    {
        levelDataList = levelDataManager.GetLevelDataList();
    }

    public void InitializePlayerGoalsForLevel(int levelNumber)
    {
        playerGoals = new List<PlayerGoalData>();
        var levelData = levelDataList[levelNumber - 1];

        foreach (var targetObjective in levelData.targetObjectives)
        {
            playerGoals.Add(new PlayerGoalData
            {
                itemName = targetObjective.name,
                targetCount = targetObjective.count,
            });
        }
    }

    public List<PlayerGoalData> GetPlayerGoals()
    {
        return playerGoals;
    }

    public void UpdatePlayerGoal(string itemName, int amount)
    {
        var playerGoal = playerGoals.Find(x => x.itemName == itemName);
        if (playerGoal == null)
        {
            return;
        }
        
        playerGoal.targetCount -= amount;
        PlayerGoalUpdated?.Invoke(playerGoal);
    }
}
