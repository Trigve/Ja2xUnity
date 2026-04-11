using System;

using UnityEngine;
using UnityEngine.Assertions;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Mocker for the <c>AudioSource</c>.
	/// </summary>
	public class AssetRefMockerAudioSource : AssetRefMocker<AudioSource>
	{
#region Constants
		/// <summary>
		/// Types used.
		/// </summary>
		private static readonly Type[] AssetTypes = { typeof(AudioClip) };
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

			m_Component!.resource = (AudioClip?)MockData.m_Assets[0];
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			// Create the asset ref
			AssetMockData asset_mock = new (
				new Object[]
				{
					m_Component!.resource
				}
			);

			// Reset the original asset
			m_Component.resource = null;

			return asset_mock;
		}

		/// <inheritdoc />
		protected override void DoResetAssets()
		{
			m_Component!.resource = null;
		}
#endif
#endregion
	}
}
