using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ja2
{
	/// <summary>
	/// Asset manager.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Asset Manager", fileName = "AssetManager")]
	public sealed class AssetManager : ScriptableObjectManager<AssetManager>
	{
#region Fields
		/// <summary>
		/// All the present asset bundles.
		/// </summary>
		private BundleData[] m_Bundles = Array.Empty<BundleData>();

		/// <summary>
		/// All the loaded bundles.
		/// </summary>
		private readonly List<BundleState> m_LoadedBundles = new ();
#endregion

#region Methods
		/// <summary>
		/// Load asset from the AssetRef.
		/// </summary>
		/// <param name="AssetPath">Asset reference.</param>
		/// <typeparam name="T">Type of the asset to load.</typeparam>
		/// <returns>Loaded asset if found. Otherwise, null.</returns>
		public async UniTask<T?> LoadAssetAsync<T>(AssetRef AssetPath) where T : Object
		{
			T? ret = null;

			var all_objs_loaded = Array.Empty<Object>();

			var use_asset_bundles = true;
#if UNITY_EDITOR
			// Load the config.
			var he_cfg = SettingsDev.instance;

			// Not using asset bundles, load the one found
			if(he_cfg != null && !he_cfg.useAssetBundles)
			{
				// Use editor asset manager so there is no code duplication
				var asset_loaded = EditorAssetManager.instance.LoadAsset<T>(AssetPath);
				if(asset_loaded != null)
					all_objs_loaded = new Object[]
					{
						asset_loaded
					};

				use_asset_bundles = false;
			}
#endif

			if(use_asset_bundles)
			{
				BundleData? bundle_data = null;

				// Bundle is explicit
				if(!string.IsNullOrEmpty(AssetPath.bundle))
				{
					// Bundle ID to load
					uint? bundle_id = AssetPath.bundleId;

					// Try to find the bundle
					foreach(BundleData it in m_Bundles)
					{
						// Using bundle ID
						if(bundle_id.HasValue)
						{
							if(it.m_BundleInfo.bundleId == bundle_id.Value)
							{
								bundle_data = it;
								break;
							}
						}
						// Using bundle name
						else
						{
							if(it.m_BundleName == AssetPath.bundle)
							{
								bundle_data = it;
								break;
							}
						}
					}
				}
				else
				{
					// Traverse all the loaded bundles and find by path
					bundle_data = m_Bundles.Where(It => It.m_Paths.ContainsKey(AssetPath.assetPath)).Cast<BundleData?>().FirstOrDefault();
				}

				// Bundle found
				if(bundle_data.HasValue)
				{
					// Find if the bundle is loaded already
					AssetBundle? bundle = m_LoadedBundles.Where(It => It.m_BundleId == bundle_data.Value.m_BundleInfo.bundleId).Cast<BundleState?>().FirstOrDefault()?.m_Bundle;


					// Not loaded yet
					if(bundle == null)
					{
						bundle = await AssetBundle.LoadFromFileAsync(bundle_data.Value.m_BundlePath);
						m_LoadedBundles.Add(
							new BundleState(bundle_data.Value.m_BundleInfo.bundleId,
								bundle
							)
						);
					}

					all_objs_loaded = await bundle.LoadAssetWithSubAssetsAsync(AssetPath.assetPath).AwaitForAllAssets();
				}
			}

			// Try to cast to the right type
			foreach(Object it_obj in all_objs_loaded.Where(Obj => Obj != null))
			{
				if(it_obj is T casted)
				{
					ret = casted;
					break;
				}
			}

			return ret;
		}
#endregion

#region Constructors
		/// <inheritdoc />
		protected override void DoInitialize(params object[] Params)
		{
			Ja2Logger.LogInfo("Loading all bundles ...");

			var asset_bundle_dir = string.Empty;

#if UNITY_EDITOR
			var cfg = SettingsDev.instance;
			if(cfg != null)
				asset_bundle_dir = cfg.bundleExportDir;
#else
				asset_bundle_dir = Application.streamingAssetsPath;
#endif

			// \TODO Maybe use some path from game config?
			// Read all the bundles
			string[] files = Directory.GetFiles(asset_bundle_dir,
				"*.bundle"
			);

			var bundle_data_list = new List<BundleData>();

			foreach(string file_name in files)
			{
				Ja2Logger.LogInfo("  Parsing asset bundle '{0}' ...",
					file_name
				);

				// Load the bundle
				AssetBundle bundle = AssetBundle.LoadFromFile(file_name);
				// Load the bundle info
				var ab_info = bundle.LoadAsset<AssetBundleInfo>(AssetBundleInfo.FileName);

				var bundle_data = new BundleData(
					UtilsPath.NormalizePath(
						Path.GetFileNameWithoutExtension(file_name)
					),
					file_name,
					ab_info
				);

				// Fill the content
				for(var i = 0; i < ab_info.assetNames.Length; ++i)
				{
					string asset_name = ab_info.assetNames[i];

					bundle_data.m_Paths[asset_name] = ab_info.assetGUIDs[i];
				}

				bundle_data_list.Add(bundle_data);
			}

			m_Bundles = bundle_data_list.ToArray();

			// Free the loaded bundles
			AssetBundle.UnloadAllAssetBundles(true);
		}

		/// <inheritdoc />
		protected override void DoDeinitialize()
		{
			m_Bundles = Array.Empty<BundleData>();
			m_LoadedBundles.Clear();

#if UNITY_EDITOR
			// Only for testing inside editor
			AssetBundle.UnloadAllAssetBundles(true);
#endif
		}
#endregion
	}

	/// <summary>
	/// Bundle data.
	/// </summary>
	internal readonly struct BundleData
	{
#region Fields
		/// <summary>
		/// Aseet bundle name.
		/// </summary>
		public readonly string m_BundleName;

		/// <summary>
		/// Asset bundle path.
		/// </summary>
		public readonly string m_BundlePath;

		/// <summary>
		/// Asset bundle info.
		/// </summary>
		public readonly AssetBundleInfo m_BundleInfo;

		/// <summary>
		/// Mapping from path -> GUI.
		/// </summary>
		public readonly Dictionary<string, string> m_Paths;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="BundleName">Bundle name.</param>
		/// <param name="BundlePath">Bundle path.</param>
		/// <param name="BundleInfo">Asset bundle info.</param>
		public BundleData(string BundleName, string BundlePath, AssetBundleInfo BundleInfo)
		{
			m_BundleName = BundleName;
			m_BundlePath = BundlePath;
			m_BundleInfo = BundleInfo;
			m_Paths = new Dictionary<string, string>();
		}
#endregion
	}

	/// <summary>
	/// State of the bundles.
	/// </summary>
	internal readonly struct BundleState
	{
#region Fields
		/// <summary>
		/// Asset bundle ID.
		/// </summary>
		public readonly uint m_BundleId;

		/// <summary>
		/// Loaded asset bundle.
		/// </summary>
		public readonly AssetBundle m_Bundle;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="BundleId">Asset bundle Id.</param>
		/// <param name="Bundle">Asset bundle instance.</param>
		public BundleState(uint BundleId, AssetBundle Bundle)
		{
			m_BundleId = BundleId;
			m_Bundle = Bundle;
		}
#endregion
	}
}
