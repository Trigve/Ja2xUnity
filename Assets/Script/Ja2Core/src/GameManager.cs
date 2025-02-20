using System;
using System.Linq;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main entry class.
	/// </summary>
	public sealed class GameManager : MonoBehaviour
	{
#region Messages
		/// See unity.
		public void Start()
		{
			RandomManager.Init();

			ProcessJa2CommandLineBeforeInitialization();

			// Inititialize the SGP
			if(!InitializeStandardGamingPlatform())
			{
				// Ffailed to initialize the SGP
				Application.Quit(-1);
			}
		}
#endregion

#region Methods
		/// <summary>
		/// Process JA2 command line before initializtion is done.
		/// </summary>
		private void ProcessJa2CommandLineBeforeInitialization()
		{
			// Get all arguments
			foreach(string token_upper in Environment.GetCommandLineArgs().Select(Token => Token.ToUpperInvariant()))
			{
				// "NO SOUND" option
				if(token_upper == "/NOSOUND")
				{
					// Disable the sound
					Ja2Settings.isSoundEnabled = false;
				}
				else if(token_upper == "/FULLSCREEN")
				{
					// Overwrite Graphic setting from JA2_settings.ini
					Ja2Settings.windowMode = Ja2Settings.WindowMode.Fullscreen;
					Ja2Settings.cmdWindowMode = true;

					// no resolution read from Args. Still from INI, but could be added here, too...
				}
				else if(token_upper == "/WINDOW")
				{
					// Overwrite Graphic setting from JA2_settings.ini
					Ja2Settings.windowMode = Ja2Settings.WindowMode.Windowed;
					Ja2Settings.cmdWindowMode = true;
					// \TODO No resolution read from Args. Still from INI, but could be added here, too...
				}
			}
		}
		/// <summary>
		/// Initialize SGP.
		/// </summary>
		/// <returns></returns>
		private bool InitializeStandardGamingPlatform()
		{

			// Open the game config file
			Vfs.File file_game_ini = Vfs.VfsManager.OpenFileRegular(
				new Vfs.Path(Constants.GameIniFile)
			);
			if(file_game_ini.AsReadable(out Vfs.IFileReadable file_game_ini_stream))
			{
				using (file_game_ini_stream)
				{
					// Read in settings
					var oProps = new IniParser(file_game_ini_stream);


					string loc = oProps.getStringProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyLocale
					);
					if(loc.Length > 0)
					{
					}

					long iResolution = oProps.getIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyScreenResolution,
						-1
					);

					// Is windowed mdoe
					if(oProps.getIntProperty(Constants.IniSectionJa2Settings, Constants.IniKeyScreenModeWindowed, -1) == 1)
						Ja2Settings.windowMode = Ja2Settings.WindowMode.Windowed;

					// Window mode should be maximized
					Ja2Settings.isWindowedModeMaximized = oProps.getIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyScreenModeWindowedMaximized,
						-1
					) == 1;


					var res_x = 1920;
					var res_y = 1080;

					// \TODO Minimal resolution should be 1920x1080?
					switch(iResolution)
					{
					case 25:
						res_x = Mathf.Max(
							(int)oProps.getIntProperty(Constants.IniSectionJa2Settings,
								Constants.IniKeyScreenResolutionX,
								-1
							),
							1920
						);
						res_y = Math.Max(
							(int)oProps.getIntProperty(Constants.IniSectionJa2Settings,
								Constants.IniKeyScreenResolutionY,
								-1
							),
							1080
						);
						break;
					// 1920x1080
					default:
						res_x = 1920;
						res_y = 1080;
						break;
					}

					if(Ja2Settings.windowMode == Ja2Settings.WindowMode.Windowed && Ja2Settings.isWindowedModeMaximized)
					{
					}

					Ja2Settings.screenWidth = res_x;
					Ja2Settings.screenHeight = res_y;


					Ja2Settings.playIntro = oProps.getIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyPlayIntro,
						1
					) == 1;

					float fTooltipScaleFactor = ((float)oProps.getFloatProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyTooltipScaleFactor,
						100)
					) / 100;
					if(fTooltipScaleFactor < 1)
						fTooltipScaleFactor = 1;

					Ja2Settings.tooltipScaleFactor = fTooltipScaleFactor;

					Ja2Settings.disableMouseScroll = oProps.getIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyDisableMouseScrolling,
						0
					) == 1;

				}
			}
			return true;
		}
#endregion
	}
}
