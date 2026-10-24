using UnityEngine;

using Aspid.MVVM;

namespace Ja2.UI.View
{
	/// <summary>
	/// View for the preferences screen.
	/// </summary>
	[View]
	public sealed partial class ViewPreferences : MonoView
	{
#region Fields Component
		/// <summary>
		/// Sound effect slider binder.
		/// </summary>
		[RequireBinder(typeof(float))]
		[SerializeField]
		private MonoBinder[]? m_SoundEffectsVolume;

		/// <summary>
		/// Command for the sound effect volume change.
		/// </summary>
		[RequireBinder(typeof(RelayCommand<float>))]
		[SerializeField]
		private MonoBinder[]? m_SoundEffectsVolumeCommand;

		/// <summary>
		/// Speech volume slider binder.
		/// </summary>
		[RequireBinder(typeof(float))]
		[SerializeField]
		private MonoBinder[]? m_SpeechVolume;

		/// <summary>
		/// Command for the speech volume change.
		/// </summary>
		[RequireBinder(typeof(RelayCommand<float>))]
		[SerializeField]
		private MonoBinder[]? m_SpeechVolumeCommand;

		/// <summary>
		/// Music volume slider binder.
		/// </summary>
		[RequireBinder(typeof(float))]
		[SerializeField]
		private MonoBinder[]? m_MusicVolume;

		/// <summary>
		/// Save game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandSaveGame;

		/// <summary>
		/// Load game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandLoadGame;

		/// <summary>
		/// Quit command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandQuit;

		/// <summary>
		/// Load game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandDone;
#endregion
	}
}
