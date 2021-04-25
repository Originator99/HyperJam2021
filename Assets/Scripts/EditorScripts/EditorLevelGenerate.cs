using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelBuilder))]
public class EditorLevelGenerate :Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUILayout.Space(15);

        LevelBuilder script = (LevelBuilder)target;
        if(GUILayout.Button("Build Randomized Level without Path")) {
            script.GenerateDefaultLevelGrid();
        }
        if(GUILayout.Button("Save Level")) {
            script.SaveLevelSettings();
            if(script.levelRoot != null) {
                PrefabUtility.SaveAsPrefabAsset(script.levelRoot, "Assets/Resources/LevelPrefabs/level" + script.levelID + ".prefab");
            }
        }
        if(GUILayout.Button("Shuffle Level")) {
            script.ShuffleLevel();
        }
        if(GUILayout.Button("Destroy Level")) {
            script.DestroyLevel();
        }

        if(GUILayout.Button("Show Safe Path")) {
            script.ToggleSafePath(true);
        }
        if(GUILayout.Button("Hide Safe Path")) {
            script.ToggleSafePath(false);
        }

        DrawBrickChange(script);
        SafePath(script);
    }

    private void DrawBrickChange(LevelBuilder script) {
        GUILayout.Space(15);
        GUILayout.Label("Switch Brick to");
        if(GUILayout.Button("Normal")) {
            script.SwitchBrickToType(BrickType.NORMAL);
        }
        if(GUILayout.Button("Bomb")) {
            script.SwitchBrickToType(BrickType.BOMB);
        }
        if(GUILayout.Button("Portal")) {
            script.SwitchBrickToType(BrickType.END);
        }
        if(GUILayout.Button("Unbreakable")) {
            script.SwitchBrickToType(BrickType.UNBREAKABLE);
        }
        if(GUILayout.Button("Boom Bot")) {
            script.SwitchBrickToType(BrickType.BOOM_BOT);
        }
        if(GUILayout.Button("Random Boom Bot")) {
            script.SwitchBrickToType(BrickType.RANDOM_BLOW);
        }
        if(GUILayout.Button("Path")) {
            script.SwitchBrickToType(BrickType.PATH);
        }
    }

    private void SafePath(LevelBuilder script) {
        GUILayout.Space(15);
        GUILayout.Label("Safe Path");
        GUILayout.Label("Make sure to fill the safe path list above");
        if(GUILayout.Button("Save Path")) {
            script.GenerateSafePath();
        }
    }
}
