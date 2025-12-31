using Cysharp.Threading.Tasks;

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

		/// <summary>
		/// Camera used.
		/// </summary>
		[SerializeField]
		private Camera? m_Camera;
#endregion

#region Messages
		public async UniTaskVoid Start()
		{
			// Disable old camera and set the active on
			m_GameState.activeCamera = m_Camera;

			await m_AssetRefMocker!.LoadAssets(m_GameState.assetManager);
		}
#endregion
	}
}
