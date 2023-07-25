// PlayerGoalData.cs
// PlayerGoalsManager.cs
using System.Collections.Generic;

public delegate void PlayerGoalUpdated(PlayerGoalData playerGoalData);

public class PlayerGoalsManager
{
    private List<LevelData> levelDataList;
    
    private List<PlayerGoalData> playerGoals;

    private ILevelManager _levelManager;

    public event PlayerGoalUpdated PlayerGoalUpdated;

        
    public bool IsGoalsAchived => playerGoals.Count == 0;

    public PlayerGoalsManager(ILevelDataManager levelDataManager, ILevelManager levelManager)
    {
        _levelManager = levelManager;
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

        if (playerGoal.targetCount <= 0)
        {
            playerGoals.Remove(playerGoal);
        }

        PlayerGoalUpdated?.Invoke(playerGoal);

        if (playerGoals.Count == 0)
        {
            _levelManager.GoalsAchived();
        }
    }
}
