using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelRandomizer {
    private readonly Player _player;
    private readonly LevelManager.Settings _levelSettings;

    public LevelRandomizer(LevelManager.Settings levelSettings, Player player) {
        _levelSettings = levelSettings;
        _player = player;
    }

    public BrickData GenerateRandomBrick(string ID, Vector3 worldPosition, int neighborsWithBombs) {
        BrickData data = new BrickData();
        data.worldPosition = worldPosition;
        data.gridNodeID = ID;

        if(_player.currentBrickCell.ID != ID) {
            data.type = GetRandomBrickType(neighborsWithBombs);
        } else {
            data.type = BrickType.PATH; //want the 0 pos of map be empty for our player to move
        }
        if(_levelSettings.bricksData != null && _levelSettings.bricksData.Count > 0) {
            data.renderData = _levelSettings.bricksData[Random.Range(0, _levelSettings.bricksData.Count)];
        }
        return data;
    }

    private BrickType GetRandomBrickType(int neighborsWithBombs) {
        int random = Random.Range(1, 100);
        if((float)random / 100 <= _levelSettings.bombChance && neighborsWithBombs <= 0) {
            return BrickType.BOMB;
        }
        return BrickType.NORMAL;
    }
}
