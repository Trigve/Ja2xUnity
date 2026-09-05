using UnityEngine;

using TMPro;

namespace Ja2
{
	/// <summary>
	/// Font class asset.
	/// Font class represent the one concrete font with all the settings (bold, italic, ...).
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Font class")]
	public class AssetFontClass : AssetBase
	{
#region Enums
		/// <summary>
		/// Font type used.
		/// </summary>
		public enum FontType
		{
			BitmapFont,
			VectorFont,
		}
#endregion

#region Fields
		/// <summary>
		/// See <see cref="fontType"/>.
		/// </summary>
		[SerializeField]
		private FontType m_FontType;

		/// <summary>
		/// See <see cref="fontBase"/>.
		/// </summary>
		[SerializeField]
		private TMP_FontAsset? m_FontBase;

		/// <summary>
		/// See <see cref="paletteFontBase"/>.
		/// </summary>
		[SerializeField]
		private AssetStciPalette? m_PaletteFontBase;

		/// <summary>
		/// Bold font asset.
		/// </summary>
		[SerializeField]
		private TMP_FontAsset? m_FontBold;

		/// <summary>
		/// Palette used for the bold font.
		/// </summary>
		[SerializeField]
		private AssetStciPalette? m_PaletteFontBold;

		/// <summary>
		/// Reference point size of the font.
		/// </summary>
		[SerializeField]
		private int m_PointSize;
#endregion

#region Properties
		/// <summary>
		/// Font class type.
		/// </summary>
		public FontType fontType => m_FontType;

		/// <summary>
		/// Base font asset.
		/// </summary>
		public TMP_FontAsset fontBase => m_FontBase!;

		/// <summary>
		/// Palette for the base font.
		/// </summary>
		public AssetStciPalette? paletteFontBase => m_PaletteFontBase;
#endregion

#region Methods
		/// <summary>
		/// Add the base font.
		/// </summary>
		/// <param name="FontAsset">Base font asset.</param>
		/// <param name="Palette">Palette used, if any.</param>
		public void AddFontBase(TMP_FontAsset FontAsset, AssetStciPalette? Palette = null)
		{
			m_FontBase =  FontAsset;
			m_PaletteFontBase = Palette;
		}

		/// <summary>
		/// Add the bold font.
		/// </summary>
		/// <param name="FontAsset">Base font asset.</param>
		/// <param name="Palette">Palette used, if any.</param>
		public void AddFontBold(TMP_FontAsset FontAsset, AssetStciPalette? Palette = null)
		{
			m_FontBold =  FontAsset;
			m_PaletteFontBold = Palette;
		}
#endregion

#region Construction
		/// <summary>
		/// Create the asset.
		/// </summary>
		/// <param name="Type">Type of the font class.</param>
		/// <param name="PointSize">Reference point size.</param>
		/// <returns></returns>
		public static AssetFontClass Create(FontType Type, int PointSize)
		{
			var ret = CreateInstance<AssetFontClass>();

			ret.m_FontType = Type;
			ret.m_PointSize = PointSize;

			return ret;
		}
#endregion
	}
}
