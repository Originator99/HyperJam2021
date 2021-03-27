using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour {

    private PlayerStateFactory _stateFactory;
    private PlayerState _state;

    [Inject]
    public void Construct(PlayerStateFactory stateFactory) {
        _stateFactory = stateFactory;
    }
    private void Start() {
        ChangeState(PlayerStates.WaitingToStart);
    }
    private void Update() {
        _state?.Update();
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
