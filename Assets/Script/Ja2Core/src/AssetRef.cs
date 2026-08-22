using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Asset reference stores the asset path (with optional sub-asset if it references on) and optional bundle, from
	/// which it came from. When the AssetRef is acutally a sub-asset, the sub-asset name is stored as "@name".
	/// For instance "texture.sti@sprite_0".
	/// </summary>
	[Serializable]
	public struct AssetRef : ISerializationCallbackReceiver
	{
#region Fields Component
		/// <summary>
		/// If it is a simple string, then it denotes a bundle name. If it is in format "[x]", then
		/// it means a bundle with the ID of "x".
		/// </summary>
		[SerializeField]
		private string m_Bundle;

		/// <summary>
		/// Asset path.
		/// </summary>
		[SerializeField]
		private string m_AssetPath;
#endregion

#region Fields
		/// <summary>
		/// Main asset path.
		/// </summary>
		private string m_MainAssetPath;

		/// <summary>
		/// Sub-asset namr, if any.
		/// </summary>
		private string m_SubAssetName;
#endregion

#region Properties
		/// <summary>
		/// Is instance valid. At least, asset should be valid.
		/// </summary>
		public bool isValid => !string.IsNullOrEmpty(m_AssetPath);

		/// <summary>
		/// Bundle.
		/// </summary>
		public string bundle => m_Bundle;

		/// <summary>
		/// Full bundle name (with extension).
		/// </summary>
		public string bundleFull => m_Bundle + ".bundle";

		/// <summary>
		/// Main asset path.
		/// </summary>
		public string assetPathMain => m_MainAssetPath;

		/// <summary>
		/// Sub-asset name, if any.
		/// </summary>
		public string subAssetName => m_SubAssetName;

		/// <summary>
		/// Get the bundle ID, if it contains one. Otherwise, null.
		/// </summary>
		public uint? bundleId
		{
			get
			{
				uint? ret = null;

				// Must have at least 3 characters ([, <number>, ])
				if(m_Bundle.Length >= 3 && m_Bundle[0] == '[' && m_Bundle[^1] == ']')
				{
					ret = uint.Parse(
						m_Bundle.Substring(1,
							m_Bundle.Length - 2
						)
					);
				}

				return ret;
			}
		}

		/// <summary>
		/// As combined path.
		/// </summary>
		public string combinedPath => m_Bundle + ":" + m_AssetPath;

		/// <summary>
		/// Has the asset path sub-asset?
		/// </summary>
		public bool hasSubAsset => !string.IsNullOrEmpty(m_SubAssetName);
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void OnBeforeSerialize()
		{
		}

		/// <inheritdoc/>
		public void OnAfterDeserialize()
		{
			m_MainAssetPath = m_AssetPath;
			m_SubAssetName = string.Empty;

			// Find if the asset is sub-asset
			int idx_sub_asset = m_AssetPath.IndexOf('@');
			if(idx_sub_asset >= 0)
			{
				m_MainAssetPath = m_AssetPath[..(idx_sub_asset)];
				m_SubAssetName = m_AssetPath[(idx_sub_asset + 1)..];
			}
		}
#endregion

#region Methods Static
		/// <summary>
		/// Parse from string.
		/// </summary>
		/// <param name="AssetPathCombined">Combined string from which to parse.</param>
		/// <returns></returns>
		public static AssetRef Parse(string AssetPathCombined)
		{
			string bundle, path;

			int idx = AssetPathCombined.IndexOf(':');
			if(idx != -1)
			{
				bundle = AssetPathCombined[..idx];
				path = AssetPathCombined[(idx + 1)..];
			}
			else
				bundle = path = string.Empty;

			var sub_asset_name = string.Empty;
			idx = AssetPathCombined.IndexOf('@');
			if(idx >= 0)
			{
				path = AssetPathCombined[..(idx)];
				sub_asset_name = AssetPathCombined[(idx + 1)..];
			}

			return new AssetRef(path,
				sub_asset_name,
				bundle
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="AssetPath">Asset Path</param>
		/// <param name="SubAsset">Sub-asset name, if any.</param>
		/// <param name="Bundle">Bundle</param>
		public AssetRef(string AssetPath, string SubAsset, string Bundle = Constants.StringEmpty)
		{
			m_Bundle = Bundle;
			m_AssetPath = AssetPath + (string.IsNullOrEmpty(SubAsset) ? string.Empty : "@" + SubAsset);
			m_MainAssetPath = AssetPath;
			m_SubAssetName = SubAsset;
		}

		/// <summary>
		/// Constructor using the bundle ID.
		/// </summary>
		/// <param name="AssetPath">Asset path.</param>
		/// <param name="SubAsset">Sub-asset name, if any.</param>
		/// <param name="BundleId">Bundle ID.</param>
		public AssetRef(string AssetPath, string SubAsset, uint BundleId)
		{
			// Store as ID
			m_Bundle = "[" + BundleId + "]";
			m_AssetPath = AssetPath + (string.IsNullOrEmpty(SubAsset) ? string.Empty : "@" + SubAsset);
			m_MainAssetPath = AssetPath;
			m_SubAssetName = SubAsset;
		}
#endregion
	}
}
