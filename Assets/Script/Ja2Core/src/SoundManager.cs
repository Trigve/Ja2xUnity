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
	[CreateAssetMenu(menuName = "JA2/Create Sound Manager")]
	public sealed class SoundManager : ScriptableObjectManager<SoundManager>
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
		/// Parent object for all the audio sources.
		/// </summary>
		private GameObject? m_AudioSourceParent;

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
				Source.transform.SetParent(m_AudioSourceParent!.transform);

				// Play the clip now
				Source.Play();
			}
		}
#endregion

#region Construction
		/// <inheritdoc />
		protected override void DoInitialize(params object[] Params)
		{
			Ja2Logger.LogSound("Initialising JA2 sound manager");

			Assert.IsNotNull(m_AudioMixer);

			m_MusicAudioSources =  new List<AudioSource>();
		}

		/// <summary>
		/// Initialize after the scene was loaded.
		/// </summary>
		/// <param name="Parent">Parent GO, to which to parent the audio sources.</param>
		public void InitializeSceneLoad(GameObject Parent)
		{
			m_AudioSourceParent = Parent;
		}

		/// <inheritdoc />
		protected override void DoDeinitialize()
		{
			m_MusicAudioSources!.Clear();
			m_MusicAudioSources = null;
			m_AudioSourceParent = null;
		}
#endregion
	}
}
