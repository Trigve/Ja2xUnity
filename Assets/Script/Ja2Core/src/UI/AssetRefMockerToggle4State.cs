using System;

using UnityEngine;
using UnityEngine.Assertions;

namespace Ja2.UI
{
	/// <summary>
	/// Ref mocker for the <see cref="Toggle4State"/>.
	/// </summary>
	public sealed class AssetRefMockerToggle4State : AssetRefMocker<Toggle4State>
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
			Assert.IsTrue(MockData.m_Assets.Length == 4);

			m_Component!.spriteNormalOn = (Sprite?)MockData.m_Assets[0];
			m_Component!.spriteNormalOff = (Sprite?)MockData.m_Assets[1];
			m_Component!.spriteHiliteOn = (Sprite?)MockData.m_Assets[2];
			m_Component!.spriteHiliteOff = (Sprite?)MockData.m_Assets[3];

			m_Component!.Refresh();
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			AssetMockData asset_mock = new (
				new UnityEngine.Object?[]
				{
					m_Component!.spriteNormalOn,
					m_Component.spriteNormalOff!,
					m_Component.spriteHiliteOn,
					m_Component.spriteHiliteOff,
				}
			);

			// Reset the original asset
			m_Component.spriteNormalOn = null;
			m_Component.spriteNormalOff = null;
			m_Component.spriteHiliteOn = null;
			m_Component.spriteHiliteOff = null;

			m_Component.Clear();

			return asset_mock;
		}

		/// <inheritdoc />
		protected override void DoResetAssets()
		{
			m_Component!.spriteNormalOn = null;
			m_Component!.spriteNormalOff = null;
			m_Component!.spriteHiliteOn = null;
			m_Component!.spriteHiliteOff = null;

			m_Component.Clear();
		}
#endif
#endregion
	}
}
