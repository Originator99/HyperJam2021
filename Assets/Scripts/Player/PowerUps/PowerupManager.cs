using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PowerupManager {
    private readonly SignalBus _signalBus;
    private readonly Settings _settings;

    public PowerupManager(SignalBus signalBus, Settings settings) {
        _signalBus = signalBus;
        _settings = settings;

        _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
        _signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);
        _signalBus.Subscribe<PlayerReachedEndSignal>(OnLevelEndReached);
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
    }

    private void OnLevelStarted(LevelStartedSignal signalData) {

    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {

    }

    private void OnLevelEndReached(PlayerReachedEndSignal signalData) {

    }

    private void OnPlayerDied(PlayerDiedSignal signalData) {

    }


    public class Settings {
        public int bricksToDestroyForImmortality;
    }
}

public enum PowerUpType {
    IMMORTALITY,
    MISSLE,
}
