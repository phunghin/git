using UnityEngine;
using UnityEditor;
using System;

public static class EditorGUIHelper
{
	public static void PropertyArray(SerializedProperty array, int count, Func<int, string> func)
	{
		EditorGUILayout.PropertyField(array);

		if (array.isExpanded)
		{
			EditorGUI.indentLevel++;

			if (array.arraySize < count)
			{
				do
				{
					array.InsertArrayElementAtIndex(array.arraySize);
				}
				while (array.arraySize < count);
			}
			else if (array.arraySize > count)
			{
				do
				{
					array.DeleteArrayElementAtIndex(array.arraySize - 1);
				}
				while (array.arraySize > count);
			}

			for (int i = 0; i < array.arraySize; i++)
			{
				EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i), new GUIContent(func(i)));
			}

			EditorGUI.indentLevel--;
		}
	}
}
