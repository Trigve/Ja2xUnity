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
#endregion
	}
}
