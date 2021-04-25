using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(playerSettings.stateDash);
        Container.BindInstance(playerSettings.stateDead);
        Container.BindInstance(radarSettings);

        Container.BindInstance(powerUpSettings.immortalitySettings);
    }

    public PlayerSettings playerSettings;
    public PowerUpSettings powerUpSettings;
    public Radar.Settings radarSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
        public PlayerStateDash.Settings stateDash;
        public PlayerStateDead.Settings stateDead;
    }

    [System.Serializable]
    public class PowerUpSettings {
        public Immortality_PU.Settings immortalitySettings;
    }
}
