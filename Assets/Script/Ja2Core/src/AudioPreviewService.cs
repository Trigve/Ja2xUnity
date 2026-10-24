using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Audio preview service.
	/// </summary>
	public sealed class AudioPreviewService : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Speech audio source.
		/// </summary>
		[SerializeField]
		private AudioSource m_AudioSpeech = null!;

		/// <summary>
		/// Sfx audio source.
		/// </summary>
		[SerializeField]
		private AudioSource m_AudioSfx = null!;
#endregion

#region Fields
		/// <summary>
		/// Audio source, that is currently playing.
		/// </summary>
		private AudioSource? m_Current;
#endregion

#region Methods Public
		/// <summary>
		/// Preview the SFX.
		/// </summary>
		public void PreviewSfx()
		{
			StartPreview(m_AudioSfx);
		}

		/// <summary>
		/// Preview the speech.
		/// </summary>
		public void PreviewSpeech()
		{
			StartPreview(m_AudioSpeech);
		}
#endregion

#region Methods Private
		/// <summary>
		/// Start the preview of the given audio source. If there is any audio source in progress, it
		/// is stopped.
		/// </summary>
		/// <param name="AudioSourceNew">New audio source.</param>
		private void StartPreview(AudioSource AudioSourceNew)
		{
			// Something is playing already
			if(m_Current != null)
			{
				// Stop when the new audio source is different from the current one
				if(m_Current != AudioSourceNew)
				{
					m_Current.Stop();
					m_Current = AudioSourceNew;
					m_Current.Play();
				}
				// Same audio sources
				else
				{
					// If it stopped playing already, start it. Otherwise, let it play the current one
					if(!m_Current.isPlaying)
						m_Current.Play();
				}
			}
			else
			{
				m_Current = AudioSourceNew;
				m_Current.Play();
			}
		}
#endregion
	}
}
