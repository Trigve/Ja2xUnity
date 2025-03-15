namespace Ja2
{
	/// <summary>
	/// Various constants.
	/// </summary>
	internal static class Constants
	{
#region Builtin
		/// <summary>
		/// CR.
		/// </summary>
		public const char CarriageReturn = '\r';

		/// <summary>
		/// LF.
		/// </summary>
		public const char LineFeed = '\n';

		/// <summary>
		/// Empty string as constant.
		/// </summary>
		public const string StringEmpty = "";
#endregion

#region File Names
		/// <summary>
		/// Game .ini file.
		/// </summary>
		public const string GameIniFile = "Ja2.ini";
#endregion

#region Ini Sections
		/// <summary>
		/// Ja2 main settings.
		/// </summary>
		public const string IniSectionJa2Settings = "Ja2 Setting";
#endregion

#region Ini Keys
		/// <summary>
		/// Locale name.
		/// </summary>
		public const string IniKeyLocale = "LOCALE";

		/// <summary>
		/// Screen resolution preset.
		/// </summary>
		public const string IniKeyScreenResolution = "SCREEN_RESOLUTION";

		/// <summary>
		/// Should use windowed mode.
		/// </summary>
		public const string IniKeyScreenModeWindowed = "SCREEN_MODE_WINDOWED";

		/// <summary>
		/// Should the window be maximized.
		/// </summary>
		public const string IniKeyScreenModeWindowedMaximized = "SCREEN_MODE_WINDOWED_MAXIMIZE";

		/// <summary>
		/// Custom screen resolution - X.
		/// </summary>
		public const string IniKeyScreenResolutionX = "CUSTOM_SCREEN_RESOLUTION_X";

		/// <summary>
		/// Custom screen resolution - Y.
		/// </summary>
		public const string IniKeyScreenResolutionY = "CUSTOM_SCREEN_RESOLUTION_Y";

		/// <summary>
		/// Play intro.
		/// </summary>
		public const string IniKeyPlayIntro = "PLAY_INTRO";

		/// <summary>
		/// Tooltip scale factor.
		/// </summary>
		public const string IniKeyTooltipScaleFactor = "TOOLTIP_SCALE_FACTOR";

		/// <summary>
		/// Disable mouse scrolling.
		/// </summary>
		public const string IniKeyDisableMouseScrolling = "DISABLE_MOUSE_SCROLLING";
#endregion
	}
}
