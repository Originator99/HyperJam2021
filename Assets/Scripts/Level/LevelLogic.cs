using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelLogic {
    private readonly Settings _levelSettings;
    private readonly Grid2D _grid;

    private Node2D startPoint, endPoint;

    public LevelLogic(Grid2D levelGrid, Settings levelSettings) {
        _grid = levelGrid;
        _levelSettings = levelSettings;
    }

    public void DebugLevel() {
        Debug.Log(_grid.Grid.Length);
    }

    #region Level Randomization
    public void Randomize() {
        if(_levelSettings != null && _grid != null && _grid.Grid != null) {
            for(int x = 0; x < _grid.gridSizeX; x++) {
                for(int y = 0; y < _grid.gridSizeY; y++) {
                    Node2D currentNode = _grid.Grid[x, y];
                    Brick brick = currentNode.data as Brick;

                    BrickData data = new BrickData {
                        gridNodeID = currentNode.ID,
                        renderData = GetRandomBrickSprite(currentNode.ID),
                        type = GetRandomBrickType(currentNode.ID),
                        worldPosition = currentNode.worldPosition
                    };

                    brick.InitializeBrick(data);
                }
            }
        } else {
            Debug.LogError("Level Logic not initialized");
        }
    }

    private BrickGraphicData GetRandomBrickSprite(string currentNodeID) {
        if(currentNodeID == endPoint?.ID) {
            return _levelSettings.endPointGFX;
        }

        if(_levelSettings.bricksGfxData != null && _levelSettings.bricksGfxData.Count > 0) {
            return _levelSettings.bricksGfxData[Random.Range(0, _levelSettings.bricksGfxData.Count)];
        } else {
            Debug.LogError("GFX data is null, cannot render brick");
        }

        return null;
    }

    private BrickType GetRandomBrickType(string currentNodeID) {
        if(currentNodeID == startPoint?.ID) {
            return BrickType.PATH;
        }

        if(currentNodeID == endPoint?.ID) {
            return BrickType.END;
        }

        if(_levelSettings != null) {
            int random = Random.Range(1, 100);
            if((float)random / 100 <= _levelSettings.bombChance && !IsAPartOfThePath(currentNodeID)) {
                return BrickType.BOMB;
            }
        } else {
            Debug.LogError("Level settings are null, cannot Randomize Brick Type");
        }
        return BrickType.NORMAL;
    }

    #endregion


    #region Path Generation
    public void GenerateRandomPath() {
        if(_grid != null) {
            startPoint = _grid.Grid[Random.Range(0, _grid.gridSizeX / 5), Random.Range(0, _grid.gridSizeY / 5)];
            endPoint = _grid.Grid[Random.Range(_grid.gridSizeX / 2, _grid.gridSizeX), Random.Range(_grid.gridSizeY / 2, _grid.gridSizeY)];

            FindPath(startPoint, endPoint);
        } else {
            Debug.LogError("Cannot find path, Grid is null");
        }
    }

    private void FindPath(Node2D start, Node2D end) {
        Pathfinding2D finder = new Pathfinding2D(_grid);
        finder.FindPath(start, end);

        if(_grid.path != null) {
            Debug.Log("Path found for start : " + start.ID + " and End : " + end.ID);
        } else {
            Debug.LogError("Path not found for end point " + end.ID);
        }
    }

    private bool IsAPartOfThePath(string nodeID) {
        if(_grid != null && _grid.path != null && _grid.path.Count > 0) {
            return _grid.path.FindIndex(x => x.ID == nodeID) != -1;
        }
        return false;
    }
    #endregion


    [System.Serializable]
    public class Settings {
        public int levelNumber;
        public BrickGraphicData endPointGFX;
        public List<BrickGraphicData> bricksGfxData;
        public float bombChance;
    }

}
