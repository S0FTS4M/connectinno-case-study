using UnityEngine;
using TMPro;
using Zenject;
using System.Collections.Generic;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moveCountText;
    [SerializeField] private GameObject _container;
    [SerializeField] private Transform _goalContainer;

    private LevelData currentLevelData;

    private PlayerGoalsManager _playerGoalsManager;

    private PlayerGoal.PlayerGoalPool _playerGoalPool;

    private Dictionary<string, PlayerGoal> _goals = new Dictionary<string, PlayerGoal>();

    [Inject]
    public void Construct(PlayerGoalsManager goalsManager, PlayerGoal.PlayerGoalPool playerGoalPool, ILevelManager levelManager)
    {
        _playerGoalsManager = goalsManager;
        _playerGoalPool = playerGoalPool;
        levelManager.LevelLoaded += OnLevelLoaded;

        _playerGoalsManager.PlayerGoalUpdated += OnPlayerGoalUpdated;
        levelManager.PlayerMadeMove += OnPlayerMove;
    }

    public void Initialize(LevelData level)
    {
        currentLevelData = level;
        SetupUI();
    }

    private void SetupUI()
    {
        if (_playerGoalsManager != null)
        {
            var playerGoals = _playerGoalsManager.GetPlayerGoals();
            if (playerGoals != null)
            {
                _moveCountText.text = currentLevelData.totalMoves.ToString();

                for (int i = 0; i < playerGoals.Count; i++)
                {
                    var playerGoal = _playerGoalPool.Spawn();
                    playerGoal.transform.SetParent(_goalContainer);
                    playerGoal.SetGoal(playerGoals[i]);
                    _goals.Add(playerGoals[i].itemName, playerGoal);
                }
            }
        }
    }

    public void ShowUI()
    {
        _container.SetActive(true);
    }

    public void HideUI()
    {
        _container.SetActive(false);
    }

    // Call this method whenever the player makes a move
    public void OnPlayerMove(int remainingMoves)
    {
        _moveCountText.text = remainingMoves.ToString();
    }

    private void OnPlayerGoalUpdated(PlayerGoalData playerGoalData)
    {
        if (_goals.ContainsKey(playerGoalData.itemName) && playerGoalData.targetCount <= 0)
        {
            _playerGoalPool.Despawn(_goals[playerGoalData.itemName]);
            return;
        }

        if(_goals.ContainsKey(playerGoalData.itemName))
        {
            _goals[playerGoalData.itemName].SetGoal(playerGoalData);
        }

    
    }

    private void OnLevelLoaded(LevelData level)
    {
        Initialize(level);
        ShowUI();
    }
}
