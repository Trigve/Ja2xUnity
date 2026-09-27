using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ja2.UI
{
	/// <summary>
	/// 5 state button.
	/// </summary>
	public sealed class Button5State : Button
	{
#if UNITY_EDITOR
#region Editor Constants
		// Field names for the editor.
		public const string PropertyNameNormalOn = nameof(m_NormalOn);
		public const string PropertyNameNormalOff = nameof(m_NormalOff);
		public const string PropertyNameHiliteOn = nameof(m_HiliteOn);
		public const string PropertyNameHiliteOff = nameof(m_HiliteOff);
		public const string PropertyNameGrayed = nameof(m_Grayed);
		public const string PropertyNameDisableStyle = nameof(m_DisableStyle);
#endregion
#endif

#region Constants
		/// <summary>
		/// Property for animator.
		/// </summary>
		private static readonly int ParameterDisableStyle = Animator.StringToHash("DisableStyle");
#endregion

#region Enums
		/// <summary>
		/// Button's disable style.
		/// </summary>
		public enum ButtonDisableStyle
		{
			DisabledHatched = 0,
			DisabledShaded = 1,
		}
#endregion

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
		/// See <see cref="spritegrayed"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_Grayed;

		/// <summary>
		/// Disabled style.
		/// </summary>
		[SerializeField]
		private ButtonDisableStyle m_DisableStyle;
#endregion

#region Fields
		/// <summary>
		/// Is pointer inside the button.
		/// </summary>
		private bool m_IsPointerInside;

		/// <summary>
		/// Animator component.
		/// </summary>
		private Animator? m_Animator;
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

		/// <summary>
		/// Greyed-out sprite.
		/// </summary>
		public Sprite? spritegrayed
		{
			get => m_Grayed;
			set => m_Grayed = value;
		}
#endregion

#region Messages
		/// <inheritdoc />
		protected override void Awake()
		{
			base.Awake();

			if(image == null)
			{
#if UNITY_EDITOR
				Debug.LogWarning("Image component was not assigned");
#endif
				image = GetComponentInChildren<Image>();
			}

			m_Animator = GetComponent<Animator>();

			// Set the disabled style
			m_Animator.SetInteger(ParameterDisableStyle,
				(int)m_DisableStyle
			);
		}
#endregion

#region Methods Public

#if UNITY_EDITOR
		/// <summary>
		/// Clear the internal components
		/// </summary>
		public void Clear()
		{
			image.sprite = null;
		}
#endif

		/// <summary>
		/// Refresh the state.
		/// </summary>
		public void Refresh()
		{
			// Apply state based in interactivity
			if(IsInteractable())
				ApplyNormal();
			else
				ApplyDisabled();
		}

		/// <inheritdoc />
		public override void OnPointerEnter(PointerEventData Event)
		{
			base.OnPointerEnter(Event);

			m_IsPointerInside = true;

			if(!interactable)
				return;

			ApplyHilite();
		}

		/// <inheritdoc />
		public override void OnPointerExit(PointerEventData Event)
		{
			m_IsPointerInside = false;

			if(!interactable)
				return;

			ApplyNormal();
		}

		/// <inheritdoc />
		public override void OnPointerDown(PointerEventData Event)
		{
			base.OnPointerDown(Event);

			if(!interactable)
				return;

			ApplyPressed(true);
		}

		/// <inheritdoc />
		public override void OnPointerUp(PointerEventData Event)
		{
			base.OnPointerUp(Event);

			if(!interactable)
				return;

			ApplyPressed(false);
		}
#endregion

#region Methods Private
		/// <summary>
		/// Apply normal sprite.
		/// </summary>
		private void ApplyNormal()
		{
			SetSprite(m_NormalOff);
		}

		/// <summary>
		/// Apply highlited state
		/// </summary>
		private void ApplyHilite()
		{
			if(IsPressed())
				SetSprite(m_HiliteOn != null ? m_HiliteOn : m_NormalOn);
			else
				SetSprite(m_HiliteOff != null ? m_HiliteOff : m_NormalOff);
		}

		/// <summary>
		/// Apply pressed/unpressed state.
		/// </summary>
		/// <param name="Pressed">If true, it is pressed. Otherwise, unpressed.</param>
		private void ApplyPressed(bool Pressed)
		{
			if(Pressed)
				SetSprite(m_NormalOn);
			else
			{
				if(m_IsPointerInside)
					ApplyHilite();
				else
					ApplyNormal();
			}
		}

		/// <summary>
		/// Apply disabled state.
		/// </summary>
		private void ApplyDisabled()
		{
			SetSprite(m_Grayed != null ? m_Grayed : m_NormalOff);
		}

		/// <summary>
		/// Set the sprite and resize the image to fit the sprite.
		/// </summary>
		/// <param name="Sprite">Sprite instance.</param>
		private void SetSprite(Sprite? Sprite)
		{
			if(Sprite != null)
			{
				image.sprite = Sprite;
				image.SetNativeSize();
			}
		}
#endregion
	}
}
