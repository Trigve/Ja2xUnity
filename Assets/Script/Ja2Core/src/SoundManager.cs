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
#region Fields Component
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

#region Initialization
		/// <summary>
		/// Initialization.
		/// </summary>
		public void Initialize()
		{
			Ja2Logger.LogSound("Initialising JA2 sound manager");

			Assert.IsNotNull(m_AudioMixer);

			m_MusicAudioSources =  new List<AudioSource>();
		}
#endregion
	}
}
