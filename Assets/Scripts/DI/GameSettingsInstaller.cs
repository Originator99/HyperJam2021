using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(playerSettings.stateDash);

        Container.BindInstance(levelSettings.levelInfo);

        Container.BindInstance(cameraSettings.cameraMovementSettings);
    }

    public PlayerSettings playerSettings;
    public LevelSettings levelSettings;
    public CameraSettings cameraSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
        public PlayerStateDash.Settings stateDash;
    }

    [System.Serializable]
    public class LevelSettings {
        public LevelManager.Settings levelInfo;
    }

    [System.Serializable]
    public class CameraSettings {
        public CameraStateFollowing.Settings cameraMovementSettings;
    }
}
