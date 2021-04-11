using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node2D : IDisposable {
    public string ID { 
        get {
            return GridX.ToString() + GridY.ToString(); 
        } 
    }

    public int gCost, hCost;
    public bool obstacle;
    public Vector3 worldPosition;

    public int GridX, GridY;
    public Node2D parent;
    public System.Object data;


    public Node2D(bool _obstacle, Vector3 _worldPos, int _gridX, int _gridY, System.Object data = null) {
        obstacle = _obstacle;
        worldPosition = _worldPos;
        GridX = _gridX;
        GridY = _gridY;

        this.data = data;
    }

    public int FCost {
        get {
            return gCost + hCost;
        }

    }

    public void SetObstacle(bool isOb) {
        obstacle = isOb;
    }

    public void SetData(System.Object data) {
        this.data = data;
    }

    public void Dispose() {
        data = null;
    }
}
