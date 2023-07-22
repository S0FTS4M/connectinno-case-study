
// GameManager.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    private int _levelNumber;
    private int _highestScore;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _highestScoreText;
    [SerializeField] private GameObject _lockIconGO;
    [SerializeField] private GameObject _playIconGO;

    private IDataManager _dataManager;

    private ILevelManager _levelManager;

    [Inject]
    public void Construct(IDataManager dataManager,ILevelManager levelManager)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
    }

    public void Initialize(LevelData levelData)
    {
        _levelNumber = levelData.levelNumber;
        _highestScore = levelData.highScore;
        _levelNumberText.text = "Level " + _levelNumber;
        _highestScoreText.text = _highestScore.ToString();

        bool isPlayable = _levelNumber <= _dataManager.GetHighestUnlockedLevel();
        _levelNumberText.gameObject.SetActive(isPlayable);
        _highestScoreText.gameObject.SetActive(isPlayable);

        // Check if the level is playable (unlocked) and update the button appearance accordingly
        _button.interactable = isPlayable;

        _playIconGO.SetActive(isPlayable);
        _lockIconGO.SetActive(!isPlayable);

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnPlayButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        // Load the specific level scene using LevelSceneLoader.
        // Pass levelNumber or levelData to the LevelSceneLoader.

        _levelManager.LoadLevel(_levelNumber);
        // Update the highest unlocked level if the player successfully completes the current level
        _dataManager.SetHighestUnlockedLevel(_levelNumber + 1);
    }

    public void SetLocked()
    {
        _button.interactable = false;
        //show lock icon
        _playIconGO.SetActive(false);
        _lockIconGO.SetActive(true);
    }

    public void SetPlayable()
    {
        _button.interactable = true;
        //show play icon
        _playIconGO.SetActive(true);
        _lockIconGO.SetActive(false);
    }

    public class Factory : PlaceholderFactory<LevelButton> { }
}
