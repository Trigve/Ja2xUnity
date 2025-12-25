using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Init screen game manager.
	/// </summary>
	[HistoricName("INIT_SCREEN")]
	public sealed class ScreenInitManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

		/// <summary>
		/// Intro screen to run.
		/// </summary>
		[SerializeField]
		private GameScreen? m_IntroScreen;
#endregion

#region Messages
		public void Start()
		{
			// Start intro
			m_GameState.screenManager.SetPendingScreen(m_IntroScreen,
				new GameScreenOptions()
				{
					destroyActiveSceen = true
				}
			);
		}
#endregion
	}
}
