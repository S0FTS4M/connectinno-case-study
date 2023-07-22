using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private IDataManager _dataManager;
    private ILevelManager _levelManager;
    private LevelButton.Factory _levelButtonFactory;

    [Inject]
    public void Construct(IDataManager dataManager, ILevelManager levelManager, LevelButton.Factory levelButtonFactory)
    {
        this._dataManager = dataManager;
        this._levelManager = levelManager;
        this._levelButtonFactory = levelButtonFactory;
    }

    private void Start()
    {
        
    }

    [System.Serializable]
    public class Settings
    {
        public GameObject[] levelPrefabs;
    }
}
