using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;

namespace Ja2.UI
{
	/// <summary>
	/// Layout sizer for the TMP_Text component with support for maximu width. It is used for cases, when the text
	/// component should be resized to fit the content, but no more than the specified maximum width.
	/// </summary>
	[ExecuteAlways]
	[RequireComponent(typeof(TMP_Text))]
	public sealed class TextLayoutSizer : UIBehaviour, ILayoutElement
	{
#region Fields Component
		/// <summary>
		/// Maximum with of the text component.
		/// </summary>
		[SerializeField]
		private float m_MaxWidth = 200;
#endregion

#region Fields
		/// <summary>
		/// Text component.
		/// </summary>
		private TMP_Text m_Text = null!;

		/// <summary>
		/// Preferred width of the text component.
		/// </summary>
		private float m_PreferredWidth = -1;

		/// <summary>
		/// Preferred height of the text component.
		/// </summary>
		private float m_PreferredHeight = -1;
#endregion

#region Properties
		/// <summary>
		/// -1 = "not specified", falls through to others
		/// </summary>
		public float minWidth => -1;

		/// <inheritdoc/>
		public float preferredWidth => m_PreferredWidth;

		/// <inheritdoc/>
		public float flexibleWidth => -1;

		/// <inheritdoc/>
		public float minHeight => -1;

		/// <inheritdoc/>
		public float preferredHeight => m_PreferredHeight;

		/// <inheritdoc/>
		public float flexibleHeight => -1;

		/// <summary>
		/// Need to have this layout element priority over the TMP_Text.
		/// </summary>
		public int layoutPriority => 1;
#endregion

#region Messages
		/// <inheritdoc/>
		protected override void Awake()
		{
			m_Text = GetComponent<TMP_Text>();
		}
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void CalculateLayoutInputHorizontal()
		{
			// Measure the preferred width
			m_PreferredWidth = Mathf.Ceil(
				Mathf.Min(
					m_Text.GetPreferredValues(m_Text.text,
						m_MaxWidth,
						Mathf.Infinity
					).x,
					m_MaxWidth
				)
			);
		}

		/// <inheritdoc/>
		public void CalculateLayoutInputVertical()
		{
			// Measure the preferred height after the width is set
			m_PreferredHeight = Mathf.Ceil(
				m_Text.GetPreferredValues(m_Text.text,
					m_PreferredWidth,
					Mathf.Infinity
				).y
			);
		}
#endregion
	}
}
