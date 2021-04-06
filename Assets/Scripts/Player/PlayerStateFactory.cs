using ModestTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates {
    WaitingToStart,
    Moving,
    Dead,
}

public class PlayerStateFactory {
    private readonly PlayerStateWaitingToStart.Factory _waitingToStartFactory;
    private readonly PlayerStateMoving.Factory _movingFactory;
    private readonly PlayerStateDead.Factory _deadFactory;

    public PlayerStateFactory(PlayerStateMoving.Factory movingFactory, PlayerStateWaitingToStart.Factory waitingToStartFactory,  PlayerStateDead.Factory deadFactory) {
        _movingFactory = movingFactory;
        _waitingToStartFactory = waitingToStartFactory;
        _deadFactory = deadFactory;
    }

    public PlayerState CreateState(PlayerStates state) {
        switch(state) {
            case PlayerStates.Moving:
                return _movingFactory.Create();
            case PlayerStates.WaitingToStart:
                return _waitingToStartFactory.Create();
            case PlayerStates.Dead:
                return _deadFactory.Create();
        }
        throw Assert.CreateException();
    }
}
