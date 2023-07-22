
// GameManager.cs
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    private int levelNumber;
    private int highestScore;

    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private TextMeshProUGUI highestScoreText;

    private IDataManager dataManager;

    [Inject]
    public void Construct(IDataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    public void Initialize(ILevelData levelData)
    {
        levelNumber = levelData.LevelNumber;
        highestScore = levelData.HighestScore;
        levelNumberText.text = "Level " + levelNumber;
        highestScoreText.text = "Highest Score: " + highestScore;

        // Check if the level is playable (unlocked) and update the button appearance accordingly
        bool isPlayable = levelNumber <= dataManager.GetHighestUnlockedLevel();
        button.interactable = isPlayable;
    }

    public void OnPlayButtonClicked()
    {
        // Load the specific level scene using LevelSceneLoader.
        // Pass levelNumber or levelData to the LevelSceneLoader.

        // Update the highest unlocked level if the player successfully completes the current level
        dataManager.SetHighestUnlockedLevel(levelNumber + 1);
    }

    public void SetLocked()
    {
        button.interactable = false;
        //show lock icon
    }

    public class Factory : PlaceholderFactory<LevelButton> { }
}
