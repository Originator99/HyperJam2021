using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelRandomizer {
    private readonly Player _player;
    private readonly LevelManager.Settings _levelSettings;
    private int bricksGenerated;

    public LevelRandomizer(LevelManager.Settings levelSettings, Player player) {
        _levelSettings = levelSettings;
        _player = player;
        bricksGenerated = 0;
    }

    public BrickData GenerateRandomBrick(Node2D gridNode, List<Node2D> neighbors) {
        BrickData data = new BrickData();
        data.worldPosition = gridNode.worldPosition;
        data.gridNodeID = gridNode.ID;

        if(_player.currentBrickCell.ID != gridNode.ID) {
            data.type = GetRandomBrickType(neighbors);
        } else {
            data.type = BrickType.PATH; //want the 0 pos of map be empty for our player to move
        }
        if(_levelSettings.bricksData != null && _levelSettings.bricksData.Count > 0) {
            data.renderData = _levelSettings.bricksData[Random.Range(0, _levelSettings.bricksData.Count)];
        }
        bricksGenerated++;
        return data;
    }

    private BrickType GetRandomBrickType(List<Node2D> neighbors) {
        int random = Random.Range(1, 100);
        int neighborsWithBombs = 0;
        if(neighbors != null) {
            neighborsWithBombs = neighbors.FindAll(x => x.obstacle == true).Count;
        }
        if((float)random / 100 <= _levelSettings.bombChance && neighborsWithBombs <= 0) {
            return BrickType.BOMB;
        }
        return BrickType.NORMAL;
    }
}
