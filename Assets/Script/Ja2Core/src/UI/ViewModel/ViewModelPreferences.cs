using System;
using System.Threading;

using UnityEngine;

using Aspid.MVVM;

using Cysharp.Threading.Tasks;

namespace Ja2.UI.ViewModel
{
	/// <summary>
	/// ViewModel for the "preferences" screen.
	/// </summary>
	[ViewModel]
	internal sealed partial class ViewModelPreferences : IDisposable
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
		/// Quit command.
		/// </summary>
		[Bind]
		private IRelayCommand m_CommandQuit;

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

		/// <summary>
		/// Message dialog prefab.
		/// </summary>
		private readonly GameObject m_MessageDialogPrefab;
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void Dispose()
		{
			// Dispose async command so it is stopped
			((RelayCommandAsync)CommandQuit).Dispose();
		}
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
		/// <returns>Value in the linear range.</returns>
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
#endregion

#region Slots
		/// <summary>
		/// "Quit" command handler.
		/// </summary>
		private async UniTask OnQuit(CancellationToken Token)
		{
			DialogManager dialog_manager = BootstrapManager.instance.dialogManager;

			// Create the dialog
			DialogHandle dialog = dialog_manager.CreateMessageDialog(m_MessageDialogPrefab);
			// \FIXME Text shouldn't be hardoced
			dialog.m_Dialog.text = "Quit game and return to the main menu?";

			// Show the dialog
			MessageDialogComponent.ButtonType ret = await dialog.m_Dialog.ShowAsync(MessageDialogComponent.ButtonType.ButtonYes | MessageDialogComponent.ButtonType.ButtonNo,
				Token
			);

			if(ret == MessageDialogComponent.ButtonType.ButtonYes)
				m_Model.Quit();
		}
#endregion

#region Constructor
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Model">Model used.</param>
		/// <param name="MessageDialogPrefab">Message dialog prefab.</param>
		public ViewModelPreferences(Models.ModelPreferences Model, GameObject MessageDialogPrefab)
		{
			m_Model = Model;
			m_MessageDialogPrefab = MessageDialogPrefab;

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

			m_CommandQuit = new RelayCommandAsync(OnQuit);

			m_CommandDone = new RelayCommand(Model.Done);
		}
#endregion
	}
}
