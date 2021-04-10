using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelBuilder))]
public class EditorLevelGenerate :Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        LevelBuilder script = (LevelBuilder)target;
        if(GUILayout.Button("Build Default Level With Path")) {
            script.GenerateDefaultLevelGrid();
        }
        if(GUILayout.Button("Randomize Current Level")) {
            script.RandomizeLevel();
        }
        if(GUILayout.Button("Save Level")) {
            if(script.levelRoot != null) {
                PrefabUtility.SaveAsPrefabAsset(script.levelRoot, "Assets/Resources/LevelPrefabs/level" + script.levelSettings.levelNumber + ".prefab");
            }
        }
        if(GUILayout.Button("Destroy Level")) {
            script.DestroyLevel();
        }
    }
}
