using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelLogic {
    public readonly GameGraphics _graphics;
    public readonly Grid2D _grid;

    private Node2D startPoint, endPoint;

    public LevelLogic(Grid2D levelGrid, GameGraphics gameGraphics) {
        //not binded in zenject yet
        _graphics = gameGraphics;
        _grid = levelGrid;

    }

    #region Level Randomization

    public GameObject GetBrickBasedOnType(BrickType type) {
        switch(type) {
            case BrickType.NORMAL:
                return GenerateNormalBrick();
            case BrickType.BOMB:
                return GenerateBombBrick();
            case BrickType.END:
                return GeneratePortalBrick();
            case BrickType.UNBREAKABLE:
                return GenerateUnbreakableBrick();
            default:
                Debug.LogWarning("Could not find brick gameobject for type : " + type.ToString() + "\n Returning normal type");
                return GenerateNormalBrick();
        }
        
    }

    public BrickType GetRandomBrickType(float bombChance) {
        BrickType type = BrickType.NORMAL;

        int random = Random.Range(1, 100);
        if((float)random / 100 <= bombChance) {
            type = BrickType.BOMB;
        }
        return type;
    }

    #endregion

    #region GeneratingBrickTypes
    private GameObject GenerateNormalBrick() {
        if(_graphics != null && _graphics.normalBricks !=null && _graphics.normalBricks.Length >0) {
            return UnityEditor.PrefabUtility.InstantiatePrefab(_graphics.normalBricks[Random.Range(0, _graphics.normalBricks.Length)]) as GameObject;
        }
        Debug.LogError("Cannot generate normal brick, the graphics data is null or array of normal bricks is empty");
        return null;
    }
    private GameObject GenerateBombBrick() {
        if(_graphics != null && _graphics.bombBricks != null && _graphics.bombBricks.Length > 0) {
            return UnityEditor.PrefabUtility.InstantiatePrefab(_graphics.bombBricks[Random.Range(0, _graphics.bombBricks.Length)]) as GameObject;
        }
        Debug.LogError("Cannot generate bomb brick, the graphics data is null or array of bomb bricks is empty");
        return null;
    }
    private GameObject GeneratePortalBrick() {
        if(_graphics != null && _graphics.portalBrick != null) {
            return UnityEditor.PrefabUtility.InstantiatePrefab(_graphics.portalBrick) as GameObject;
        }
        Debug.LogError("Cannot generate normal brick, the graphics data is null or portal Brick prefab is null");
        return null;
    }
    private GameObject GenerateUnbreakableBrick() {
        if(_graphics != null && _graphics.unbreakableBricks != null && _graphics.unbreakableBricks.Length > 0) {
            return UnityEditor.PrefabUtility.InstantiatePrefab(_graphics.unbreakableBricks[Random.Range(0, _graphics.unbreakableBricks.Length)]) as GameObject;
        }
        Debug.LogError("Cannot generate unbreakable brick, the graphics data is null or empty");
        return null;
    }
    #endregion

    //not using this for now
    #region Path Generation via Grid
    /*
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

            //findPath doesnt consider the first node as part of the path so we override
            _grid.path.Insert(0, start);

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
    */
    #endregion
}
