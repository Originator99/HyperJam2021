using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(playerSettings.stateDash);
        Container.BindInstance(playerSettings.stateDead);
        Container.BindInstance(radarSettings);
    }

    public PlayerSettings playerSettings;
    public Radar.Settings radarSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
        public PlayerStateDash.Settings stateDash;
        public PlayerStateDead.Settings stateDead;
    }
}
