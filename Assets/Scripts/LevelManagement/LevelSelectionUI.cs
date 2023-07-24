using DG.Tweening;
using UnityEngine;
using Zenject;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Transform levelsGrid;
    [SerializeField] private GameObject playButton;

    [SerializeField] private CanvasGroup canvasGroup;

    private ILevelManager _levelManager;
    private IDataManager _dataManager;

    private LevelButton.Factory _levelButtonFactory;

    private Tweener _tweener;

    [Inject]
    public void Construct(IDataManager dataManager, ILevelManager levelManager, LevelButton.Factory levelButtonFactory)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
        this._levelButtonFactory = levelButtonFactory;

        int highestUnlockedLevel = _dataManager.GetHighestUnlockedLevel();
        
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

        _tweener = playButton.transform.DOScale(Vector3.one * 0.8f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ShowUI()
    {
        container.SetActive(true);
        container.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
        playButton.SetActive(false);
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


    [System.Serializable]
    public class Settings
    {
        public GameObject levelButtonPrefab;
    }
}
