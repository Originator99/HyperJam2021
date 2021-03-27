using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {
    public void PlaceBrick(Vector2 position) {
        gameObject.SetActive(true);
        transform.position = position;
    }
}
