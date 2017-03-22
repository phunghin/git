
using Gamelogic.Extensions.Internal;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Object = UnityEngine.Object;

namespace Gamelogic.Grids
{
	/// <summary>
	/// Provides utility functions for grid builders.
	/// Renamed from GridBuilderUtil to GridBuilderUtils(1.8.1)
	/// </summary>
	[Version(1,8)]
	public static class GridBuilderUtils
	{
		/// <summary>
		/// This function only works if a main camera has been set.
		/// </summary>
		public static Vector3 ScreenToWorld(Vector3 screenPosition)
		{
			if (Camera.main == null)
			{
				Debug.LogError("No main camera found in scene");

				return Vector3.zero;
			}
#if GL_NGUI
			return UICamera.currentCamera.ScreenToWorldPoint(screenPosition);
#else
			return Camera.main.ScreenToWorldPoint(screenPosition);
#endif
		}

		/// <summary>
		/// This function only works if a main camera has been set.
		/// </summary>
		public static Vector3 ScreenToWorld(GameObject root, Vector3 screenPosition)
		{
			if (Camera.main == null)
			{
				Debug.LogError("No main camera found in scene");

				return Vector3.zero;
			}
#if GL_NGUI
			return root.transform.InverseTransformPoint(UICamera.currentCamera.ScreenToWorldPoint(screenPosition));
#else
			return root.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(screenPosition));
#endif
		}

		public static T Instantiate<T>(T prefab)
			where T : MonoBehaviour
		{
			T instance = null;
			if (Application.isPlaying)
			{
				instance = (T) Object.Instantiate(prefab);
			}
#if UNITY_EDITOR
			else
			{
				instance = (T) PrefabUtility.InstantiatePrefab(prefab);
			}
#endif
			return instance;
		}
	}
}
