using System;

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
		/// Path for saving/loading various data.
		/// </summary>
		public static string userDataPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Ja2";

		/// <summary>
		/// Is sound enabled.
		/// </summary>
		public static bool isSoundEnabled { get; set; }

		/// <summary>
		/// Window mode.
		/// </summary>
		public static WindowMode windowMode { get; set; }

		/// <summary>
		/// Should the window be maximized.
		/// </summary>
		public static bool isWindowedModeMaximized { get; set; }

		/// <summary>
		/// Whatever the window mode was set from command line. If set TRUE, INI is no longer evaluated.
		/// </summary>
		public static bool cmdWindowMode { get; set; }

		/// <summary>
		/// Screen width resolution.
		/// </summary>
		public static int screenWidth { get; set; }

		/// <summary>
		/// Screen height resolution.
		/// </summary>
		public static int screenHeight { get; set; }

		/// <summary>
		/// Should the intro be played.
		/// </summary>
		public static bool playIntro { get; set; }

		/// <summary>
		/// Tooltip scale factor.
		/// </summary>
		public static float tooltipScaleFactor {get; set;}

		/// <summary>
		/// Disable scrolling with mouse.
		/// </summary>
		public static bool disableMouseScroll { get; set; }
#endregion
	}
}
