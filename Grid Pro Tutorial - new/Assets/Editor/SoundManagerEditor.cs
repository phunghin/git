using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		SerializedProperty musicClips = serializedObject.FindProperty("musicClips");
		EditorGUIHelper.PropertyArray(musicClips, (int)MusicID.Count, (i) => { return ((MusicID)i).ToString(); });

		SerializedProperty soundClips = serializedObject.FindProperty("soundClips");
		EditorGUIHelper.PropertyArray(soundClips, (int)SoundID.Count, (i) => { return ((SoundID)i).ToString(); });

		EditorGUILayout.PropertyField(serializedObject.FindProperty("musicVolume"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("soundVolume"));

		if (GUI.changed)
		{
			serializedObject.ApplyModifiedProperties();
		}
	}
}
