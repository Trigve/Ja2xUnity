namespace Ja2
{
	/// <summary>
	/// Input manager.
	/// </summary>
	internal static class InputManager
	{
#region Fields
		/// <summary>
		/// Table is used to track which of the keys is up or down at any one time. This is used while polling the interface. TRUE = Pressed, FALSE = Not Pressed.
		/// </summary>
		private static readonly bool[] gfKeyState = new bool[256];
#endregion

#region Construction
		/// <summary>
		/// Initalization.
		/// </summary>
		/// <returns></returns>
		public static bool Init()
		{

			return true;
		}
#endregion
	}
}
