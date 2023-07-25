using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class LevelCompletionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject highScoreUI;

    [SerializeField]
    private GameObject failInfoUI;

    [SerializeField]
    private TextMeshProUGUI failInfo;

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

        _levelManager.NoAvailableSetFound += OnNoAvailableSetFound;
        levelManager.PlayerOutOfMoves += OnPlayerOutOfMoves;
        levelManager.LevelCompleted += OnLevelCompleted;
        levelManager.LevelFailed += OnLevelFailed;
    }

    private void OnLevelCompleted(LevelData levelData, bool newHighScore)
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(_levelUI.HideUI);
        seq.AppendInterval(.5f);
        Action levelSelectionUIAction = null;
        if (newHighScore)
        {
            seq.AppendCallback(() => highScoreUI.SetActive(true));
            seq.AppendCallback(() => levelCompleteParticleSystem.Play());
            seq.Append(highScoreUI.transform.DOScale(1.1f, .5f).SetLoops(4, LoopType.Yoyo));
            seq.AppendCallback(() => highScoreUI.SetActive(false));
            levelSelectionUIAction = () => _levelSelectionUI.SetLevelPlayable(levelData.levelNumber + 1);
            _dataManager.SetHighestUnlockedLevel(levelData.levelNumber + 1);
        }

        seq.AppendCallback(() =>
        _levelSelectionUI.ShowUI(
            () => levelSelectionUIAction?.Invoke()
        ));

        _levelSelectionUI.UpdateHighScore(levelData.levelNumber);
    }

    private void OnPlayerOutOfMoves()
    {
        failInfo.SetText("Out Of Moves");
        failInfoUI.SetActive(true);
        failInfoUI.transform.DOScale(1.1f, .5f).SetLoops(4, LoopType.Yoyo).OnComplete(() => failInfoUI.SetActive(false));
    }

    private void OnNoAvailableSetFound()
    {
        failInfo.SetText("No Available Set");
        failInfoUI.SetActive(true);
        failInfoUI.transform.DOScale(1.1f, .5f).SetLoops(4, LoopType.Yoyo).OnComplete(() => failInfoUI.SetActive(false));
    }

    private void OnLevelFailed(LevelData data)
    {
        _levelUI.HideUI();
        _levelSelectionUI.ShowUI();
    }

}
