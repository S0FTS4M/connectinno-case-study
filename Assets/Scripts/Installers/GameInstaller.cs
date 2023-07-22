using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    LevelsUI.Settings LevelsUISettings;

    [Inject]
    GameManager.Settings GameManagerSettings;

    public override void InstallBindings()
    {
        // Bind LevelData list with appropriate data for each level.
        Container.Bind<List<ILevelData>>().FromInstance(CreateLevelDataList()).AsSingle();

        // Bind LevelManager interface to LevelManager implementation.
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();

        // Bind DataManager interface to DataManager implementation.
        Container.Bind<IDataManager>().To<DataManager>().AsSingle();

        // Bind other necessary dependencies.

        Container.BindFactory<LevelButton, LevelButton.Factory>().FromComponentInNewPrefab(LevelsUISettings.levelButtonPrefab);

    }

    private List<ILevelData> CreateLevelDataList()
    {
        var levelDatas = new List<ILevelData>();

        for (int i = 0; i < GameManagerSettings.levelPrefabs.Length; i++)
        {
            levelDatas.Add(new LevelData() { LevelNumber = i, HighestScore = 0 });
        }
        // Create a list of ILevelData (LevelData) with appropriate level information.
        // For simplicity, you can hardcode the data here or load it from a file or database.
        return levelDatas;
    }
}
