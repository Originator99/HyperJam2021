using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager :IInitializable, ITickable {
    private readonly Player _player;
    private readonly LevelManager _levelManager;
    private readonly SignalBus _signalBus;
    private readonly UIManager _uiManager;
    private readonly ScoreHelper _scoreHelper;

    private GameStates currentGameState = GameStates.WaitingToStart;

    private AudioSource _audioSource;

    public GameManager(Player player, LevelManager levelManager, SignalBus signalBus,UIManager uiManager, ScoreHelper scoreHelper, [Inject(Id = "GlobalAudio")] AudioSource audioSource) {
        _player = player;
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
        _levelManager.Start();
        StartGame();
    }

    public void Tick() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            _signalBus.Fire<PlayerReachedEndSignal>(new PlayerReachedEndSignal { hasWon = false });
        }
    }

    private void StartGame() {
        currentGameState = GameStates.Playing;

        _player.ChangeState(PlayerStates.Moving);
        _scoreHelper.ResetScore();

        _uiManager.ShowHUD();

        CheckAndShowTutorial();
    }

    private void OnGameOver(bool hasWon) {
        _uiManager.ShowGameOver(hasWon);
    }

    private void OnBrickDestroyed(BrickDestroyedSignal signalData) {
        // made one move
        _scoreHelper.UpdateMoves(1);
    }

    private void CheckAndShowTutorial() {
        if(PlayerPrefs.GetInt("tutorial", 0) == 0) {
            _player.ChangeState(PlayerStates.WaitingToStart);
            _uiManager.ShowDialogueBox(TutorialData.GetStartMessage1(null));
            _uiManager.ShowDialogueBox(TutorialData.GetStartMessage2(null));
            _uiManager.ShowDialogueBox(TutorialData.GetStartMessage3(null));
            _uiManager.ShowDialogueBox(TutorialData.GetStartMessage4(delegate() {
                PlayerPrefs.SetInt("tutorial", 1);
                _player.ChangeState(PlayerStates.Moving);
            }));

        }
    }
}

public enum GameStates {
    WaitingToStart,
    Playing,
    GameOver
}