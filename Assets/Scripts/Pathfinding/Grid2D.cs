using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid2D {
    
    private Vector3 gridWorldSize;
    private float nodeRadius;

    public Node2D[,] Grid;

    public List<Node2D> path;
    
    Vector3 worldBottomLeft;

    float nodeDiameter;

    public int gridSizeX, gridSizeY;

    public Grid2D(Vector3 worldSize, float cellSpace) {
        gridWorldSize = worldSize;
        nodeRadius = cellSpace;

        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    void CreateGrid() {
        Grid = new Node2D[gridSizeX, gridSizeY];

        worldBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++) {
            for(int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                Grid[x, y] = new Node2D(false, worldPoint, x, y);

                //if (obstaclemap.HasTile(obstaclemap.WorldToCell(Grid[x, y].worldPosition)))
                //Grid[x, y].SetObstacle(true);
                //else
                Grid[x, y].SetObstacle(false);
            }
        }
    }


    //gets the neighboring nodes in the 4 cardinal directions. If you would like to enable diagonal pathfinding, uncomment out that portion of code
    public List<Node2D> GetNeighbors(Node2D node, bool checkDiagonal = false) {
        List<Node2D> neighbors = new List<Node2D>();

        //checks and adds top neighbor
        if(node.GridX >= 0 && node.GridX < gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY + 1]);

        //checks and adds bottom neighbor
        if(node.GridX >= 0 && node.GridX < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
            neighbors.Add(Grid[node.GridX, node.GridY - 1]);

        //checks and adds right neighbor
        if(node.GridX + 1 >= 0 && node.GridX + 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
            neighbors.Add(Grid[node.GridX + 1, node.GridY]);

        //checks and adds left neighbor
        if(node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
            neighbors.Add(Grid[node.GridX - 1, node.GridY]);



        if(checkDiagonal) {
            //checks and adds top right neighbor
            if(node.GridX + 1 >= 0 && node.GridX + 1 < gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX + 1, node.GridY + 1]);

            //checks and adds bottom right neighbor
            if(node.GridX + 1 >= 0 && node.GridX + 1 < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX + 1, node.GridY - 1]);

            //checks and adds top left neighbor
            if(node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY + 1 >= 0 && node.GridY + 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX - 1, node.GridY + 1]);

            //checks and adds bottom left neighbor
            if(node.GridX - 1 >= 0 && node.GridX - 1 < gridSizeX && node.GridY - 1 >= 0 && node.GridY - 1 < gridSizeY)
                neighbors.Add(Grid[node.GridX - 1, node.GridY - 1]);
        }

        return neighbors;
    }

    public Node2D GetNodeInDirection(Direction direction, Node2D node, int level) {
        //checks and adds top neighbor
        if(direction == Direction.UP) {
            if(node.GridX >= 0 && node.GridX < gridSizeX && node.GridY + level >= 0 && node.GridY + level < gridSizeY)
                return (Grid[node.GridX, node.GridY + level]);
        }
        //checks and adds bottom neighbor
        if(direction == Direction.DOWN) {
            if(node.GridX >= 0 && node.GridX < gridSizeX && node.GridY - level >= 0 && node.GridY - level < gridSizeY)
                return (Grid[node.GridX, node.GridY - level]);
        }

        //checks and adds right neighbor
        if(direction == Direction.RIGHT) {
            if(node.GridX + level >= 0 && node.GridX + level < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
                return (Grid[node.GridX + level, node.GridY]);
        }

        //checks and adds left neighbor
        if(direction == Direction.LEFT) {
            if(node.GridX - level >= 0 && node.GridX - level < gridSizeX && node.GridY >= 0 && node.GridY < gridSizeY)
                return (Grid[node.GridX - level, node.GridY]);
        }

        return null; //means no node found in that direction
    }
    public Node2D NodeFromWorldPoint(Vector3 worldPosition) {

        int x = Mathf.RoundToInt(worldPosition.x - 1 + (gridSizeX / 2));
        int y = Mathf.RoundToInt(worldPosition.y + (gridSizeY / 2));
        return Grid[x, y];
    }
}
