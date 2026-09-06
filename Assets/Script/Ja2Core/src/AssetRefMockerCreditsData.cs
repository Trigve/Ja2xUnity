using System;

using UnityEngine.Assertions;

using Object = UnityEngine.Object;

namespace Ja2
{
	/// <summary>
	/// Mocker for the credit's data.
	/// </summary>
	public sealed class AssetRefMockerCreditsData : AssetRefMocker<CreditsDataComponent>
	{
#region Constants
		/// <summary>
		/// Types used.
		/// </summary>
		private static readonly Type[] AssetTypes =
		{
			typeof(CreditsDataAsset),
			typeof(AssetFontClass),
			typeof(AssetFontClass),
		};
#endregion

#region Properties
		/// <inheritdoc />
		public override Type[] assetType => AssetTypes;
#endregion

#region Methods Private
		/// <inheritdoc />
		protected override void DoLoadAssets(AssetMockData MockData)
		{
			Assert.IsTrue(MockData.m_Assets.Length == 3);

			m_Component!.m_CreditsData = (CreditsDataAsset?)MockData.m_Assets[0];
			m_Component!.m_FontTitle = (AssetFontClass?)MockData.m_Assets[1];
			m_Component!.m_FontNormal = (AssetFontClass?)MockData.m_Assets[2];
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			// Create the asset ref
			AssetMockData asset_mock = new(
				new Object[]
				{
					m_Component!.m_CreditsData!,
					m_Component!.m_FontTitle!,
					m_Component!.m_FontNormal!,
				}
			);

			// Reset the original asset
			m_Component.m_CreditsData = null;
			m_Component.m_FontTitle = null;
			m_Component.m_FontNormal = null;

			return asset_mock;
		}

		/// <inheritdoc />
		protected override void DoResetAssets()
		{
			m_Component!.m_CreditsData = null;
			m_Component.m_FontTitle = null;
			m_Component.m_FontNormal = null;
		}
#endif
#endregion
	}
}
