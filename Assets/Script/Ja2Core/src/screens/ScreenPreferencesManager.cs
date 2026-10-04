using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Screen manager for the preferences screen.
	/// </summary>
	public sealed class ScreenPreferencesManager : SceneManagerSingleton
	{
#region Fields Component
		/// <summary>
		/// Main menu view.
		/// </summary>
		[SerializeField]
		private UI.View.ViewPreferences? m_GameSettingsView;

		/// <summary>
		/// View for the preferences items.
		/// </summary>
		[SerializeField]
		private UI.View.ViewPreferencesAllItems? m_PreferencesItemsView;
#endregion

#region Fields
		/// <summary>
		/// Model used.
		/// </summary>
		private UI.Models.ModelPreferences m_ModelPreferences = null!;
#endregion

#region Messages
		public void Start()
		{
			// Load all the assets
			BootsrapManager.instance.assetRefMockerManager!.StopBatchMode(true);

			m_ModelPreferences = new UI.Models.ModelPreferences(m_GameState);
			m_ModelPreferences.eventDone += OnPreferencesDone;

			// UI initalization
			m_GameSettingsView!.Initialize(
				new UI.ViewModel.ViewModelPreferences(m_ModelPreferences)
			);
			//
			m_PreferencesItemsView!.Initialize(
				new UI.ViewModel.ViewModelPreferencesAllItems(m_ModelPreferences.gameSettings)
			);
		}
#endregion

#region Methods Private
		/// <inheritdoc/>
		protected override void DoAwake()
		{
			BootsrapManager.instance.assetRefMockerManager!.StartBatchMode();
		}

		/// <inheritdoc/>
		protected override void DoOnDestroy()
		{
			m_GameSettingsView?.Deinitialize();
			m_PreferencesItemsView?.Deinitialize();
		}
#endregion

#region Slots
		/// <summary>
		/// Slot for the event, when the preferences screen is done.
		/// </summary>
		/// <param name="Sender">Sender object.</param>
		/// <param name="Event">Event.</param>
		private void OnPreferencesDone(object Sender, EventArgs Event)
		{
			// Return to the previous screen
			m_GameState.screenManager.SetPreviousScreen(
				new GameScreenOptions()
				{
					destroyActiveSceen = true
				}
			);
		}
#endregion
	}
}
