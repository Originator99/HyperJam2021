using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager :IInitializable, ITickable {
    private readonly Player _player;

    private GameStates state = GameStates.WaitingToStart;

    public GameManager(Player player) {
        _player = player;
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
                break;
        }
    }

    private void OnWaitingForUsersInput() {
        if(Input.GetMouseButtonDown(0)) {
            StartGame();
        }
    }

    private void StartGame() {
        state = GameStates.Playing;
        _player.ChangeState(PlayerStates.Moving);
    }
}

public enum GameStates {
    WaitingToStart,
    Playing,
    GameOver
}