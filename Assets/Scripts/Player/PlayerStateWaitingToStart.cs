using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerStateWaitingToStart :PlayerState {
    private readonly Player _player;
    public PlayerStateWaitingToStart(Player player) {
        _player = player;
    }

    public override void Start() {
        _player.gameObject.SetActive(true);
    }

    public override void Dispose() {

    }

    public override void Update() {

    }

    public class Factory :PlaceholderFactory<PlayerStateWaitingToStart> {
    
    }
}
