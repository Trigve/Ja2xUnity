using System;
using System.IO;
using System.Linq;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// This is the main manager class, that is always present and is responsible for the initialization of the game.
	/// </summary>
	public sealed class BootsrapManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state singleton.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

		/// <summary>
		/// Init screen to run.
		/// </summary>
		[SerializeField]
		private GameScreen? m_InitScreen;
#endregion

#region Messages
		public void Start()
		{
			Ja2Logger.LogInfo("BootsrapManager Start");

			ProcessJa2CommandLineBeforeInitialization();

			// Inititialize the SGP
			if(!InitializeStandardGamingPlatform())
			{
				// Ffailed to initialize the SGP
				Application.Quit(-1);
			}

			// Next screen to run
			GameScreen? next_screen = m_InitScreen;

#if UNITY_EDITOR
			// Load the active scene before playback started. This is for situation when another
			// scene is being tested but bootstrap scene need to be run first (check against loading the bootstrap scene
			// if it is the active scene)
			if(m_GameState.originalScene is not null && m_GameState.originalScene.name != UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name)
				next_screen = m_GameState.originalScene;
#endif

			if(next_screen != null)
				m_GameState.screenManager.SetPendingScreen(next_screen);
		}
#endregion

#region Methods Private
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
			using var stream_reader = new StreamReader(
				new FileStream(
					Path.Combine(Ja2Settings.userDataPath,
						Constants.GameIniFile
					),
					FileMode.Open
				)
			);
			// Read in settings
			var oProps = new IniFile(stream_reader);

			string loc = oProps.GetStringProperty(Constants.IniSectionJa2Settings,
				Constants.IniKeyLocale,
				string.Empty
			)!;
			if(loc.Length > 0)
			{
			}

			long iResolution = oProps.GetIntProperty(Constants.IniSectionJa2Settings,
				Constants.IniKeyScreenResolution,
				-1
			)!.Value;

			// Is windowed mdoe
			if(oProps.GetIntProperty(Constants.IniSectionJa2Settings, Constants.IniKeyScreenModeWindowed, -1) == 1)
				Ja2Settings.windowMode = Ja2Settings.WindowMode.Windowed;

			// Window mode should be maximized
			Ja2Settings.isWindowedModeMaximized = oProps.GetIntProperty(Constants.IniSectionJa2Settings,
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
					(int)oProps.GetIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyScreenResolutionX,
						-1
					)!.Value,
					1920
				);
				res_y = Math.Max(
					(int)oProps.GetIntProperty(Constants.IniSectionJa2Settings,
						Constants.IniKeyScreenResolutionY,
						-1
					)!.Value,
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

			Ja2Settings.playIntro = oProps.GetIntProperty(Constants.IniSectionJa2Settings,
				Constants.IniKeyPlayIntro,
				1
			) == 1;

			float fTooltipScaleFactor = ((float)oProps.GetFloatProperty(Constants.IniSectionJa2Settings,
					Constants.IniKeyTooltipScaleFactor,
					100)!.Value
				) / 100;
			if(fTooltipScaleFactor < 1)
				fTooltipScaleFactor = 1;

			Ja2Settings.tooltipScaleFactor = fTooltipScaleFactor;

			Ja2Settings.disableMouseScroll = oProps.GetIntProperty(Constants.IniSectionJa2Settings,
				Constants.IniKeyDisableMouseScrolling,
				0
			) == 1;

			Ja2Logger.LogInfo("Initializing Game Manager");

			// Initialize the Game
			if(!InitGame())
			{
				// We were unable to initialize the game
				Ja2Logger.LogWarning("FAILED : Initializing Game Manager");

				return false;
			}

			return true;
		}

		/// <summary>
		/// Initialize the game stuff.
		/// </summary>
		/// <returns></returns>
		[HistoricName("InitializeGame")]
		private bool InitGame()
		{

			return true;
		}
#endregion
	}
}
