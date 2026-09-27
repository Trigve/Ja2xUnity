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
		/// Speech volume slider binder.
		/// </summary>
		[RequireBinder(typeof(float))]
		[SerializeField]
		private MonoBinder[]? m_SpeechVolume;

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
		/// Load game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandDone;
#endregion
	}
}
