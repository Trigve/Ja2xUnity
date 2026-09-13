using UnityEngine;

using TMPro;

namespace Ja2
{
	/// <summary>
	/// Credits face info component.
	/// </summary>
	[RequireComponent(typeof(TMP_Text))]
	public sealed class CreditsInfoComponent : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Text control.
		/// </summary>
		[SerializeField]
		private TMP_Text? m_TextControl;
#endregion

#region Methods Public
		/// <summary>
		/// Show the text.
		/// </summary>
		public void Show()
		{
			gameObject.SetActive(true);
		}

		/// <summary>
		/// Hide the text.
		/// </summary>
		public void Hide()
		{
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Update the text to show.
		/// </summary>
		/// <param name="Text">Text to show.</param>
		public void UpdateText(string Text)
		{
			m_TextControl!.text = Text;
		}
#endregion
	}
}
