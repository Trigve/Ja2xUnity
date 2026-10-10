using UnityEngine;

using TMPro;

namespace Ja2.UI
{
	/// <summary>
	/// Fast help text data.
	/// </summary>
	public sealed class FastHelpTextRendererComponent : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Text control.
		/// </summary>
		[SerializeField]
		private TMP_Text? m_Text;

		/// <summary>
		/// Max text width.
		/// </summary>
		[SerializeField]
		private int m_MaxTextWidth = 300;
#endregion

#region Fields
		/// <summary>
		/// Canvas to use.
		/// </summary>
		private Canvas m_Canvas = null!;
#endregion

#region Properties
		/// <summary>
		/// Set the text.
		/// </summary>
		public string text
		{
			set => m_Text!.text = value;
		}
#endregion

#region Methods Public
		/// <summary>
		/// Refresh the text control.
		/// </summary>
		public void RefreshText()
		{
			var text_transform = ((RectTransform)m_Text!.transform);
			var rect_transform = (RectTransform)transform;
			var canvas_transform = (RectTransform)m_Canvas.transform;

			// Measure the preferred width
			float width = Mathf.Ceil(
				Mathf.Min(
					m_Text!.GetPreferredValues(m_Text.text,
						m_MaxTextWidth,
						Mathf.Infinity
					).x,
					m_MaxTextWidth
				)
			);
			// Use padding as defined for text control
			width += Mathf.Abs(text_transform.offsetMin.x) + Mathf.Abs(text_transform.offsetMax.x);

			var corners = new Vector3[4];

			// Canvas dimension
			canvas_transform.GetWorldCorners(corners);
			float canvas_right = corners[3].x;

			// Tooltip dimensions
			rect_transform.GetWorldCorners(corners);

			// Adjust the width (with some margin) so it doesn't extend past the canvas
			width = Mathf.Min(width,
				canvas_right - corners[0].x - 5
			);

			// Measure the preferred height, after the width is set
			float height = Mathf.Ceil(
				m_Text.GetPreferredValues(m_Text.text,
					width,
					Mathf.Infinity
				).y
			);

			// Use padding as defined for text control
			height += Mathf.Abs(text_transform.offsetMin.y) + Mathf.Abs(text_transform.offsetMax.y);

			rect_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
				width
			);
			rect_transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
				height
			);
		}
#endregion

#region Initialization
		/// <summary>
		/// Initialization.
		/// </summary>
		/// <param name="CanvasToUse">Canvas to use.</param>
		public void Initialize(Canvas CanvasToUse)
		{
			m_Canvas = CanvasToUse;
		}
#endregion
	}
}
