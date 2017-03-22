using UnityEditor;

namespace Gamelogic.Extensions.Editor.Internal
{
	public partial class GLMenu
	{
		[MenuItem("Gamelogic/Grids/API Documentation")]
		public static void OpenGridsAPI()
		{
			OpenUrl("http://www.gamelogic.co.za/documentation/grids/");
		}
	}
}