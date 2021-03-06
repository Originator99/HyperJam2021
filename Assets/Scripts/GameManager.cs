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
        ShowStartScreen();
    }

    public async void BuildLevel(LevelData levelData) {
        if(levelData != null && !string.IsNullOrEmpty(levelData.prefab_name)) {
            ShowLoadingScreen(2f);
            await System.Threading.Tasks.Task.Delay(750);
            GameObject levelPrefab = FetchLevelPrefab(levelData.prefab_name);
            LoadLevel(levelPrefab, levelData);
        } else {
            Debug.LogError("Prefab name is empty");
        }
    }

    public void LoadLevel(GameObject levelPrefab, LevelData levelData) {
        if(levelPrefab != null) {
            GameObject level = GameObject.Instantiate(levelPrefab);
            _levelManager.RenderLevel(level.GetComponent<Level>(), levelData);

            StartGame();
        } else {
            Debug.LogError("Cannot build level, level prefab not found in resources folder");
        }
    }

    public void Tick() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            _signalBus.Fire<CameraZoomSingal>(new CameraZoomSingal { isZoom = true });
        }
    }

    private void StartGame() {
        currentGameState = GameStates.Playing;
        _scoreHelper.ResetScore();

        _uiManager.ShowHUD();
    }

    public void ShowStartScreen() {
        _levelManager.CheckAndDestroyCurrentLevel();
        _uiManager.ShowStartScren();
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

    public void ShowLoadingScreen(float forTime) {
        _uiManager.ShowLoadingScreen(forTime);
    }
}

public enum GameStates {
    WaitingToStart,
    Playing,
    GameOver
}