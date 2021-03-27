using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : ITickable, IFixedTickable {
    private readonly List<Brick> levelBricks = new List<Brick>();
    private readonly LevelRandomizer _levelLogic;
    private readonly Settings _settings;
    private readonly Player _player;

    private Grid2D grid;

    private Transform parentForBricks;

    private Node2D startPoint, endPoint;

    public LevelManager(Settings settings, LevelRandomizer levelLogic, Player player) {
        _settings = settings;
        _levelLogic = levelLogic;
        _player = player;

        CreateParentForBricks();
        Start();
    }
    public void Start() {
        levelBricks.Clear();

        grid = new Grid2D(_settings.worldSize, _settings.gridSpace);
        
        SetStartPoint();
        SetEndPoint();

        for(int x = 0; x < grid.gridSizeX; x++) {
            for(int y = 0; y < grid.gridSizeY; y++) {
                
                Brick brick = GenerateEmptyBrickCell();

                BrickData data = _levelLogic.GenerateRandomBrick(grid.Grid[x, y], GetNeighborNodes(grid.Grid[x, y], true));
                if(data.type == BrickType.BOMB && endPoint.ID == grid.Grid[x, y].ID) {
                    data.type = BrickType.END;
                }

                if(data.type == BrickType.BOMB) {
                    grid.Grid[x, y].SetObstacle(true);
                } else {
                    grid.Grid[x, y].SetObstacle(false);
                }

                brick.InitializeBrick(data);
                levelBricks.Add(brick);

                //for hirachy to look better 
                brick.transform.name = "Brick" + x + "," + y;
            }
        }

        FindPath(startPoint, endPoint);
    }

    public void Tick() {
        //Similar to update
    }

    public void FixedTick() {
        //similar to fixedUpdate
    }

    private void SetStartPoint() {
        startPoint = grid.Grid[0, 0];
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