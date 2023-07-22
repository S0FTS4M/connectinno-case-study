
// GameManager.cs
using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LevelsUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
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

        int highestUnlockedLevel = _dataManager.GetHighestUnlockedLevel();
        
        if(_levelManager == null)
            Debug.Log("whaaaat");
        foreach (LevelData levelData in _levelManager.GetLevels())
        {
            LevelButton levelButton = _levelButtonFactory.Create();
            levelButton.transform.SetParent(levelsGrid);
            levelButton.Initialize(levelData);

            // If the level is locked, disable the play button
            if (levelData.levelNumber > highestUnlockedLevel)
            {
                levelButton.SetLocked();
            }
        }

        levelManager.LevelLoaded += OnLevelLoaded;
    }

    public void ShowUI()
    {
        container.SetActive(true);
        container.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }

    public void HideUI()
    {
        container.transform.DOPunchScale(Vector3.one * -0.1f, 0.2f).OnComplete(() => container.SetActive(false));
    }

    private void OnLevelLoaded(LevelData level)
    {
        HideUI();
    }


    [System.Serializable]
    public class Settings
    {
        public GameObject levelButtonPrefab;
    }
}
