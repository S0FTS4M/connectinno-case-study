using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelCompletionManager : MonoBehaviour
{
    private ILevelManager _levelManager;
    private LevelUI _levelUI;
    private LevelSelectionUI _levelSelectionUI;
    private IDataManager _dataManager;

    [Inject]
    private void Construct(ILevelManager levelManager, LevelUI levelUI, LevelSelectionUI levelSelectionUI, IDataManager dataManager)
    {
        _levelManager = levelManager;
        _levelUI = levelUI;
        _levelSelectionUI = levelSelectionUI;
        _dataManager = dataManager;

        levelManager.LevelCompleted += OnLevelCompleted;
    }

    private void OnLevelCompleted(LevelData levelData, bool newHighScore)
    {
        Action levelSelectionUIAction = null;
        if (newHighScore)
        {
            levelSelectionUIAction = () => _levelSelectionUI.SetLevelPlayable(levelData.levelNumber + 1);
            _dataManager.SetHighestUnlockedLevel(levelData.levelNumber + 1);
        }
        _levelUI.HideUI();
        _levelSelectionUI.ShowUI(
            () => levelSelectionUIAction?.Invoke()
        );
        
        _levelSelectionUI.UpdateHighScore(levelData.levelNumber);
    }
}
