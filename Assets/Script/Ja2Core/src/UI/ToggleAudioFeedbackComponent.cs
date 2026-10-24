using UnityEngine;
using UnityEngine.UI;

namespace Ja2.UI
{
	/// <summary>
	/// Audio feedback for the toggle component.
	/// </summary>
	[RequireComponent(typeof(Toggle4State))]
	[RequireComponent(typeof(AudioSource))]
	public sealed class ToggleAudioFeedbackComponent : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

		/// <summary>
		/// Audio clip played on checked value.
		/// </summary>
		[Attributes.AssetRefType(typeof(AudioClip))]
		[SerializeField]
		private AssetRef m_AudioOn;

		/// <summary>
		/// Audio clip played on unchecked value.
		/// </summary>
		[Attributes.AssetRefType(typeof(AudioClip))]
		[SerializeField]
		private AssetRef m_AudioOff;
#endregion

#region Fields
		/// <summary>
		/// Toggle component.
		/// </summary>
		private Toggle4State? m_Toggle;

		/// <summary>
		/// Audio source to use.
		/// </summary>
		private AudioSource? m_AudioSource;

		/// <summary>
		/// See <see cref="m_AudioOn"/>.
		/// </summary>
		private AudioClip? m_AudioClipOn;

		/// <summary>
		/// See <see cref="m_AudioOff"/>.
		/// </summary>
		private AudioClip? m_AudioClipOff;
#endregion

#region Messages
		public void Awake()
		{
			m_Toggle = GetComponent<Toggle4State>();
			m_AudioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
			// Must be provided
			if(m_Toggle == null || m_AudioSource == null)
			{
				Debug.LogError(
					string.Format("{0} requires {1} and {2}",
						nameof(ToggleAudioFeedbackComponent),
						nameof(AudioSource),
						nameof(Toggle)
					)
				);
			}
#endif
			// Load the audio clips
			m_AudioClipOn = m_GameState.assetManager.LoadAsset<AudioClip>(m_AudioOn);
			m_AudioClipOff = m_GameState.assetManager.LoadAsset<AudioClip>(m_AudioOff);

			// Need to use solo events, otherwise if the value changed event were used, it would be trigger
			// also on initial value settings and then when object would be reused (which is case for
			// ASPID VirtualList).
			if(m_Toggle != null)
				m_Toggle.eventToggleClicked += OnToggleValueChanged;
		}
#endregion

#region Slots
		/// <summary>
		/// Play the sound on the value change.
		/// </summary>
		/// <param name="Sender">Sender object.</param>
		/// <param name="Value">Value.</param>
		private void OnToggleValueChanged(object? Sender, bool Value)
		{
			// Only on enabled audio source
			if(m_AudioSource?.enabled ?? false)
				m_AudioSource?.PlayOneShot(Value ? m_AudioClipOn : m_AudioClipOff);
		}
#endregion
	}
}
