using UnityEngine;

using TMPro;

namespace Ja2
{
	/// <summary>
	/// Component for mananing the credits data node.
	/// </summary>
	public sealed class CreditsDataNodeComponent : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Text control.
		/// </summary>
		[SerializeField]
		private TMP_Text? m_TextControl;

		/// <summary>
		/// Text component.
		/// </summary>
		[SerializeField]
		private UI.TextComponent? m_TextComponent;
#endregion

#region Fields
		/// <summary>
		/// Rect transform.
		/// </summary>
		private RectTransform m_RectTransform = null!;

		/// <summary>
		/// Generate text to show.
		/// </summary>
		private string m_TextGenerated = string.Empty;
#endregion

#region Methods Public
		/// <summary>
		/// Initialize with the node.
		/// </summary>
		/// <param name="Text">Text to show.</param>
		/// <param name="InitPosition">Initial position of the node.</param>
		/// <param name="Font">Font used for the text.</param>
		/// <param name="FontSize">Font size.</param>
		/// <param name="TextColor">Text color.</param>
		/// <param name="AlignmentHor">Horizontal alignment used for the text.</param>
		public void Initialize(string Text, RectTransform InitPosition, AssetFontClass Font, int FontSize, ushort TextColor, HorizontalAlignmentOptions AlignmentHor)
		{
#if UNITY_EDITOR
			// Just for the debugging
			name = Text;
#endif
			m_TextGenerated = Text;
			m_RectTransform = GetComponent<RectTransform>();

			// Set the font asset as first
			m_TextComponent!.fontAsset = Font;

			m_TextControl!.text = m_TextGenerated;
			// Need to set right point size for the font, be it bitmap one or TTF
			m_TextControl.fontSize = FontSize;

			// If palette is provided, set the color
			if(m_TextComponent.fontAsset.paletteFontBase != null)
				m_TextControl.faceColor = m_TextComponent.fontAsset.paletteFontBase[TextColor];

			m_TextControl.horizontalAlignment = AlignmentHor;

			// Set the initial position
			m_RectTransform.localPosition = InitPosition.localPosition;
		}

		/// <summary>
		/// Get difference between actual position and the reference transform.
		/// </summary>
		/// <param name="Reference">Reference transform.</param>
		/// <returns>Difference of Y positions.</returns>
		public float SpaceDiffY(Transform Reference)
		{
			return (m_RectTransform.position.y - m_RectTransform.rect.height) - Reference.position.y;
		}

		/// <summary>
		/// Scroll the node with the given speed.
		/// </summary>
		/// <param name="Speed"></param>
		public void ScrollNode(float Speed)
		{
			Vector3 cur_position = m_RectTransform.position;

			// Update the position based on the speed
			cur_position.y += Speed * Time.deltaTime;
			m_RectTransform.position = cur_position;
		}
#endregion
	}
}
