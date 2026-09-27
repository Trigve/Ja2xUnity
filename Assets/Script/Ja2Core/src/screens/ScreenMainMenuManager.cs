using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main menu screen manager.
	/// </summary>
	public sealed class ScreenMainMenuManager :  SceneManagerSingleton, IModelMainMenu
	{
#region Fields Component
		/// <summary>
		/// Asset ref mocker.
		/// </summary>
		[SerializeField]
		private AssetRefMockerManager? m_AssetRefMocker;

		/// <summary>
		/// Main menu music component.
		/// </summary>
		[SerializeField]
		private AudioSource? m_Music;

		/// <summary>
		/// Main menu view.
		/// </summary>
		[SerializeField]
		private UI.View.ViewMainMenu? m_MainMenuView;

		/// <summary>
		/// Credits screen.
		/// </summary>
		[SerializeField]
		private GameScreen? m_CreditsScreen;
#endregion

#region Properties
		/// <inheritdoc/>
		public override IAssetRefMockRegistry assetRefMockRegistry => m_AssetRefMocker!;
#endregion

#region Messages
		public void Start()
		{
			m_GameState.cursorManager.ChangeCursor(CursorType.Generic);
			m_GameState.cursorManager.ShowCursor();

			// UI initalization
			m_MainMenuView?.Initialize(
				new UI.ViewModel.ViewModelMainMenu(this)
			);

			m_AssetRefMocker!.LoadAssets();

			// Start the main menu music, if not already started
			BootsrapManager.instance.soundManager.AddMusicSource(m_Music!);
		}

		public void Update()
		{
			// New game
			if(m_GameState.inputManager.IsKeyDown(KeyCode.N))
				StartNewGame();
			// Saved game
			else if(m_GameState.inputManager.IsKeyDown(KeyCode.C))
				ContinueSaveGame();
			// Preferences
			else if(m_GameState.inputManager.IsKeyDown(KeyCode.O))
				ShowPreferences();
			// Credits
			else if(m_GameState.inputManager.IsKeyDown(KeyCode.S))
				ShowCredits();
			// Quit game
			else if(m_GameState.inputManager.IsKeyDown(KeyCode.Q))
				Quit();
		}
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void StartNewGame()
		{
		}

		/// <inheritdoc/>
		public void ContinueSaveGame()
		{
		}

		/// <inheritdoc/>
		public void ShowPreferences()
		{
		}

		/// <inheritdoc/>
		public void ShowCredits()
		{
			// Start the credits screen
			m_GameState.screenManager.SetPendingScreen(m_CreditsScreen!,
				new GameScreenOptions()
				{
					destroyActiveSceen = true
				}
			);
		}

		/// <inheritdoc/>
		public void Quit()
		{
			Application.Quit();
		}
#endregion

#region Methods Private
		/// <inheritdoc/>
		protected override void DoAwake()
		{
			m_AssetRefMocker!.Initialize(m_GameState.assetManager);
		}

		/// <inheritdoc/>
		protected override void DoOnDestroy()
		{
			m_MainMenuView?.Deinitialize();
		}
#endregion
	}
}
