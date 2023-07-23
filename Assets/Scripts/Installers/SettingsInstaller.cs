using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public LevelSelectionUI.Settings LevelsUISettings;
    public Tile.Settings TileSettings;
    public LevelButton.Settings LevelButtonSettings;
    public PlayerGoal.Settings PlayerGoalSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(LevelsUISettings);
        Container.BindInstance(TileSettings);
        Container.BindInstance(LevelButtonSettings);
        Container.BindInstance(PlayerGoalSettings);
    }
}


