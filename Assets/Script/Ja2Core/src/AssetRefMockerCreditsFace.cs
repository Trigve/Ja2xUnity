using System;

using UnityEngine;
using UnityEngine.Assertions;

using Object = UnityEngine.Object;

namespace Ja2
{
	/// <summary>
	/// Mocker for the credit's face.
	/// </summary>
	public sealed class AssetRefMockerCreditsFace : AssetRefMocker<CreditsFaceComponent>
	{
#region Constants
		/// <summary>
		/// Types used.
		/// </summary>
		private static readonly Type[] AssetTypes =
		{
			typeof(Sprite),
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
			Assert.IsTrue(MockData.m_Assets.Length == 1);

			m_Component!.spriteEyes = (Sprite?)MockData.m_Assets[0];
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			// Create the asset ref
			AssetMockData asset_mock = new(
				new Object[]
				{
					m_Component!.spriteEyes!,
				}
			);

			// Reset the original asset
			m_Component.spriteEyes = null;

			return asset_mock;
		}

		/// <inheritdoc />
		protected override void DoResetAssets()
		{
			m_Component!.spriteEyes = null;
		}
#endif
#endregion
	}
}
