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
		/// See <see cref="fontAsset"/>.
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
		private Material? m_FontMaterial;
#endregion

#region Properties
		/// <summary>
		/// Font asset to use.
		/// </summary>
		public AssetJa2Font? fontAsset
		{
			get => m_FontAsset;
			set
			{
				m_FontAsset = value;
				// Clear the material, so it is reloaded when needed
				m_FontMaterial = null;

				// New font asset assignment
				if(m_FontAsset != null)
				{
					font = m_FontAsset.font;

					ApplyChanges();
				}
				// Reset
				else
				{
					font = null;
				}
			}
		}
#endregion

#region Messages
		/// <inheritdoc/>
		protected override void Awake()
		{
			base.Awake();

			ApplyChanges();
		}

#if UNITY_EDITOR
		/// <inheritdoc/>
		protected override void OnValidate()
		{
			base.OnValidate();

			// Apply the font, if provided
			if(m_FontAsset != null)
				font = m_FontAsset.font;
		}
#endif
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
				// Clone the material (if needed) to set custom properties
				if(m_FontMaterial == null)
					m_FontMaterial = fontMaterial;

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
