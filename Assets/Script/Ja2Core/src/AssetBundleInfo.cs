using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Information about assets in the asset bundle.
	/// </summary>
	[Serializable]
	public sealed class AssetBundleInfo : ScriptableObject
	{
#region Fields
		/// See <see cref="bundleId"/>.
		[SerializeField]
		private uint m_BundleId;

		/// See <see cref="assetNames"/>
		[SerializeField]
		private string[]? m_AssetNames;

		/// See <see cref="assetGUIDs"/>
		[SerializeField]
		private string[]? m_AssetGUIDs;
#endregion

#region Properties
		/// <summary>
		/// Bundle ID.
		/// </summary>
		public uint bundleId => m_BundleId;

		/// <summary>
		/// Asset names.
		/// </summary>
		public string[] assetNames => m_AssetNames!;

		/// <summary>
		/// Assets GUIDs.
		/// </summary>
		public string[] assetGUIDs => m_AssetGUIDs!;
#endregion

#region Methods
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="BundleId">Bundle ID.</param>
		/// <param name="AssetNames">All the asset names.</param>
		/// <param name="AssetGUIDs">All the asset GUIDs.</param>
		/// <returns></returns>
		public static AssetBundleInfo Create(uint BundleId, string[] AssetNames, string[] AssetGUIDs)
		{
			var obj = CreateInstance<AssetBundleInfo>();

			obj.m_BundleId = BundleId;
			obj.m_AssetNames = AssetNames;
			obj.m_AssetGUIDs = AssetGUIDs;

			return obj;
		}
#endregion
	}

}
