using TMPro;

using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Component for handling the TMP_Text with custom fonts.
	/// </summary>
	[RequireComponent(typeof(TMP_Text))]
	public sealed class TextComponent : MonoBehaviour
	{
#if UNITY_EDITOR
#region Editor Reflection
		/// <summary>
		/// Reflection.
		/// </summary>
		public const string TextComponentFieldName = nameof(m_TextComponent);

		/// <summary>
		/// Reflection.
		/// </summary>
		public const string FontAssetFieldName = nameof(m_FontAsset);

		/// <summary>
		/// Reflection.
		/// </summary>
		public const string UseShadowFieldName = nameof(m_UseShadow);

		/// <summary>
		/// Reflection.
		/// </summary>
		public const string ShadowColorIndexFieldName = nameof(m_ShadowColorIndex);
#endregion
#endif

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
		/// Text component.
		/// </summary>
		[SerializeField]
		private TMP_Text? m_TextComponent;

		/// <summary>
		/// See <see cref="fontAsset"/>.
		/// </summary>
		[SerializeField]
		private AssetFontClass? m_FontAsset;

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

#region Properties
		/// <summary>
		/// Font asset.
		/// </summary>
		public AssetFontClass? fontAsset
		{
			get => m_FontAsset;
			set
			{
				m_FontAsset = value;

				// Set the font in the control, if any
				m_TextComponent!.font = m_FontAsset?.fontBase;

				ApplyChanges();
			}
		}
#endregion

#if UNITY_EDITOR
#region Methods Public Editor
		/// <summary>
		/// Called from the custom editor, when the font was changed.
		/// </summary>
		public void EditorFontChanged()
		{
			fontAsset = m_FontAsset;
		}

		/// <summary>
		/// Called from the custom editor, when other property was changed.
		/// </summary>
		public void EditorChangedOther()
		{
			ApplyChanges();
		}
#endregion
#endif

#region Methods Private
		/// <summary>
		/// Apply any changes.
		/// </summary>
		private void ApplyChanges()
		{
			// Only for bitmap fonts
			if(m_FontAsset?.fontType == AssetFontClass.FontType.BitmapFont)
			{
				m_TextComponent!.fontSharedMaterial.SetFloat(ShadowUsePropertyId,
					m_UseShadow ? 1f : 0f
				);
				m_TextComponent!.fontSharedMaterial.SetFloat(ShadowColorIndexPropertyId,
					m_ShadowColorIndex
				);
			}
		}
#endregion
	}
}
