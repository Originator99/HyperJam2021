using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStateWaitingToStart :PlayerState {
    public override void Start() {

    }

    public override void Dispose() {

    }

    public override void Update() {

    }

    public class Factory :PlaceholderFactory<PlayerStateWaitingToStart> {
    
    }
}
