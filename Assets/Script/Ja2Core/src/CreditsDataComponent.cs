using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Helper component for the <see cref="CreditsDataAsset"/>.
	/// </summary>
	public sealed class CreditsDataComponent : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Credits data.
		/// </summary>
		public CreditsDataAsset? m_CreditsData;

		/// <summary>
		/// Title font.
		/// </summary>
		[Header("=== Fonts ===")]
		public AssetFontClass? m_FontTitle;

		/// <summary>
		/// Normal font.
		/// </summary>
		public AssetFontClass? m_FontNormal;
#endregion
	}
}
