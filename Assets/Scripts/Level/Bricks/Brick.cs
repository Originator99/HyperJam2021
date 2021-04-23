using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BrickA : MonoBehaviour {

}


public enum BrickType {
    NORMAL,
    BOMB,
    PATH,
    SAFE_PATH,
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
    public GameObject portalBrick;
}