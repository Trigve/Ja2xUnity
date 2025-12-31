using System;

using UnityEngine;
using UnityEngine.Assertions;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Mocker for the MainMenuButton.
	/// </summary>
	public sealed class AssetRefMockerMainMenuButton : AssetRefMocker<MainMenuButton>
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

			m_Component!.spriteNormal = (Sprite?)MockData.m_Assets[0];
			m_Component!.spriteHighlighted = (Sprite?)MockData.m_Assets[1];
			m_Component!.spritePressed = (Sprite?)MockData.m_Assets[2];
			m_Component!.spriteDisabled = (Sprite?)MockData.m_Assets[3];

			m_Component!.Refresh();
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			AssetMockData asset_mock = new (
				new Object?[]
				{
					m_Component!.spriteNormal,
					m_Component.spriteHighlighted!,
					m_Component.spritePressed,
					m_Component.spriteDisabled,
				}
			);

			// Reset the original asset
			m_Component.spriteNormal = null;
			m_Component.spriteHighlighted = null;
			m_Component.spritePressed = null;
			m_Component.spriteDisabled = null;

			m_Component.Clear();

			return asset_mock;
		}
#endif

#endregion
	}
}
