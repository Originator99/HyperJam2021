using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrickType {
    NORMAL,
    BOMB,
    PATH,
    UNBREAKABLE,
    BOOM_BOT,
    END
}

[System.Serializable]
public class BrickData {
    public string gridNodeID;
    public BrickType type;
}

[CreateAssetMenu(fileName = "GraphicsData", menuName = "GraphicData/NewData", order = 1)]
public class GameGraphics : ScriptableObject {
    public GameObject[] normalBricks;
    public GameObject[] bombBricks;
    public GameObject[] unbreakableBricks;
    public GameObject boomBot;
    public GameObject portalBrick;
}