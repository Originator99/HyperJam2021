using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game UI Settings")]
public class UI_SettingsInstaller :ScriptableObjectInstaller<GameSettingsInstaller> {
    public override void InstallBindings() {
        Container.BindInstance(patternSettings);
    }

    public Pattern.Settings patternSettings;
}
