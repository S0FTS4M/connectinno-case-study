using System.Collections.Generic;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject]
    LevelSelectionUI.Settings LevelsUISettings;

    [Inject]
    Tile.Settings TileSettings;

    [Inject]
    PlayerGoal.Settings PlayerGoalSettings;

    public override void InstallBindings()
    {
        Container.Bind<ILevelDataManager>().To<LevelDataManager>().AsSingle();
        Container.Bind<ILevelManager>().To<LevelManager>().AsSingle();
        Container.Bind<IDataManager>().To<DataManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerGoalsManager>().AsSingle();


        Container.BindFactory<LevelButton, LevelButton.Factory>().FromComponentInNewPrefab(LevelsUISettings.levelButtonPrefab);
        Container.BindMemoryPool<Tile, TilePool>().WithInitialSize(50).FromComponentInNewPrefab(TileSettings.tilePrefab);
        Container.BindMemoryPool<PlayerGoal, PlayerGoal.PlayerGoalPool>().FromComponentInNewPrefab(PlayerGoalSettings.goalPrefab);
    }
}
