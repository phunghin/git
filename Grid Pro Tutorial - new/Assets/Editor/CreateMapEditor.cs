using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditorController))]
public class CreateMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapEditorController level = (MapEditorController)target;

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(" Build "))
        {
            level.BuildLevel();
        }
        if (GUILayout.Button(" FixLevel "))
        {
            level.RepPlayBuildLevel();
        }
        GUILayout.EndHorizontal();
    }

}
