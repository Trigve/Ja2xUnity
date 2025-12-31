using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main menu screen manager.
	/// </summary>
	public sealed class ScreenMainMenuManager :  MonoBehaviour
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
		private UI.AssetRefMockerManager? m_AssetRefMocker;
#endregion

#region Messages
		public void Start()
		{
			m_AssetRefMocker?.LoadAssets(m_GameState.assetManager);
		}
#endregion
	}
}
