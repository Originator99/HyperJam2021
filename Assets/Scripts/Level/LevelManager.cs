﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelManager :ITickable {

    private readonly SignalBus _signalBus;
    private readonly Player _player;

    private Level currentLevel;

    public LevelManager(SignalBus signalBus, Player player) {
        _player = player;
        _signalBus = signalBus;

        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    public void RenderLevel(Level level) {
        currentLevel = level;
        _player.ResetPlayerPosition(currentLevel.GetStartBrick());
        _signalBus.Fire(new LevelStartedSignal { levelSettings = currentLevel.levelSettings });
    }

    public void Tick() {
    }

    public Brick GetBrickInDirection(Direction direction) {
        if(currentLevel != null) {
            return currentLevel.GetBrickInDirection(direction, _player.transform.position, _player.currentBrickCell);
        } else {
            Debug.LogError("Current level is null, cannot find brick in direction: " + direction.ToString());
            return null;
        }
    }

    private async void OnPlayerDied(PlayerDiedSignal signalData) {
        _player.ChangeState(PlayerStates.Dead);
        await Task.Delay(1000);
        currentLevel.ShuffleLevel();
        await Task.Delay(1000);
        _player.ResetPlayerPosition(currentLevel.GetStartBrick());
    }
}