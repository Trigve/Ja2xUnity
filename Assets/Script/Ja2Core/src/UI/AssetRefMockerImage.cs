using System;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Mocker for the Image.
	/// </summary>
	public sealed class AssetRefMockerImage : AssetRefMocker<Image>
	{
#region Constants
		/// <summary>
		/// Types used.
		/// </summary>
		private static readonly Type[] AssetTypes = { typeof(Sprite) };
#endregion

#region Properties
		/// <inheritdoc />
		public override Type[] assetType => AssetTypes;
#endregion

#region Methods Private
		/// <inheritdoc />
		protected override void DoLoadAssets(AssetMockData MockData)
		{
			Assert.IsTrue(MockData.m_Assets.Length == 1);

			m_Component!.sprite = (Sprite?)MockData.m_Assets[0];
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			// Create the asset ref
			AssetMockData asset_mock = new (
				new Object[]
				{
					m_Component!.sprite
				}
			);

			// Reset the original asset
			m_Component.sprite = null;

			return asset_mock;
		}
#endif

#endregion
	}
}
