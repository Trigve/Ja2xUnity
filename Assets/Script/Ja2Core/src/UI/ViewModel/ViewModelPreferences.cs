using Aspid.MVVM;

namespace Ja2.UI.ViewModel
{
	/// <summary>
	/// ViewModel for the preferences screen.
	/// </summary>
	[ViewModel]
	internal sealed partial class ViewModelPreferences
	{
#region Fields View
		/// <summary>
		/// Sound effect volume.
		/// </summary>
		[Bind]
		private float m_SoundEffectsVolume;

		/// <summary>
		/// Speech volume.
		/// </summary>
		[Bind]
		private float m_SpeechVolume;

		/// <summary>
		/// Music volume.
		/// </summary>
		[Bind]
		private float m_MusicVolume;

		/// <summary>
		/// Save game command.
		/// </summary>
		[Bind]
		private IRelayCommand m_CommandSaveGame;

		/// <summary>
		/// Load game command.
		/// </summary>
		[Bind]
		private IRelayCommand m_CommandLoadGame;

		/// <summary>
		/// Done command.
		/// </summary>
		[Bind]
		private IRelayCommand m_CommandDone;
#endregion

#region Fields
		/// <summary>
		/// Model used.
		/// </summary>
		private readonly Models.ModelPreferences m_Model;
#endregion

#region Methods Private
		/// <summary>
		/// Called when changing the value.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnSoundEffectsVolumeChanged(float newValue)
		{
			m_Model.gameSettings.SetValue(GameSettingOptionData.OptionType.OptionSoundEffectsVolume,
				in newValue
			);
		}

		/// <summary>
		/// Called when changing the value.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnSpeechVolumeChanged(float newValue)
		{
			m_Model.gameSettings.SetValue(GameSettingOptionData.OptionType.OptionSpeechVolume,
				in newValue
			);
		}

		/// <summary>
		/// Called when changing the value.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnMusicVolumeChanged(float newValue)
		{
			m_Model.gameSettings.SetValue(GameSettingOptionData.OptionType.OptionMusicVolume,
				in newValue
			);
		}
#endregion

#region Constructor
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Model">Model used.</param>
		public ViewModelPreferences(Models.ModelPreferences Model)
		{
			m_Model = Model;

			// Initial values
			SoundEffectsVolume = m_Model.gameSettings.GetValue<float>(GameSettingOptionData.OptionType.OptionSoundEffectsVolume);
			SpeechVolume = m_Model.gameSettings.GetValue<float>(GameSettingOptionData.OptionType.OptionSpeechVolume);
			MusicVolume = m_Model.gameSettings.GetValue<float>(GameSettingOptionData.OptionType.OptionMusicVolume);

			m_CommandSaveGame = new RelayCommand(Model.SaveGame,
				Model.canSaveGame
			);
			m_CommandLoadGame = new RelayCommand(Model.LoadGame);
			m_CommandDone = new RelayCommand(Model.Done);
		}
#endregion
	}
}
