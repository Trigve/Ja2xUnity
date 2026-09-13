using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main menu screen manager.
	/// </summary>
	public sealed class ScreenMainMenuManager :  MonoBehaviour, IModelMainMenu
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

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

#region Messages
		public void Start()
		{
			m_GameState.cursorManager.ChangeCursor(CursorType.Generic);
			m_GameState.cursorManager.ShowCursor();

			// UI initalization
			m_MainMenuView?.Initialize(
				new UI.ViewModel.ViewModelMainMenu(this)
			);

			m_GameState.eventUpdate += OnUpdate;

			m_AssetRefMocker!.LoadAssets(m_GameState.assetManager);

			// Start the main menu music, if not already started
			m_GameState.soundManager.AddMusicSource(m_Music!);
		}

		public void OnDestroy()
		{
			m_GameState.eventUpdate -= OnUpdate;

			m_MainMenuView?.Deinitialize();
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

#region Slots
		/// <summary>
		/// Update is called on each frame.
		/// </summary>
		private void OnUpdate()
		{

		}
#endregion
	}
}
