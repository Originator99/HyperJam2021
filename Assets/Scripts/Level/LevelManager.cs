using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LevelManager :ITickable {

    private readonly SignalBus _signalBus;
    private readonly Player _player;
    private readonly ChapterDataManager _chapterDataManager;

    private Level currentLevel;

    private LevelData currentLevelData;

    public LevelManager(SignalBus signalBus, Player player, ChapterDataManager chapterDataManager) {
        _player = player;
        _signalBus = signalBus;
        _chapterDataManager = chapterDataManager;

        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        _signalBus.Subscribe<PlayerReachedEndSignal>(OnLevelEnd);
    }

    public void RenderLevel(Level level, LevelData levelData) {
        CheckAndDestroyCurrentLevel();

        currentLevel = level;
        currentLevelData = levelData;

        _player.ResetPlayerPosition(currentLevel.GetStartBrick());
        _signalBus.Fire(new LevelStartedSignal { levelSettings = currentLevel.levelSettings });

        foreach(BaseBrick brick in level.levelBricks) {
            brick.Construct(_signalBus);
        }
    }

    public void Tick() {
    }

    public Level.Settings GetCurrentLevelSettings() {
        return currentLevel.levelSettings;
    }

    public BaseBrick GetBrickInDirectionFrom(BaseBrick currentBrick, Direction direction, Vector2 fromPosition) {
        if(currentLevel != null) {
            return currentLevel.GetBrickInDirectionFrom(currentBrick, direction, fromPosition);
        } else {
            Debug.LogError("Current level is null, cannot find brick in direction: " + direction.ToString());
            return null;
        }
    }

    public void CheckAndDestroyCurrentLevel() {
        if(currentLevel != null) {
            GameObject.Destroy(currentLevel.gameObject);
        }
    }

    public List<BaseBrick> GetPortalBricks() {
        if(currentLevel != null) {
            return currentLevel.GetPortalBricks();
        } else {
            Debug.LogError("Current level is null, cannot find portal bricks ");
            return null;
        }
    }

    private async void OnPlayerDied(PlayerDiedSignal signalData) {
        _player.ChangeState(PlayerStates.Dead);
        await Task.Delay(1000);
        currentLevel.ShuffleLevel();
        await Task.Delay(1500);
        _player.ResetPlayerPosition(currentLevel.GetStartBrick());
    }

    private void OnLevelEnd(PlayerReachedEndSignal signalData) {
        if(signalData.hasWon) {
            ChapterData currentChapter = _chapterDataManager.GetCurrentChapter();
            _chapterDataManager.SetChapterLevelAsCompelte(currentChapter.chaper_id, currentLevelData.level_number);
        }
        _player.gameObject.SetActive(false);
    }

}