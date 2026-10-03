using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

namespace Ja2
{
	/// <summary>
	/// Sound manager.
	/// </summary>
	public sealed class SoundManager : MonoBehaviour
	{
#region Constants
		/// <summary>
		/// Music volume audio mixer parameter name.
		/// </summary>
		private const string ParameterMusicVolume = "MusicVolume";

		/// <summary>
		/// Speech volume audio mixer parameter name.
		/// </summary>
		private const string ParameterSpeechVolume = "SpeechVolume";

		/// <summary>
		/// Sfx volume audio mixer parameter name.
		/// </summary>
		private const string ParameterSfxVolume = "SfxVolume";
#endregion

#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState? m_GameState;

		/// <summary>
		/// Main audio mixer used.
		/// </summary>
		[SerializeField]
		private AudioMixer? m_AudioMixer;

		/// <summary>
		/// Music audio group in mixer.
		/// </summary>
		[SerializeField]
		private AudioMixerGroup? m_MusicAudioGroup;

		/// <summary>
		/// Speech audio group in mixer.
		/// </summary>
		[SerializeField]
		private AudioMixerGroup? m_SpeechAudioGroup;

		/// <summary>
		/// Sfx audio group in mixer.
		/// </summary>
		[SerializeField]
		private AudioMixerGroup? m_SfxAudioGroup;
#endregion

#region Fields
		/// <summary>
		/// Audio sources used for the music.
		/// </summary>
		private List<AudioSource>? m_MusicAudioSources;
#endregion

#region Methods Public
		/// <summary>
		/// Add and play music audio source.
		/// </summary>
		/// <param name="Source">Music audio source.</param>
		/// <param name="RestartIfPlaying">If true, the music will be restarted from playing. Otherwise, no audio source would be added.</param>
		public void AddMusicSource(AudioSource Source, bool RestartIfPlaying = false)
		{
			// Find if the same audio source exist
			AudioSource? audio_source = m_MusicAudioSources!.FirstOrDefault(Value => Value.clip == Source.clip);

			// Exist
			if(audio_source != null)
			{
				// Delete the original GO as it isn't needed anymore
				Destroy(Source.gameObject);

				// Should restart playing
				if(RestartIfPlaying)
					audio_source.Play();
			}
			// Doesn't exist
			else
			{
				m_MusicAudioSources!.Add(Source);
				// Parent it
				Source.transform.SetParent(transform);

				// Play the clip now
				Source.Play();
			}
		}
#endregion

#region Methods Private Static
		/// <summary>
		/// Convert dB value to linear.
		/// </summary>
		/// <param name="Value">dB value.</param>
		/// <returns>Linear value.</returns>
		private static float DecibelToLinear(float Value)
		{
			return Mathf.Pow(10f,
				Value / 20
			);
		}

		/// <summary>
		/// Convert linear value to dB.
		/// </summary>
		/// <param name="Value">Linear value.</param>
		/// <returns>db Value.</returns>
		private static float LinearToDecibel(float Value)
		{
			return 20f * Mathf.Log10(Value);
		}

		/// <summary>
		/// Convert linear value range to dB range.
		/// </summary>
		/// <param name="Value">Linear value to convert.</param>
		/// <param name="Min">dB range minimum value.</param>
		/// <param name="Max">dB range maximum value.</param>
		/// <returns>dB value in the <paramref name="Min"/>/<paramref name="Max"/> range.</returns>
		private static float LinearRangeToDecibel(float Value, float Min, float Max)
		{
			// Be sure it is still in the range
			Value = Mathf.Clamp01(Value);

			return LinearToDecibel(
				Mathf.Lerp(DecibelToLinear(Min),
					DecibelToLinear(Max),
					Value
				)
			);
		}

		/// <summary>
		/// Set the audio group volume.
		/// </summary>
		/// <param name="AudioGroup">Audio mixer group used to set the volume.</param>
		/// <param name="OptionValue">Option value.</param>
		/// <param name="Parameter">Parameter name of the volume.</param>
		private static void SetAudioGroupVolume(AudioMixerGroup AudioGroup, GameSettingOptionValue OptionValue, string Parameter)
		{
			(float min, float max) = OptionValue.GetMinMaxValues<float>();
			AudioGroup.audioMixer.SetFloat(Parameter,
				// Convert to the dB
				LinearRangeToDecibel(OptionValue.GetValue<float>(),
					min,
					max
				)
			);
		}
#endregion

#region Slots
		/// <summary>
		/// Handle volume settings changes.
		/// </summary>
		/// <param name="Sender">Sender object.</param>
		/// <param name="Event">Event.</param>
		private void OnSettingsChanged(object? Sender, GameSettingsEventArgs Event)
		{
			// Only volume settings
			if(Event.optionValue.optionData.m_OptionType == GameSettingOptionData.OptionType.OptionMusicVolume)
			{
				SetAudioGroupVolume(m_MusicAudioGroup!,
					Event.optionValue,
					ParameterMusicVolume
				);
			}
			else if(Event.optionValue.optionData.m_OptionType == GameSettingOptionData.OptionType.OptionSpeechVolume)
			{
				SetAudioGroupVolume(m_SpeechAudioGroup!,
					Event.optionValue,
					ParameterSpeechVolume
				);
			}
			else if(Event.optionValue.optionData.m_OptionType == GameSettingOptionData.OptionType.OptionSoundEffectsVolume)
			{
				SetAudioGroupVolume(m_SfxAudioGroup!,
					Event.optionValue,
					ParameterSfxVolume
				);
			}
		}
#endregion

#region Initialization
		/// <summary>
		/// Initialization.
		/// </summary>
		public void Initialize()
		{
			Ja2Logger.LogSound("Initialising JA2 sound manager");

			Assert.IsNotNull(m_AudioMixer);

			m_MusicAudioSources = new List<AudioSource>();

			// Set the volumes for the audio groups
			{
				SetAudioGroupVolume(m_MusicAudioGroup!,
					m_GameState!.gameSettings[GameSettingOptionData.OptionType.OptionMusicVolume],
					ParameterMusicVolume
				);
				SetAudioGroupVolume(m_SpeechAudioGroup!,
					m_GameState!.gameSettings[GameSettingOptionData.OptionType.OptionSpeechVolume],
					ParameterSpeechVolume
				);
				SetAudioGroupVolume(m_SfxAudioGroup!,
					m_GameState!.gameSettings[GameSettingOptionData.OptionType.OptionSoundEffectsVolume],
					ParameterSfxVolume
				);
			}

			// Connect to settings events
			m_GameState.gameSettings.eventSettingsChanged += OnSettingsChanged;
		}
#endregion
	}
}
