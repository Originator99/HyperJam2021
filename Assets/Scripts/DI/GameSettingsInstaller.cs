using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Roulette/Game Settings")]
public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {

    public override void InstallBindings() {
        Container.BindInstance(playerSettings.stateMoving);
        Container.BindInstance(levelSettings.levelInfo);
    }

    public PlayerSettings playerSettings;
    public LevelSettings levelSettings;

    [System.Serializable]
    public class PlayerSettings {
        public PlayerStateMoving.Settings stateMoving;
    }

    [System.Serializable]
    public class LevelSettings {
        public LevelManager.Settings levelInfo;
    }
}
