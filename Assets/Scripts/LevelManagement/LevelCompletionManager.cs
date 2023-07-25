using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class LevelCompletionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject highScoreUI;

    private ILevelManager _levelManager;
    private LevelUI _levelUI;
    private LevelSelectionUI _levelSelectionUI;
    private IDataManager _dataManager;

    public ParticleSystem levelCompleteParticleSystem;

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
        var seq = DOTween.Sequence();
        seq.AppendCallback(_levelUI.HideUI);
        seq.AppendInterval(.5f);
        seq.AppendCallback(() => highScoreUI.SetActive(true));
        seq.AppendCallback(() => levelCompleteParticleSystem.Play());
        seq.Append(highScoreUI.transform.DOScale(1.1f, .5f).SetLoops(4, LoopType.Yoyo));
        Action levelSelectionUIAction = null;
        if (newHighScore)
        {
            levelSelectionUIAction = () => _levelSelectionUI.SetLevelPlayable(levelData.levelNumber + 1);
            _dataManager.SetHighestUnlockedLevel(levelData.levelNumber + 1);
        }

        seq.AppendCallback(() => highScoreUI.SetActive(false));
        seq.AppendCallback(() =>
        _levelSelectionUI.ShowUI(
            () => levelSelectionUIAction?.Invoke()
        ));

        _levelSelectionUI.UpdateHighScore(levelData.levelNumber);
    }
}
