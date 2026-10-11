using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace Ja2.UI
{
	/// <summary>
	/// Manager for showing the fast help texts.
	/// </summary>
	public sealed class FastHelpTextManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Canvas used for the fast help text objects.
		/// </summary>
		[SerializeField]
		private Canvas? m_Canvas;

		/// <summary>
		/// Prefab for the fasth help text.
		/// </summary>
		[SerializeField]
		private GameObject? m_FastHelpTextPrefab;
#endregion

#region Fields
		/// <summary>
		/// Dictionary of the showed texts.
		/// </summary>
		private readonly Dictionary<FastHelpTextComponent, GameObject> m_ShowedTexts = new();
#endregion

#region Methods Public
		/// <summary>
		/// Instantiate the prefab and return the game object.
		/// </summary>
		/// <param name="Component">Component, for which the prefab is instantiated.</param>
		/// <param name="Font">Default font to use for the text.</param>
		public GameObject GetGameObject(FastHelpTextComponent Component, TMP_FontAsset Font)
		{
			// Instantiate the prefab
			GameObject ret = Instantiate(m_FastHelpTextPrefab,
				m_Canvas!.transform
			)!;
			ret.GetComponent<FastHelpTextRendererComponent>().Initialize(m_Canvas,
				Font
			);

			m_ShowedTexts[Component] = ret;

			return ret;
		}

		/// <summary>
		/// Hide the text for the given component.
		/// </summary>
		/// <param name="Component">Component.</param>
		public void ReleaseGameObject(FastHelpTextComponent Component)
		{
			// Try to find, if it is shown
			if(m_ShowedTexts.TryGetValue(Component, out GameObject go_text))
			{
				// Destroy
				Destroy(go_text);

				m_ShowedTexts.Remove(Component);
			}
		}
#endregion
	}
}
