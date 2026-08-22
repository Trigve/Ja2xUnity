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
		/// Main menu music component.
		/// </summary>
		[SerializeField]
		private AudioSource? m_Music;

		/// <summary>
		/// Main menu view.
		/// </summary>
		[SerializeField]
		private UI.View.ViewMainMenu? m_MainMenuView;
#endregion

#region Fields
		/// <summary>
		/// Main menu model.
		/// </summary>
		private ModelMainMenu? m_MainMenuModel;
#endregion

#region Messages
		public void Start()
		{
			m_MainMenuModel = new ModelMainMenu();

			// UI initalization
			m_MainMenuView?.Initialize(
				new UI.ViewModel.ViewModelMainMenu(m_MainMenuModel)
			);

			m_GameState.eventUpdate += OnUpdate;

			m_AssetRefMocker!.LoadAssets(m_GameState.assetManager);

			m_Music!.Play();
		}

		public void OnDestroy()
		{
			m_GameState.eventUpdate -= OnUpdate;

			m_MainMenuView?.Deinitialize();
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
