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
    [SerializeField] private Image _playIcon;
    [SerializeField] private GameObject _highScoreGroup;

    private IDataManager _dataManager;

    private ILevelManager _levelManager;

    private Settings _settings;

    private PlayerGoalsManager _playerGoalsManager;

    [Inject]
    public void Construct(IDataManager dataManager,ILevelManager levelManager, Settings settings, PlayerGoalsManager playerGoalsManager)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
        this._settings = settings;
        this._playerGoalsManager = playerGoalsManager;
    }

    public void Initialize(LevelData levelData)
    {
        _levelNumber = levelData.levelNumber;
        _highestScore = levelData.highScore;
        _levelNumberText.text = "Level " + _levelNumber;
        _highestScoreText.text = _highestScore.ToString();

        bool isPlayable = _levelNumber <= _dataManager.GetHighestUnlockedLevel();

        var displayHighScore = _dataManager.GetHighScore(_levelNumber) > 0;
        _highScoreGroup.gameObject.SetActive(displayHighScore);
        

        // Check if the level is playable (unlocked) and update the button appearance accordingly
        _button.interactable = isPlayable;

        _playIcon.color = isPlayable ? _settings.greenColor : _settings.grayColor;

        // Add the button click listener
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnPlayButtonClicked);
    }

    public void OnPlayButtonClicked()
    {
        // Load the specific level scene using LevelSceneLoader.
        // Pass levelNumber or levelData to the LevelSceneLoader.

        _playerGoalsManager.InitializePlayerGoalsForLevel(_levelNumber);
        _levelManager.LoadLevel(_levelNumber);
    }

    public void SetLocked()
    {
        _button.interactable = false;
        _playIcon.color = _settings.grayColor;
        _highScoreGroup.SetActive(false);
        
    }

    public void SetPlayable()
    {
        _button.interactable = true;
        _playIcon.color = _settings.greenColor;
        _highScoreGroup.SetActive(true);
    }

    public class Factory : PlaceholderFactory<LevelButton> { }

    [System.Serializable]
    public class Settings
    {
        public Color greenColor;
        public Color grayColor;
    }
}
