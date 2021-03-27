﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ITickable, IFixedTickable {
    private readonly List<Brick> levelBricks = new List<Brick>();
    private readonly LevelRandomizer _levelLogic;
    private readonly Settings _settings;
    private readonly Player _player;
    private readonly SignalBus _signalBus;

    private Grid2D grid;

    private Transform parentForBricks;

    private Node2D startPoint, endPoint;

    public LevelManager(Settings settings, LevelRandomizer levelLogic, Player player, SignalBus signalBus) {
        _settings = settings;
        _levelLogic = levelLogic;
        _player = player;
        _signalBus = signalBus;
        _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);

        CreateParentForBricks();
    }

    public void Tick() {
        //Similar to update
    }

    public void FixedTick() {
        //similar to fixedUpdate
    }

    public void Start() {
        ResetLevel();
    }

    public void ResetLevel() {
        //clean this code later to reuse
        if(levelBricks.Count > 0) {
            foreach(Brick brick in levelBricks)
                GameObject.Destroy(brick.gameObject);
        }
        levelBricks.Clear();

        grid = new Grid2D(_settings.worldSize, _settings.gridSpace);

        SetStartPoint();
        SetEndPoint();

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                Node2D currentNode = grid.Grid[x, y];
                Brick brick = GenerateEmptyBrickCell();

                int neighborsWithBombs = 0;
                var neighbors = GetNeighborNodes(currentNode, true);
                if(neighbors != null) {
                    neighborsWithBombs = neighbors.FindAll(t => t.obstacle == true).Count;
                }

                BrickData data = _levelLogic.GenerateRandomBrick(currentNode.ID, currentNode.worldPosition, neighborsWithBombs);
                if(data.type == BrickType.BOMB && endPoint.ID == currentNode.ID) {
                    data.type = BrickType.END;
                }

                if(data.type == BrickType.BOMB) {
                    currentNode.SetObstacle(true);
                } else {
                    currentNode.SetObstacle(false);
                }

                brick.InitializeBrick(data);
                levelBricks.Add(brick);

                //for hirachy to look better 
                brick.transform.name = "Brick" + x + "," + y;
            }
        }

        FindPath(startPoint, endPoint);
    }

    public Node2D GetBrickInDirection(Direction direction, Node2D currentNode) {
        if(currentNode != null) {
            return grid.GetNodeInDirection(direction, currentNode);
        } else {
            Debug.LogError("Current Node is null, cannot find the next node in direction " + direction.ToString());
            return null;
        }
    }

    public List<Node2D> GetNeighborNodes(Node2D currentNode, bool checkDiagonal) {
        return grid.GetNeighbors(currentNode, checkDiagonal);
    }

    private void SetStartPoint() {
        startPoint = grid.Grid[Random.Range(0, grid.gridSizeX / 5), Random.Range(0, grid.gridSizeY / 5)];
        _player.ResetPlayerPosition(startPoint);
    }

    private void SetEndPoint() {
        endPoint = grid.Grid[Random.Range(grid.gridSizeX / 2, grid.gridSizeX), Random.Range(grid.gridSizeY / 2, grid.gridSizeY)];
        Debug.Log("End Point : " + endPoint.ID);

    }

    private void FindPath(Node2D start, Node2D end) {
        Pathfinding2D finder = new Pathfinding2D(grid);
        finder.FindPath(start, end);

        if(grid.path != null) {
            foreach(var pathNode in grid.path) {
                int index = levelBricks.FindIndex(x => x.IDOnGrid == pathNode.ID);
                if(index >= 0) {
                    levelBricks[index].DestroyBrick();
                }
            }
        } else {
            Debug.LogError("Path not found for end point " + end.ID);
        }
    }

    private Brick GenerateEmptyBrickCell() {
        GameObject obj = GameObject.Instantiate(_settings.emptyBrickPrefab, parentForBricks);
        Brick controller = obj.GetComponent<Brick>();
        if(controller == null) {
            controller = obj.AddComponent<Brick>();
        }
        return controller;
    }

    private void CreateParentForBricks() {
        GameObject parentForBricks = new GameObject {
            name = "Bricks"
        };
        this.parentForBricks = parentForBricks.transform;
    }

    private void OnPlayerDied(PlayerDiedSignal playerDiedData) {
        _player.ChangeState(PlayerStates.Dead);
        ResetLevel();
    }


    [System.Serializable]
    public class Settings {
        public int difficultyID = 1;
        public float gridSpace = 2.5f;
        public Vector3 worldSize;
        public float bombChance = 0.2f;
        public List<BrickGraphicData> bricksData;
        public GameObject emptyBrickPrefab;
    }
}