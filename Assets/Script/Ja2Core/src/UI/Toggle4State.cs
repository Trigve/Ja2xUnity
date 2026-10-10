using TMPro;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ja2.UI
{
	/// <summary>
	/// 4 state toggle.
	/// </summary>
	public sealed class Toggle4State : Toggle
	{
#if UNITY_EDITOR
#region Editor Constants
		// Field names for the editor.
		public const string PropertyNameNormalOn = nameof(m_NormalOn);
		public const string PropertyNameNormalOff = nameof(m_NormalOff);
		public const string PropertyNameHiliteOn = nameof(m_HiliteOn);
		public const string PropertyNameHiliteOff = nameof(m_HiliteOff);
		public const string PropertyNameTextHiliteColor = nameof(m_TextHiliteColor);
		public const string PropertyNameImage = "m_TargetGraphic";
#endregion
#endif

#region Fields Component
		/// <summary>
		/// See <see cref="spriteNormalOn"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_NormalOn;

		/// <summary>
		/// See <see cref="spriteNormalOff"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_NormalOff;

		/// <summary>
		/// See <see cref="spriteHiliteOn"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_HiliteOn;

		/// <summary>
		/// See <see cref="spriteHiliteOff"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_HiliteOff;

		/// <summary>
		/// Text hilite color.
		/// </summary>
		[SerializeField]
		private Color32 m_TextHiliteColor;
#endregion

#region Fields
		/// <summary>
		/// Text control.
		/// </summary>
		private TMP_Text? m_Text;

		/// <summary>
		/// Original text color.
		/// </summary>
		private Color32 m_TextColorOrig;
#endregion

#region Properties
		/// <summary>
		/// Normal pressed sprite.
		/// </summary>
		public Sprite? spriteNormalOn
		{
			get => m_NormalOn;
			set => m_NormalOn = value;
		}

		/// <summary>
		/// Normal (not pressed) sprite.
		/// </summary>
		public Sprite? spriteNormalOff
		{
			get => m_NormalOff;
			set => m_NormalOff = value;
		}

		/// <summary>
		/// Hilite pressed sprite.
		/// </summary>
		public Sprite? spriteHiliteOn
		{
			get => m_HiliteOn;
			set => m_HiliteOn = value;
		}

		/// <summary>
		/// Hilite (not pressed) sprite.
		/// </summary>
		public Sprite? spriteHiliteOff
		{
			get => m_HiliteOff;
			set => m_HiliteOff = value;
		}
#endregion

#region Messages
		protected override void Awake()
		{
			base.Awake();

			m_Text = GetComponentInChildren<TMP_Text>();
			Assert.IsNotNull(m_Text);

			m_TextColorOrig = m_Text.color;

			onValueChanged.AddListener(OnValueChanged);
		}
#endregion

#region Methods Public
		/// <inheritdoc/>
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);

			Refresh();
		}

		/// <inheritdoc/>
		public override void OnPointerExit(PointerEventData eventData)
		{
			base.OnPointerExit(eventData);

			Refresh();
		}

		/// <summary>
		/// Refresh the control.
		/// </summary>
		public void Refresh()
		{
			switch(currentSelectionState)
			{
			case SelectionState.Normal:
				image.sprite = (isOn ? m_NormalOn : m_NormalOff);
				m_Text!.color = m_TextColorOrig;
				break;
			case SelectionState.Pressed or SelectionState.Selected or SelectionState.Highlighted:
				image.sprite = (isOn ? m_HiliteOn : m_HiliteOff);
				m_Text!.color = m_TextHiliteColor;
				break;
			default:
				image.sprite = image.sprite;
				m_Text!.color = m_TextColorOrig;
				break;
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Clear the internal components
		/// </summary>
		public void Clear()
		{
			image.sprite = null;
		}
#endif
#endregion

#region Slots
		/// <summary>
		/// Value changed handler.
		/// </summary>
		/// <param name="State"></param>
		private void OnValueChanged(bool State)
		{
			Refresh();
		}
#endregion
	}
}
