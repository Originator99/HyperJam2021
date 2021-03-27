using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour {

    private PlayerStateFactory _stateFactory;
    private PlayerState _state;

    public Direction currentDirection;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory) {
        _stateFactory = stateFactory;

        currentDirection = Direction.UP;
    }
    
    private void Start() {
        ChangeState(PlayerStates.WaitingToStart);
    }

    private void Update() {
        _state?.Update();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _state?.OnTriggerEnter2D(collision);
    }

    public void ChangeState(PlayerStates state) {
        if(_state != null) {
            _state.Dispose();
            _state = null;
        }
        _state = _stateFactory.CreateState(state);
        _state.Start();
    }
}
