namespace Ja2
{
	/// <summary>
	/// Input manager.
	/// </summary>
	internal class InputManager
	{
#region Fields Static
		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static InputManager? m_Instance;
#endregion

#region Fields
		/// <summary>
		/// Table is used to track which of the keys is up or down at any one time. This is used while polling the interface. TRUE = Pressed, FALSE = Not Pressed.
		/// </summary>
		private readonly bool[] gfKeyState = new bool[256];
#endregion

#region Properties
		/// <summary>
		/// Singletong instance.
		/// </summary>
		public static InputManager instance => m_Instance!;
#endregion

#region Construction
		/// <summary>
		/// Initalization.
		/// </summary>
		/// <returns></returns>
		public static bool Init()
		{
			m_Instance ??= new InputManager();


			return true;
		}
#endregion
	}
}
