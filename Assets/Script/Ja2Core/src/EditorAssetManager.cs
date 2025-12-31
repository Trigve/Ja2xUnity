using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

using Object = UnityEngine.Object;

#if UNITY_EDITOR
namespace Ja2
{

	/// <summary>
	/// Asset manager for the editor. This class is intentionally in the Ja2Core asmdef, because it is used
	/// for some runtime components also. If it were in Ja2Editor, then it couldn't be used in this asmdef
	/// (editor dependencies problem) and code duplication would occur.
	/// </summary>
	public sealed class EditorAssetManager : UnityEditor.ScriptableSingleton<EditorAssetManager>
	{
#region Fields
		/// <summary>
		/// Mapping between the path and the corresponding asset bundle desc.
		/// </summary>
		private readonly List<(string path, AssetBundleDesc bundleDesc)> m_AssetsBundleDesc = new ();
#endregion

#region Messages
		public void OnEnable()
		{
			Reload();
		}
#endregion

#region Methods Public
		/// <summary>
		/// Load asset from the AssetRef.
		/// </summary>
		/// <param name="AssetPath">Asset ref.</param>
		/// <param name="AssetType">Type of object to load.</param>
		/// <returns>Object instance loaded, if found. Otherwise, null.</returns>
		public Object? LoadAsset(AssetRef AssetPath, Type AssetType)
		{
			Object? ret = null;

			// Load the config
			var he_cfg = SettingsDev.instance;

			// If using asset bundles
			if(he_cfg.useAssetBundles)
			{
				Debug.LogError("Not implemented, yet!");
			}
			else
			{
				// Has a bundle ID
				uint? bundle_id = AssetPath.bundleId;
				if(bundle_id.HasValue)
				{
					// Fin by the bundle ID
					foreach((string path, AssetBundleDesc bundleDesc) it in m_AssetsBundleDesc.Where(It => It.bundleDesc.bundleId == bundle_id.Value))
					{
						ret = UnityEditor.AssetDatabase.LoadAssetAtPath(it.path + "/" + AssetPath.assetPath,
							AssetType
						);
						break;
					}
				}
				// Otherwise, use bundle name
				else
				{
					foreach((string path, AssetBundleDesc bundleDesc) it in m_AssetsBundleDesc.Where(It => It.bundleDesc.fileName == AssetPath.bundle))
					{
						ret = UnityEditor.AssetDatabase.LoadAssetAtPath(it.path + "/" + AssetPath.assetPath,
							AssetType
						);
						break;
					}
				}
			}

			return ret;
		}

		/// <summary>
		/// Load asset from the AssetRef.
		/// </summary>
		/// <param name="AssetPath">Asset ref.</param>
		/// <typeparam name="T">Type of object to load.</typeparam>
		/// <returns>Object instance loaded, if found. Otherwise, null.</returns>
		public T? LoadAsset<T>(AssetRef AssetPath) where T : Object
		{
			return LoadAsset(AssetPath,
				typeof(T)
			) as T;
		}

		/// <summary>
		/// Get the <see cref="AssetRef"/> from the asset.
		/// </summary>
		/// <param name="Asset"></param>
		/// <returns></returns>
		public AssetRef? GetAssetRefFromAsset(Object Asset)
		{
			AssetRef? ret = null;

			string asset_path = UnityEditor.AssetDatabase.GetAssetPath(Asset).ToLower();
			// Traverse all the bundle desc
			foreach((string path, AssetBundleDesc bundleDesc) it in m_AssetsBundleDesc.Where(Value => asset_path.Contains(Value.path)))
			{
				ret = new AssetRef(
					UtilsPath.NormalizePath(
						Path.GetRelativePath(it.path,
							asset_path
						)
					),
					it.bundleDesc.bundleId
				);

				break;
			}

			return ret;
		}

		/// <summary>
		/// Reload the data.
		/// </summary>
		public void Reload()
		{
			m_AssetsBundleDesc.Clear();

			// Read all the asset bundle descriptors
			foreach(AssetBundleDesc? it in UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", nameof(AssetBundleDesc))).Select(UnityEditor.AssetDatabase.GUIDToAssetPath).Select(UnityEditor.AssetDatabase.LoadAssetAtPath<AssetBundleDesc>))
			{
				m_AssetsBundleDesc.Add(
					(UtilsPath.NormalizePath(Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(it))!), it)
				);
			}
		}
#endregion
	}
}
#endif