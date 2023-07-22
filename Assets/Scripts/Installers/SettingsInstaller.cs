using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public LevelsUI.Settings LevelsUISettings;
    public Tile.Settings TileSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(LevelsUISettings);
        Container.BindInstance(TileSettings);
    }
}


