
// GameManager.cs
using UnityEngine;
using Zenject;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] private Transform levelsGrid;

    private ILevelManager _levelManager;
    private IDataManager _dataManager;

    private LevelButton.Factory _levelButtonFactory;

    [Inject]
    public void Construct(IDataManager dataManager, ILevelManager levelManager, LevelButton.Factory levelButtonFactory)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
        this._levelButtonFactory = levelButtonFactory;
    }

    private void Start()
    {
        int highestUnlockedLevel = _dataManager.GetHighestUnlockedLevel();

        foreach (ILevelData levelData in _levelManager.GetLevels())
        {
            LevelButton levelButton = _levelButtonFactory.Create();
            levelButton.transform.SetParent(levelsGrid);
            levelButton.Initialize(levelData);

            // If the level is locked, disable the play button
            if (levelData.LevelNumber > highestUnlockedLevel)
            {
                levelButton.SetLocked();
            }
        }
    }
}
