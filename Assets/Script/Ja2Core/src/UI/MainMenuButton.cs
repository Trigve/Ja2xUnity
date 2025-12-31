using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ja2.UI
{
	/// <summary>
	/// Main menu button that implements the sprite swaps.
	/// </summary>
	[DisallowMultipleComponent]
	public sealed class MainMenuButton : Button
	{
#region Fields Component
		/// <summary>
		/// See <see cref="spriteNormal"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_Normal;

		/// <summary>
		/// See <see cref="spriteHighlighted"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_Highlighted;

		/// <summary>
		/// See <see cref="spritePressed"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_Pressed;

		/// <summary>
		/// See <see cref="spriteDisabled"/>.
		/// </summary>
		[SerializeField]
		private Sprite? m_Disabled;

		/// <summary>
		/// Visual root transform.
		/// </summary>
		[SerializeField]
		[HideInInspector]
		private RectTransform? m_VisualRoot;
#endregion

#region Fields
		/// <summary>
		/// Is pointer inside the button.
		/// </summary>
		private bool m_IsPointerInside;

		/// <summary>
		/// Base offset for the button.
		/// </summary>
		private Vector2 m_BaseOffset;
#endregion

#region Properties
		/// <summary>
		/// Normal sprite.
		/// </summary>
		public Sprite? spriteNormal
		{
			get => m_Normal;
			set => m_Normal = value;
		}

		/// <summary>
		/// Highlited sprite.
		/// </summary>
		public Sprite? spriteHighlighted
		{
			get => m_Highlighted;
			set => m_Highlighted = value;
		}

		/// <summary>
		/// Pressed sprite.
		/// </summary>
		public Sprite? spritePressed
		{
			get => m_Pressed;
			set => m_Pressed = value;
		}

		/// <summary>
		/// Disabled sprite.
		/// </summary>
		public Sprite? spriteDisabled
		{
			get => m_Disabled;
			set => m_Disabled = value;
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

			// Get the visual root
			if(m_VisualRoot == null)
				m_VisualRoot = image.rectTransform;

			m_BaseOffset = m_VisualRoot.anchoredPosition;

			// Disable transition as it would be made with the code
			transition = Transition.None;
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

			ApplyHighlighted();
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
			if(!interactable)
				return;

			ApplyPressed();
		}

		/// <inheritdoc />
		public override void OnPointerUp(PointerEventData Event)
		{
			if(!interactable)
				return;

			if(m_IsPointerInside)
				ApplyHighlighted();
			else
				ApplyNormal();
		}
#endregion

#region Methods Private
		/// <summary>
		/// Apply normal sprite.
		/// </summary>
		private void ApplyNormal()
		{
			SetSprite(m_Normal);
			m_VisualRoot!.anchoredPosition = m_BaseOffset;
		}

		/// <summary>
		/// Apply highlited state
		/// </summary>
		private void ApplyHighlighted()
		{
			SetSprite(m_Highlighted ? m_Highlighted : m_Normal);
			m_VisualRoot!.anchoredPosition = m_BaseOffset;
		}

		/// <summary>
		/// Apply pressed state.
		/// </summary>
		private void ApplyPressed()
		{
			SetSprite(m_Pressed);
			m_VisualRoot!.anchoredPosition = m_BaseOffset + m_Pressed!.pivot / m_Pressed.pixelsPerUnit;
		}

		/// <summary>
		/// Apply disabled state.
		/// </summary>
		private void ApplyDisabled()
		{
			SetSprite(m_Disabled != null ? m_Disabled : m_Normal);
			m_VisualRoot!.anchoredPosition = m_BaseOffset;
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
