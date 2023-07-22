using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
{
    public LevelsUI.Settings LevelsUISettings;
    public override void InstallBindings()
    {
        Container.BindInstance(LevelsUISettings);
    }
}


