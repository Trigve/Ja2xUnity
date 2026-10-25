using System;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

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

		/// <summary>
		/// Original pivot of the image component.
		/// </summary>
		private Vector2 m_PivotOrig;
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

#region Events
		/// <summary>
		/// Event raised when toggle is clicked.
		/// </summary>
		public event EventHandler<bool>? eventToggleClicked;
#endregion

#region Messages
		protected override void Awake()
		{
			base.Awake();

			m_PivotOrig = image.rectTransform.pivot;

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

		/// <inheritdoc/>
		public override void OnPointerClick(PointerEventData EventData)
		{
			if(EventData.button != PointerEventData.InputButton.Left)
				return;

			eventToggleClicked?.Invoke(this,
				!isOn
			);

			base.OnPointerClick(EventData);
		}

		/// <inheritdoc/>
		public override void OnSubmit(BaseEventData EventData)
		{
			eventToggleClicked?.Invoke(this,
				!isOn
			);

			base.OnSubmit(EventData);
		}

		/// <summary>
		/// Refresh the control.
		/// </summary>
		public void Refresh()
		{
			Sprite? sprite_to_set = null;

			switch(currentSelectionState)
			{
			case SelectionState.Normal:
				sprite_to_set = (isOn ? m_NormalOn : m_NormalOff);
				m_Text!.color = m_TextColorOrig;
				break;
			case SelectionState.Pressed or SelectionState.Selected or SelectionState.Highlighted:
				sprite_to_set = (isOn ? m_HiliteOn : m_HiliteOff);
				m_Text!.color = m_TextHiliteColor;
				break;
			default:
				sprite_to_set = image.sprite;
				m_Text!.color = m_TextColorOrig;
				break;
			}

			if(sprite_to_set != null)
			{
				image.sprite = sprite_to_set;

				// Update the pivot based on the sprite pivot
				RectTransform rect_trans = image.rectTransform;

				Vector2 pos = m_PivotOrig;

				pos.x += sprite_to_set.pivot.x / sprite_to_set.rect.height;
				pos.y -= (sprite_to_set.rect.height - sprite_to_set.pivot.y) / sprite_to_set.rect.height;

				rect_trans.pivot = pos;

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
