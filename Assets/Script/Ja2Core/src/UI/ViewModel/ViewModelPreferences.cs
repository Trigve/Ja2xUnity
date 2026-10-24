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
				ConvertValueToDecibel(GameSettingOptionData.OptionType.OptionSoundEffectsVolume,
					newValue
				)
			);
		}

		/// <summary>
		/// Called when changing the value.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnSpeechVolumeChanged(float newValue)
		{
			m_Model.gameSettings.SetValue(GameSettingOptionData.OptionType.OptionSpeechVolume,
				ConvertValueToDecibel(GameSettingOptionData.OptionType.OptionSpeechVolume,
					newValue
				)
			);
		}

		/// <summary>
		/// Called when changing the value.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnMusicVolumeChanged(float newValue)
		{
			m_Model.gameSettings.SetValue(GameSettingOptionData.OptionType.OptionMusicVolume,
				ConvertValueToDecibel(GameSettingOptionData.OptionType.OptionMusicVolume,
					newValue
				)
			);
		}

		/// <summary>
		/// Convert the value to the linear range.
		/// </summary>
		/// <param name="Option">Option for which data are used.</param>
		/// <returns>Value in linear range.</returns>
		private float ConvertValueToLinear(GameSettingOptionData.OptionType Option)
		{

			// Range of the option
			(float min, float max) = m_Model.gameSettings[Option].GetMinMaxValues<float>();

			return SoundManager.DecibelRangeToLinear(m_Model.gameSettings.GetValue<float>(Option),
				min,
				max
			);
		}

		/// <summary>
		/// Convert the value to the dB range.
		/// </summary>
		/// <param name="Option">Option form which data are used.</param>
		/// <param name="Value">Value.</param>
		/// <returns>Value in the dB range.</returns>
		private float ConvertValueToDecibel(GameSettingOptionData.OptionType Option, float Value)
		{
			// Range of the option
			(float min, float max) = m_Model.gameSettings[Option].GetMinMaxValues<float>();

			return SoundManager.LinearRangeToDecibel(Value,
				min,
				max
			);
		}
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
			SoundEffectsVolume = ConvertValueToLinear(GameSettingOptionData.OptionType.OptionSoundEffectsVolume);
			SpeechVolume = ConvertValueToLinear(GameSettingOptionData.OptionType.OptionSpeechVolume);
			MusicVolume = ConvertValueToLinear(GameSettingOptionData.OptionType.OptionMusicVolume);

			m_CommandSaveGame = new RelayCommand(Model.SaveGame,
				Model.canSaveGame
			);
			m_CommandLoadGame = new RelayCommand(Model.LoadGame,
				Model.canLoadGame
			);
			m_CommandDone = new RelayCommand(Model.Done);
		}
#endregion
	}
}
