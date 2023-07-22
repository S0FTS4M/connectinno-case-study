using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    LevelsUI.Settings LevelsUISettings;


    public override void InstallBindings()
    {
        // Bind LevelData list with appropriate data for each level.
        // Container.Bind<List<ILevelData>>().FromInstance(CreateLevelDataList()).AsSingle();

        // Bind LevelManager interface to LevelManager implementation.

        Container.Bind<ILevelDataManager>().To<LevelDataManager>().AsSingle();

        // Bind DataManager interface to DataManager implementation.
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();


        Container.Bind<IDataManager>().To<DataManager>().AsSingle();

        // Bind other necessary dependencies.

        Container.BindFactory<LevelButton, LevelButton.Factory>().FromComponentInNewPrefab(LevelsUISettings.levelButtonPrefab);

    }
}
