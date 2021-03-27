using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager :IInitializable, ITickable {
    private readonly Player _player;
    private readonly LevelManager _levelManager;
    private readonly SignalBus _signalBus;

    private GameStates state = GameStates.WaitingToStart;

    public GameManager(Player player, LevelManager levelManager, SignalBus signalBus) {
        _player = player;
        _levelManager = levelManager;
        
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);

        _levelManager.Start();
    }

    private void OnPlayerDied(PlayerDiedSignal signalData) {
        state = GameStates.WaitingToStart;
    }

    public void Initialize() {

    }

    public void Tick() {
        switch(state) {
            case GameStates.WaitingToStart:
                OnWaitingForUsersInput();
                break;
            case GameStates.Playing:
                break;
            case GameStates.GameOver:
                GameEnd();
                break;
        }
    }

    private void OnWaitingForUsersInput() {
        if(Input.anyKeyDown) {
            StartGame();
        }
    }

    private void StartGame() {
        state = GameStates.Playing;
        _player.ChangeState(PlayerStates.Moving);
    }
    private void GameEnd() {
        state = GameStates.GameOver;
    }
}

public enum GameStates {
    WaitingToStart,
    Playing,
    GameOver
}