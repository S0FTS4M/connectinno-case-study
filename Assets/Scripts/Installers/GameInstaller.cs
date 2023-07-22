using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    LevelsUI.Settings LevelsUISettings;

    [Inject]
    Tile.Settings TileSettings;

    public override void InstallBindings()
    {
        Container.Bind<ILevelDataManager>().To<LevelDataManager>().AsSingle();

        // Bind DataManager interface to DataManager implementation.
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();


        Container.Bind<IDataManager>().To<DataManager>().AsSingle();


        Container.BindFactory<LevelButton, LevelButton.Factory>().FromComponentInNewPrefab(LevelsUISettings.levelButtonPrefab);
        Container.BindMemoryPool<Tile, TilePool>().WithInitialSize(50).FromComponentInNewPrefab(TileSettings.tilePrefab);
    }
}
