using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelRandomizer {
    private readonly LevelManager.Settings _levelSettings;
    private int bricksGenerated;

    public LevelRandomizer(LevelManager.Settings levelSettings) {
        _levelSettings = levelSettings;

        bricksGenerated = 0;
    }

    public BrickData GenerateRandomBrick(Vector2 worldPosition) {
        BrickData data = new BrickData();
        data.worldPosition = worldPosition;

        if(bricksGenerated != 0) {
            data.type = GetRandomBrickType();
        } else {
            data.type = BrickType.PATH; //want the 0 pos of map be empty for our player to move
        }
        if(_levelSettings.bricksData != null && _levelSettings.bricksData.Count > 0) {
            data.renderData = _levelSettings.bricksData[Random.Range(0, _levelSettings.bricksData.Count)];
        }
        bricksGenerated++;
        return data;
    }

    private BrickType GetRandomBrickType() {
        int random = Random.Range(0, 100);
        if((float)random / 100 <= _levelSettings.bombChance) {
            return BrickType.BOMB;
        }
        return BrickType.NORMAL;
    }
}
