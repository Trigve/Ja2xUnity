using UnityEngine;

using TMPro;

namespace Ja2
{
	/// <summary>
	/// Font asset.
	/// </summary>
	public sealed class AssetJa2Font : AssetBase
	{
#region Fields Component
		/// <summary>
		/// See <see cref="font"/>.
		/// </summary>
		[SerializeField]
		private TMP_FontAsset? m_Font;

		/// <summary>
		/// See <see cref="isBitampFont"/>.
		/// </summary>
		[SerializeField]
		private bool m_IsBitampFont;
#endregion

#region Properties
		/// <summary>
		/// Font used.
		/// </summary>
		public TMP_FontAsset font => m_Font!;

		/// <summary>
		/// Is this a bitmap font.
		/// </summary>
		public bool isBitampFont => m_IsBitampFont;
#endregion

#region Construction
		/// <summary>
		/// Create instance.
		/// </summary>
		/// <param name="Font">Font asset.</param>
		/// <param name="IsBitmapFont">Is bitmap font.</param>
		/// <returns>New instance.</returns>
		public static AssetJa2Font Create(TMP_FontAsset Font, bool IsBitmapFont)
		{
			var ret = CreateInstance<AssetJa2Font>();
			ret.m_Font = Font;
			ret.m_IsBitampFont = IsBitmapFont;

			return ret;
		}
#endregion
	}
}
