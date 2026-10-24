using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Game settings data asset.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Game Settings")]
	public sealed class AssetGameSettingsData : AssetBase
	{
#region Fields
		/// <summary>
		/// List of all options.
		/// </summary>
		[SerializeField]
		private GameSettingOptionData[] m_Options = Array.Empty<GameSettingOptionData>();

		/// <summary>
		/// The group of options, in which at least 1 one option needs to be enabled.
		/// </summary>
		[Tooltip("The group of options, in which at least 1 one option needs to be enabled.")]
		[SerializeField]
		private GameSettingOptionGroup[] m_OptionsNeededGroup = Array.Empty<GameSettingOptionGroup>();
#endregion

#region Properties
		/// <summary>
		/// Gett all the options.
		/// </summary>
		public IEnumerable<GameSettingOptionData> options => m_Options;
#endregion

#region Methods Public
		/// <summary>
		/// Get all the groups, in which the <paramref name="Option"/> figures.
		/// </summary>
		/// <param name="Option">Option to check on.</param>
		/// <returns>All option groups, in which the option is in.</returns>
		public IEnumerable<GameSettingOptionGroup> GetOptionsNeededGroup(GameSettingOptionData.OptionType Option)
		{
			// Process all groups and don't exit early if found, because more than 1 group could contain the
			// option in combination with other options
			foreach(GameSettingOptionGroup it in m_OptionsNeededGroup)
			{
				// If option is in the given group, returns it
				if(it.m_OptionsNeeded.Contains(Option))
					yield return it;
			}
		}
#endregion
	}

	/// <summary>
	/// Settings option definition.
	/// </summary>
	[Serializable]
	public sealed class GameSettingOptionData
	{
#region Enums
		/// <summary>
		/// Option type.
		/// </summary>
		public enum OptionType
		{
			OptionNone = 0,

			/// <summary>
			/// Sound effects volume
			/// </summary>
			OptionSoundEffectsVolume = 1,

			/// <summary>
			/// Speech effects volume
			/// </summary>
			OptionSpeechVolume = 2,

			/// <summary>
			/// Music volume
			/// </summary>
			OptionMusicVolume = 3,

			SpeechOn = 4,
			MuteConfirmations = 5,
			SubTitles = 6,
			PauseTextDialog = 7,
			AnimateSmoke = 8,
			BloodNGore = 9,
			NeverMoveMouse = 10,
			OldSelectionMethod = 11,
			ShowMovementPath = 12,
			ShowMisses = 13,
			RealTimeConfirmation = 14,
			DisplaySleepWakeNotifications = 15,
			UseMetricSystem = 16,
			MercLightsDuringMove = 17,
			SnapCursorToMerc = 18,
			SnapCursorToDoors = 19,
			MakeItemsGlow = 20,
			ShowTreeTops = 21,
			ShowWireframes = 22,
			Show3DCursor = 23,
			ShowLightsUnderMerc = 24,
			HideBullets = 25,
			TrackingMode = 26,
		}

		/// <summary>
		/// Allowed value type.
		/// </summary>
		public enum ValueType
		{
			String = 0,
			Integer = 1,
			Float = 2,
			Boolean = 3,
			Double = 4,
			Short = 5,
			Long = 6,
		}

		/// <summary>
		/// Option flags.
		/// </summary>
		[Flags]
		public enum OptionFlag
		{
			None = 0,

			/// <summary>
			/// Show option in game setting
			/// </summary>
			ShowInSettings = 1 << 0,
		}
#endregion

#region Fields
		/// <summary>
		/// Section name in which the option is saved to.
		/// </summary>
		[SerializeField]
		public string m_SectionName = string.Empty;

		/// <summary>
		/// Option type.
		/// </summary>
		[SerializeField]
		public OptionType m_OptionType;

		/// <summary>
		/// All the name aliases for this option. The first one is used for saving.
		/// </summary>
		[SerializeField]
		public string[] m_Names =  Array.Empty<string>();

		/// <summary>
		/// Option text.
		/// </summary>
		[SerializeField]
		public string m_Text = string.Empty;

		/// <summary>
		/// Option description.
		/// </summary>
		[SerializeField]
		public string m_Description = string.Empty;

		/// <summary>
		/// Value type.
		/// </summary>
		[SerializeField]
		public ValueType m_ValueType;

		/// <summary>
		/// Default value.
		/// </summary>
		[Tooltip("Default value in in <min, max> range.")]
		[SerializeField]
		public string m_DefaultValue = string.Empty;

		/// <summary>
		/// Minimum value, if provided.
		/// </summary>
		[SerializeField]
		public string m_ValueMin = string.Empty;

		/// <summary>
		/// Maximum value, if provided.
		/// </summary>
		[SerializeField]
		public string m_ValueMax = string.Empty;

		/// <summary>
		/// Option flags.
		/// </summary>
		[SerializeField]
		public OptionFlag m_Flags = OptionFlag.ShowInSettings;
#endregion
	}

	/// <summary>
	/// Group of game setting options.
	/// </summary>
	[Serializable]
	public class GameSettingOptionGroup
	{
#region Fields Component
		/// <summary>
		/// Game setting option types.
		/// </summary>
		[SerializeField]
		public GameSettingOptionData.OptionType[] m_OptionsNeeded = Array.Empty<GameSettingOptionData.OptionType>();
#endregion
	}
}
