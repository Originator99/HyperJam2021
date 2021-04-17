using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager :IInitializable, ITickable {
    private readonly LevelManager _levelManager;
    private readonly SignalBus _signalBus;
    private readonly UIManager _uiManager;
    private readonly ScoreHelper _scoreHelper;

    private GameStates currentGameState = GameStates.WaitingToStart;

    private AudioSource _audioSource;

    public GameManager(LevelManager levelManager, SignalBus signalBus, UIManager uiManager, ScoreHelper scoreHelper, [Inject(Id = "GlobalAudio")] AudioSource audioSource) {
        _levelManager = levelManager;
        _uiManager = uiManager;
        _scoreHelper = scoreHelper;
        _audioSource = audioSource;

        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerReachedEndSignal>(OnPlayerReachedEnd);
        _signalBus.Subscribe<BrickDestroyedSignal>(OnBrickDestroyed);
    }

    private void OnPlayerReachedEnd(PlayerReachedEndSignal signalData) {
        currentGameState = GameStates.GameOver;

        OnGameOver(signalData.hasWon);
    }

    public void Initialize() {
        _uiManager.ShowStartScren();
    }

    public void BuildLevel() {
        GameObject levelPrefab = FetchLevelPrefab("level2");
        if(levelPrefab != null) {
            GameObject level = GameObject.Instantiate(levelPrefab);
            _levelManager.RenderLevel(level.GetComponent<Level>());

            StartGame();
        } else {
            Debug.LogError("Cannot build level, level prefab not found in resources folder");
        }
    }

    public void Tick() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            _signalBus.Fire<PlayerReachedEndSignal>(new PlayerReachedEndSignal { hasWon = false });
        }
    }

    private void StartGame() {
        currentGameState = GameStates.Playing;
        _scoreHelper.ResetScore();

        _uiManager.ShowHUD();
    }

    private void OnGameOver(bool hasWon) {
        _uiManager.ShowGameOver(hasWon);
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {
        // made one move
        _scoreHelper.UpdateMoves(1);
    }

    private GameObject FetchLevelPrefab(string levelID) {
        return Resources.Load("LevelPrefabs/" + levelID) as GameObject;
    }
}

public enum GameStates {
    WaitingToStart,
    Playing,
    GameOver
}