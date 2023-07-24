using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Transform levelsGrid;
    [SerializeField] private GameObject playButtonScreenGO;
    [SerializeField] private GameObject playButtonToAnimate;
    [SerializeField] private Button playButton;

    [SerializeField] private CanvasGroup canvasGroup;

    private ILevelManager _levelManager;
    private IDataManager _dataManager;

    private LevelButton.Factory _levelButtonFactory;

    private List<LevelButton> _levelButtons;

    private Tweener _tweener;

    [Inject]
    public void Construct(IDataManager dataManager, ILevelManager levelManager, LevelButton.Factory levelButtonFactory)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
        this._levelButtonFactory = levelButtonFactory;

        _levelButtons = new List<LevelButton>();

        int highestUnlockedLevel = _dataManager.GetHighestUnlockedLevel();
        
        foreach (LevelData levelData in _levelManager.GetLevels())
        {
            LevelButton levelButton = _levelButtonFactory.Create();
            levelButton.transform.SetParent(levelsGrid);
            levelButton.Initialize(levelData);
            _levelButtons.Add(levelButton);

            // If the level is locked, disable the play button
            if (levelData.levelNumber > highestUnlockedLevel)
            {
                levelButton.SetLocked();
            }
        }

        playButton.onClick.AddListener(() => ShowUI());

        levelManager.LevelLoaded += OnLevelLoaded;

        _tweener = playButtonToAnimate.transform.DOScale(Vector3.one * 0.8f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowUI(Action onShowUIComplete = null)
    {
        canvasGroup.alpha = 1;
        container.SetActive(true);
        container.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f).OnComplete(()=> onShowUIComplete?.Invoke());
        playButtonScreenGO.SetActive(false);

        if(_tweener != null)
            _tweener.Kill();
    }

    public void HideUI()
    {
        canvasGroup.DOFade(0f, 0.5f).OnComplete(() => container.SetActive(false));
    }

    private void OnLevelLoaded(LevelData level)
    {
        HideUI();
    }

    public void SetLevelPlayable(int levelNumber)
    {
        _levelButtons[levelNumber - 1].SetPlayable();
    }

    public void UpdateHighScore(int levelNumber)
    {
        _levelButtons[levelNumber - 1].SetHighScore();
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject levelButtonPrefab;
    }
}
