using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSignalsInstaller : MonoInstaller<GameSignalsInstaller> {
    public override void InstallBindings() {
    }
}

public struct PlayerDiedSignal {

}

public struct PlayerReachedEndSignal {
    public bool hasWon;
}

public struct PlayerInputSignal {
    public Direction moveDirection;
    public bool doDash; 
}

public struct BrickDestroyedSignal {
    public BrickData data;
}

public struct LevelStartedSignal {
    public Level.Settings levelSettings;
}