namespace Ja2
{
	/// <summary>
	/// Ja2 settings.
	/// </summary>
	public static class Ja2Settings
	{
#region Enums
		/// <summary>
		/// Window mode type.
		/// </summary>
		public enum WindowMode
		{
			Fullscreen = 0,
			Windowed = 1,
		}
#endregion

#region Properties
		/// <summary>
		/// Is sound enabled.
		/// </summary>
		public static bool isSoundEnabled { get; set; }

		/// <summary>
		/// Window mode.
		/// </summary>
		public static WindowMode windowMode { get; set; }

		/// <summary>
		/// Whatever the window mode was set from command line. If set TRUE, INI is no longer evaluated.
		/// </summary>
		public static bool cmdWindowMode { get; set; }
#endregion
	}
}
