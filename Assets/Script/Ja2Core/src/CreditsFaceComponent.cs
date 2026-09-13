using System;
using System.Threading;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

using Random = UnityEngine.Random;

namespace Ja2
{
	/// <summary>
	/// Component handling the credits faces.
	/// </summary>
	public sealed class CreditsFaceComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
#region Constants
		/// <summary>
		/// Blinking transition time.
		/// </summary>
		private const float BlinkTransition = 0.1f;

		/// <summary>
		/// Max iterations for the transition
		/// </summary>
		private const int MaxIterations = 4;

		/// <summary>
		/// Closed eyes duration.
		/// </summary>
		private const float ClosedEyesDuration = 0.15f;
#endregion

#region Fields Component
		/// <summary>
		/// Image component used for the eyes.
		/// </summary>
		/// <returns></returns>
		[SerializeField]
		private Image? m_ImageEyes;

		/// <summary>
		/// See <see cref="spriteEyes"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_SpriteEyes;

		/// <summary>
		/// Delay between blinkings.
		/// </summary>
		[SerializeField]
		private float m_BlinkDelay;

		/// <summary>
		/// Additional info text.
		/// </summary>
		[Multiline(3)]
		[SerializeField]
		private string m_InfoText = string.Empty;
#endregion

#region Fields
		/// <summary>
		/// Cancelation token for async tasks.
		/// </summary>
		private CancellationToken m_CancellationToken;

		/// <summary>
		/// Time, when the last blink ended.
		/// </summary>
		private float m_LastBlink;

		/// <summary>
		/// Blinking eyes task.
		/// </summary>
		private UniTask? m_BlinkingTask;
#endregion

#region Properties
		/// <summary>
		/// Sprite used for the eyes.
		/// </summary>
		public Sprite? spriteEyes
		{
			get => m_SpriteEyes;
			set
			{
				m_SpriteEyes = value;

				// Update the image component also
				m_ImageEyes!.sprite = m_SpriteEyes;
			}
		}
#endregion

#region Events
		/// <summary>
		/// Event raised, when the info text should be shown
		/// </summary>
		public event Action<string>? eventFaceInfoShow;

		/// <summary>
		/// Event raised, when the info text should be hidden.
		/// </summary>
		public event Action? eventFaceInfoHide;
#endregion

#region Methods Public
		/// <summary>
		/// Initialization.
		/// </summary>
		/// <param name="CancellationToken">Cancelation token.</param>
		public void Initialize(CancellationToken CancellationToken)
		{
			m_CancellationToken = CancellationToken;

			// Reset the last blinking time to the random number
			m_LastBlink = Random.Range(0,
				m_BlinkDelay
			);
		}

		/// <summary>
		/// Update.
		/// </summary>
		public void DoUpdate()
		{
			// Is blinking task in progress
			if(m_BlinkingTask.HasValue)
			{
				// If completed, reset it
				if(m_BlinkingTask.Value.Status.IsCompleted())
					m_BlinkingTask = null;
				else
					return;
			}

			// Needs to start the blinking task
			if(m_LastBlink > m_BlinkDelay)
			{
				Assert.IsFalse(m_BlinkingTask.HasValue);

				m_BlinkingTask = HandleEyesBlinkingAsync();

				return;
			}

			m_LastBlink += Time.deltaTime;
		}
#endregion

#region Methods Private
		/// <summary>
		/// Blinking.
		/// </summary>
		private async UniTask HandleEyesBlinkingAsync()
		{
			// Transition to closed eyes
			for(var i = 0; i < MaxIterations; ++i)
			{
				Color new_color = m_ImageEyes!.color;

				// Update alpha channel as a transition
				new_color.a = Mathf.Lerp(0,
					1,
					(float)i / MaxIterations
				);

				m_ImageEyes.color = new_color;

				await UniTask.WaitForSeconds(BlinkTransition / MaxIterations,
					cancellationToken: m_CancellationToken
				);
			}

			// Wait for the closed eyes (add some randomness)
			await UniTask.WaitForSeconds(ClosedEyesDuration + Random.Range(0, ClosedEyesDuration / 2),
				cancellationToken: m_CancellationToken
			);

			// Reset the alpha back
			Color new_color_reset = m_ImageEyes!.color;
			new_color_reset.a = 0;
			m_ImageEyes!.color =  new_color_reset;

			m_LastBlink = 0;
		}
#endregion

#region Slots
		/// <summary>
		/// On mouse enter.
		/// </summary>
		/// <param name="EventData">Event.</param>
		public void OnPointerEnter(PointerEventData EventData)
		{
			eventFaceInfoShow?.Invoke(m_InfoText);
		}

		/// <summary>
		/// On mouse exit.
		/// </summary>
		/// <param name="EventData">Event.</param>
		public void OnPointerExit(PointerEventData EventData)
		{
			eventFaceInfoHide?.Invoke();
		}
#endregion
	}
}
