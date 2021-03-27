using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(playerSettings.stateDash);
        Container.BindInstance(playerSettings.stateDead);

        Container.BindInstance(levelSettings.levelInfo);

        Container.BindInstance(cameraSettings.cameraMovementSettings);

        //UI Settings
        Container.BindInstance(patterSettings); 
    }

    public PlayerSettings playerSettings;
    public LevelSettings levelSettings;
    public CameraSettings cameraSettings;

    [Space(5)]
    public Pattern.Settings patterSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
        public PlayerStateDash.Settings stateDash;
        public PlayerStateDead.Settings stateDead;
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
