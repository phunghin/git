using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(LevelEditor))]
public class GridManagerEditor : Editor
{
    string levelName = Settings.Level.ToString();

    public override void OnInspectorGUI()
    {
        LevelEditor level = (LevelEditor)target;

        
        GUILayout.BeginHorizontal();

        GUILayout.Label("Level :");
        levelName = GUILayout.TextField(levelName);

        if (GUILayout.Button(" Load "))
        {
            level.ClearMap();
            level.LoadMap(levelName.ToInt());
        }
        if (GUILayout.Button(" Save "))
        {
            level.SaveMap(levelName.ToInt());
        }
        if (GUILayout.Button(" Clear "))
        {
            level.ClearMap();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Build Map Default"))
        {
            level.BuildMap();
        }
        if (GUILayout.Button("Test Map"))
        {
            Manager.Instance.LevelChose = levelName.ToInt();
            EditorApplication.isPlaying = true;



        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Map"))
        {
            level.BuildMap();
        }

        GUILayout.EndHorizontal();
    }

}
