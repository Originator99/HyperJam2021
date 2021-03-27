using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates {
    WaitingToStart,
    Moving,
    Dashing
}

public class PlayerStateFactory {
    private readonly PlayerStateWaitingToStart.Factory _waitingToStartFactory;
    private readonly PlayerStateMoving.Factory _movingFactory;

    public PlayerStateFactory(PlayerStateMoving.Factory movingFactory, PlayerStateWaitingToStart.Factory waitingToStartFactory) {
        _movingFactory = movingFactory;
        _waitingToStartFactory = waitingToStartFactory;
    }

    public PlayerState CreateState(PlayerStates state) {
        switch(state) {
            case PlayerStates.Moving:
                return _movingFactory.Create();
            case PlayerStates.Dashing:
                break;
            case PlayerStates.WaitingToStart:
                return _waitingToStartFactory.Create();
        }
        throw Assert.CreateException();
    }
}
