using TMPro;

using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Text component with shadow support.
	/// </summary>
	[ExecuteInEditMode]
	public sealed class TextWithShadow : TextMeshProUGUI
	{
#region Constants
		/// <summary>
		/// Material property ID for the shadow toggle.
		/// </summary>
		private static readonly int  ShadowUsePropertyId = Shader.PropertyToID("_UseFontShadow");

		/// <summary>
		/// Material property ID for the shadow color index.
		/// </summary>
		private static readonly int  ShadowColorIndexPropertyId = Shader.PropertyToID("_ShadowColorIndex");
#endregion

#region Fields Component
		/// <summary>
		/// Font asset to use.
		/// </summary>
		[SerializeField]
		private AssetJa2Font? m_FontAsset;

		/// <summary>
		/// Use text shadow.
		/// </summary>
		[SerializeField]
		private bool m_UseShadow;

		/// <summary>
		/// Shadow color index.
		/// </summary>
		[SerializeField]
		private int m_ShadowColorIndex;
#endregion

#region Fields
		/// <summary>
		/// Cloned material, that will be usde.
		/// </summary>
		private Material m_FontMaterial = null!;
#endregion

#region Messages
		/// <inheritdoc/>
		protected override void Awake()
		{
			base.Awake();

			// Clone the material to set custom properties
			m_FontMaterial = fontMaterial;

			ApplyChanges();
		}

		/// <inheritdoc/>
		protected override void OnValidate()
		{
			base.OnValidate();

			// Apply the font, if provided
			if(m_FontAsset != null)
				font = m_FontAsset.font;
		}
#endregion

#region Methods Public
		/// <summary>
		/// Apply any changes.
		/// </summary>
		public void ApplyChanges()
		{
			// Only for bitmap fonts
			if(m_FontAsset?.isBitampFont ?? false)
			{
				m_FontMaterial.SetFloat(ShadowUsePropertyId,
					m_UseShadow ? 1f : 0f
				);
				m_FontMaterial.SetFloat(ShadowColorIndexPropertyId,
					m_ShadowColorIndex
				);
			}
		}
#endregion
	}
}
