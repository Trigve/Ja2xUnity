using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Ja2
{
	/// <summary>
	/// Screen manager.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Screen Manager")]
	public sealed class ScreenManager : ScriptableObjectManager<ScreenManager>
	{
#region Fields Component
		/// See <see cref="introScreen"/>.
		[SerializeField]
		private GameScreen m_IntroScreen = null!;
#endregion

#region Fields
		/// <summary>
		/// Set/get current screen.
		/// </summary>
		[HistoricName("guiCurrentScreen")]
		private GameScreenData? m_CurrentScreen;

		/// <summary>
		/// Previous screen, if any.
		/// </summary>
		[HistoricName("guiPreviousScreen")]
		private GameScreenData? m_PreviousScreen;

		/// <summary>
		/// Pending screen, if any.
		/// </summary>
		[HistoricName("guiPendingScreen")]
		private GameScreenData? m_PendingScreen;

		/// <summary>
		/// Current task for changing the screens.
		/// </summary>
		private UniTaskVoid? m_Task;

		/// <summary>
		/// Cancelation token.
		/// </summary>
		private CancellationToken m_CancellationToken;
#endregion

#region Properties
		/// <summary>
		/// Intro screen.
		/// </summary>
		public GameScreen introScreen => m_IntroScreen;
#endregion

#region Events
		/// <summary>
		/// Event called after the screen has been loaded.
		/// </summary>
		public event Action? eventScreenLoaded;
#endregion

#region Methods Public
		/// <summary>
		/// Set the new pending screen.
		/// </summary>
		/// <param name="Screen">New screen to set.</param>
		/// <param name="Options">Screen options.</param>
		internal void SetPendingScreen(GameScreen Screen, GameScreenOptions Options = new ())
		{
			m_PendingScreen = new GameScreenData(Screen,
				Options
			);
		}

		/// <summary>
		/// Update the screen manager.
		/// </summary>
		public void UpdateManager()
		{
			// Task already in progress
			if(m_Task.HasValue)
				return;

			var old_screen = m_CurrentScreen;

			// Is there any pending screen.
			if(m_PendingScreen != null)
			{
				// Based on active screen, deinit!
				if(m_PendingScreen != m_CurrentScreen)
				{
//x					switch(currentScreen)
//x					{
//x					case MAP_SCREEN:
//x						if( guiPendingScreen != MSG_BOX_SCREEN && guiPendingScreen != MP_CHAT_SCREEN )
//x						{
//x							EndMapScreen( FALSE );
//x						}
//x						break;
//x					case LAPTOP_SCREEN:
//x						ExitLaptop();
//x						break;
//x					}
				}

				// Screen is changing
				if(old_screen != m_PendingScreen)
				{
					// Set the fact that the screen has changed
					old_screen = m_PendingScreen;

					HandleNewScreenChange(m_PendingScreen.Value,
						m_CurrentScreen
					);
				}
				m_PreviousScreen = m_CurrentScreen;
				m_CurrentScreen = m_PendingScreen;
				m_PendingScreen = null;

				// Start task for changing the screen
				m_Task = ChangeScreenAsync(m_CurrentScreen.Value);
				m_Task.Value.Forget();
			}


			// Handle the screen update
//x			old_screen = m_AllScreens[currentScreen].Update();

			// if the screen has changed
//x			if(old_screen != m_CurrentScreen)
//x			{
//x				HandleNewScreenChange(old_screen,
//x					m_CurrentScreen
//x				);
//x
//x				m_PreviousScreen = m_CurrentScreen;
//x				m_CurrentScreen = old_screen;
//x			}
		}
#endregion

#region Methods Private
		/// <summary>
		/// Handle the screen change.
		/// </summary>
		/// <param name="NewScreen">New screen to be shown.</param>
		/// <param name="OldScreen">Old screen.</param>
		private void HandleNewScreenChange(GameScreenData NewScreen, GameScreenData? OldScreen)
		{
/*
			//if we are not going into the message box screen, and we didnt just come from it
			if( ( uiNewScreen != MSG_BOX_SCREEN && uiOldScreen != MSG_BOX_SCREEN && uiNewScreen != MP_CHAT_SCREEN && uiOldScreen != MP_CHAT_SCREEN ) )
			{
				//reset the help screen
				NewScreenSoResetHelpScreen( );
			}

			//rain
			if( uiNewScreen == MAP_SCREEN )
			{
				if ( guiRainLoop != NO_SAMPLE )
				{
					SoundStop( guiRainLoop );
					guiRainLoop = NO_SAMPLE;
				}
			}
			// end rain

			// sevenfm: start/stop SSA
			if (uiNewScreen == GAME_SCREEN)
			{
				// check that no sound is playing currently
				if (guiCurrentSteadyStateSoundHandle == NO_SAMPLE)
				{
					SetSSA();
				}
			}
			else if (uiNewScreen != MSG_BOX_SCREEN)
			{
				// Stop SSA
				if (guiCurrentSteadyStateSoundHandle != NO_SAMPLE)
				{
					SoundStop(guiCurrentSteadyStateSoundHandle);
					guiCurrentSteadyStateSoundHandle = NO_SAMPLE;
				}
				// stop ambients
				StopFireAmbient();
			}
*/
		}

		/// <summary>
		/// Change the screen.
		/// </summary>
		/// <param name="NewScreen">New screen.</param>
		private async UniTaskVoid ChangeScreenAsync(GameScreenData NewScreen)
		{
			Scene current_scene = SceneManager.GetActiveScene();

			// Load the new scene
			await SceneManager.LoadSceneAsync(NewScreen.gameScreen.name,
				LoadSceneMode.Additive
			).WithCancellation(m_CancellationToken);

			// \FIXME
			// This is necessary so the Start() method would be called on the new scene
			await UniTask.NextFrame(m_CancellationToken);
			await UniTask.NextFrame(m_CancellationToken);

			// Activate the new scene
			SceneManager.SetActiveScene(
				SceneManager.GetSceneByName(NewScreen.gameScreen.name)
			);

			// Need to destroy the old scene
			if(NewScreen.options.destroyActiveSceen)
				await SceneManager.UnloadSceneAsync(current_scene);

			// Screen loading is done
			eventScreenLoaded?.Invoke();

			m_Task = null;
		}
#endregion

#region Construction
		/// <inheritdoc />
		protected override void DoInitialize(params object[] Params)
		{
			Assert.IsTrue(Params.Length == 1);
			Assert.IsTrue(Params[0] is CancellationToken);

			m_CancellationToken = (CancellationToken)Params[0];

			m_CurrentScreen = m_PendingScreen = m_PreviousScreen = null;
		}

		/// <inheritdoc />
		protected override void DoDeinitialize()
		{
			eventScreenLoaded = null;
			m_Task = null;
		}
#endregion
	}
}
