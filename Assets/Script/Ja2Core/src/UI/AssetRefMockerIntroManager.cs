using System;
using System.Linq;

using UnityEngine.Video;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Mocker for the <see cref="ScreenIntroManager"/>.
	/// </summary>
	public sealed class AssetRefMockerIntroManager : AssetRefMocker<ScreenIntroManager>
	{
#region Constants
		/// <summary>
		/// Types used.
		/// </summary>
		private static readonly Type[] AssetTypes = { typeof(VideoClip) };
#endregion

#region Properties
		/// <inheritdoc />
		public override Type[] assetType => AssetTypes;
#endregion

#region Methods Private
		/// <inheritdoc />
		protected override void DoLoadAssets(AssetMockData MockData)
		{
			// Fill the new values
			for(var i = 0; i < m_Component!.videoClips.Length; ++i)
				m_Component!.videoClips[i] = (VideoClip)MockData.m_Assets[i]!;
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		protected override AssetMockData DoGatherAssets()
		{
			AssetMockData asset_mock = new (
				m_Component!.videoClips.Cast<Object?>().ToArray()
			);

			// Reset the original asset
			for(var i = 0; i < m_Component!.videoClips.Length; ++i)
				m_Component!.videoClips[i] = null;

			return asset_mock;
		}
#endif

#endregion
	}
}
