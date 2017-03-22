using UnityEngine;
using UnityEditor;

namespace Gamelogic.Extensions.Editor
{
	/// <summary>
	/// A property drawer for fields marked with the CommentAttribute. Similar to Header, but useful
	/// for longer descripotions. 
	/// </summary>
	[CustomPropertyDrawer(typeof(CommentAttribute))]
	public class CommentPropertyDrawer : PropertyDrawer
	{
		CommentAttribute CommentAttribute
		{
			get { return (CommentAttribute) attribute; }
		}

		private GUIStyle commentStyle;

		private GUIStyle CommentStyle
		{
			get
			{
				if (commentStyle == null)
				{
					commentStyle = new GUIStyle(EditorStyles.whiteMiniLabel)
					{
						wordWrap = true,
						normal = {textColor = new Color(0.4f, 0.4f, 0.4f)}
					};

				}

				return commentStyle;
			}
		}

		public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
		{
			return CommentStyle.CalcHeight(CommentAttribute.content, Screen.width - 19) + EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			position.height -= EditorGUIUtility.singleLineHeight;
			EditorGUI.LabelField(position, CommentAttribute.content, CommentStyle);
			position.height += EditorGUIUtility.singleLineHeight;

			position.y += position.height - EditorGUIUtility.singleLineHeight;
			position.height = EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(position, prop);
		}
	}
}
