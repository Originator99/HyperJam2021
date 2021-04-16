using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(playerSettings.stateDash);
        Container.BindInstance(playerSettings.stateDead);
        Container.BindInstance(radarSettings);

        Container.BindInstance(cameraSettings.cameraMovementSettings);

    }

    public PlayerSettings playerSettings;
    public CameraSettings cameraSettings;
    public Radar.Settings radarSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
        public PlayerStateDash.Settings stateDash;
        public PlayerStateDead.Settings stateDead;
    }

    [System.Serializable]
    public class CameraSettings {
        public CameraStateFollowing.Settings cameraMovementSettings;
    }
}
