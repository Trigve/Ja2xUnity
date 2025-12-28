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
		private readonly Dictionary<string, BundleData> m_Bundles = new ();

		/// <summary>
		/// All the loaded bundles.
		/// </summary>
		private readonly Dictionary<string, AssetBundle> m_LoadedBundles = new ();
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
					all_objs_loaded = new Object[] { asset_loaded };

				use_asset_bundles = false;
			}
#endif

			if(use_asset_bundles)
			{
				// Bundle is explicit
				if(!string.IsNullOrEmpty(AssetPath.bundle))
				{
					// Not loaded yet
					if(!m_LoadedBundles.TryGetValue(AssetPath.bundle, out AssetBundle? bundle))
					{
						bundle = await AssetBundle.LoadFromFileAsync(m_Bundles[AssetPath.bundle.ToLower()].m_BundlePath);
						m_LoadedBundles[AssetPath.bundle] = bundle;
					}

					all_objs_loaded = await bundle.LoadAssetWithSubAssetsAsync(AssetPath.assetPath).AwaitForAllAssets();
				}
				else
				{
					// If bundle exists
					if(m_Bundles.TryGetValue(AssetPath.assetPath, out BundleData? bundle_data))
					{
						// Bundle already loaded
						if(!m_LoadedBundles.TryGetValue(bundle_data.m_BundlePath, out AssetBundle? bundle))
						{
							bundle = await AssetBundle.LoadFromFileAsync(AssetPath.bundle);
							m_LoadedBundles[AssetPath.bundle] = bundle;
						}

						all_objs_loaded = await bundle.LoadAssetWithSubAssetsAsync(AssetPath.assetPath).AwaitForAllAssets();
					}
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

			foreach(string file_name in files)
			{
				// Load the bundle
				AssetBundle bundle = AssetBundle.LoadFromFile(file_name);

				var bundle_data = new BundleData(file_name);

				// Load the bundle info
				var ab_info = bundle.LoadAsset<AssetBundleInfo>(AssetBundleInfo.FileName);

				// Fill the content
				for(var i = 0; i < ab_info.assetNames.Length; ++i)
				{
					string asset_name = ab_info.assetNames[i];

					bundle_data.m_Paths[asset_name] = ab_info.assetGUIDs[i];
				}

				m_Bundles[Path.GetFileNameWithoutExtension(file_name).ToLower()] = bundle_data;
			}

			// Free the loaded bundles
			AssetBundle.UnloadAllAssetBundles(true);
		}

		/// <inheritdoc />
		protected override void DoDeinitialize()
		{
			m_Bundles.Clear();
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
	internal sealed class BundleData
	{
#region Fields
		/// <summary>
		/// Asset bundle path.
		/// </summary>
		public readonly string m_BundlePath;

		/// <summary>
		/// Mapping from path -> GUI.
		/// </summary>
		public readonly Dictionary<string, string> m_Paths = new();
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="BundlePath">Bundle path.</param>
		public BundleData(string BundlePath)
		{
			m_BundlePath = BundlePath;
		}
#endregion
	}
}
