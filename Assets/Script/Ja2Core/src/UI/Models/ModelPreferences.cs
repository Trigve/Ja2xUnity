using System;

namespace Ja2.UI.Models
{
	/// <summary>
	/// Model for the preferences view.
	/// </summary>
	internal sealed class ModelPreferences
	{
#region Fields
		/// <summary>
		/// Game state instance.
		/// </summary>
		private readonly GameState m_GameState;

		/// <summary>
		/// Screen which gets loaded on quit.
		/// </summary>
		private GameScreen m_Quitcreen;
#endregion

#region Properties
		/// <summary>
		/// Game settings used.
		/// </summary>
		public GameSettings gameSettings { get; }

		// \FIXME Needs implementation
		/// <summary>
		/// Can save game.
		/// </summary>
		public Func<bool> canSaveGame => () => false;

		// \FIXME Needs implementation
		/// <summary>
		/// Can load the game.
		/// </summary>
		public Func<bool> canLoadGame => () => false;
#endregion

#region Events
		/// <summary>
		/// Event for save game action.
		/// </summary>
		public event EventHandler? eventSaveGame;

		/// <summary>
		/// Event for load game action.
		/// </summary>
		public event EventHandler? eventLoadGame;

		/// <summary>
		/// Event for "done" action.
		/// </summary>
		public event EventHandler? eventDone;
#endregion

#region Methods
		/// <summary>
		/// Save game handler
		/// </summary>
		public void SaveGame()
		{
			eventSaveGame?.Invoke(this,
				EventArgs.Empty
			);
		}

		/// <summary>
		/// Load game hander.
		/// </summary>
		public void LoadGame()
		{
			eventLoadGame?.Invoke(this,
				EventArgs.Empty
			);
		}

		/// <summary>
		/// Save and close the preferences.
		/// </summary>
		public void Done()
		{
			// Save the game settings
			m_GameState.gameSettings = gameSettings;
			m_GameState.SaveSettings();

			eventDone?.Invoke(this,
				EventArgs.Empty
			);
		}

		/// <summary>
		/// Quit the "preferences" screen.
		/// </summary>
		public void Quit()
		{
			m_GameState.screenManager.SetPendingScreen(m_Quitcreen,
				new GameScreenOptions()
				{
					destroyActiveSceen = true
				}
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="State">Game state.</param>
		/// <param name="Quitcreen">Quitting screen.</param>
		public ModelPreferences(GameState State, GameScreen Quitcreen)
		{
			m_GameState = State;
			m_Quitcreen = Quitcreen;

			// Get copy of the settings
			gameSettings = new GameSettings(State.gameSettings);
		}
#endregion
	}
}
