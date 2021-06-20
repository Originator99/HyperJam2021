using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour {
    public GameObject prefab;
    public Vector2 position;

    public void BuildObject() {
        Instantiate(prefab, position, Quaternion.identity);
    }
}

