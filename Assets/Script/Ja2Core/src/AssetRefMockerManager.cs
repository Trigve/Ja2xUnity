using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ja2
{
	/// <summary>
	/// Helper class for managing the <see cref="AssetRefMocker{T}"/>.
	/// </summary>
	public sealed class AssetRefMockerManager : MonoBehaviour, IAssetRefMockRegistry
	{
#region Fields
		/// <summary>
		/// All the mock data.
		/// </summary>
		private List<AssetRefMockerInstance> m_AssetMocks = new();
#endregion

#region Methods Public
		/// <summary>
		/// Register new ref mocker to the manager.
		/// </summary>
		/// <param name="MockerComponent">Component to add to the asset list.</param>
		public void RegisterAssetRefMocker(AssetRefMockerBase MockerComponent)
		{
			// Add new item
			m_AssetMocks.Add(
				new AssetRefMockerInstance(MockerComponent)
			);
		}

		/// <summary>
		/// Load all the assets from the AssetRefs.
		/// </summary>
		/// <param name="Manager"></param>
		public void LoadAssets(AssetManager Manager)
		{
			var asset_list = new List<Object?>();

			// Process all the components
			foreach(AssetRefMockerInstance it in m_AssetMocks)
			{
				asset_list.Clear();

				// Process all the assets
				var i = 0;
				foreach(AssetRef it_ref in it.component.assetRefs)
				{
					// Find the right type
					Type asset_type = it.component.assetType.Length == 1 ? it.component.assetType[0] : it.component.assetType[i];

					Object? asset_loaded = null;

					if(it_ref.isValid)
					{
						asset_loaded = Manager.LoadAsset(it_ref,
							asset_type
						);
					}

					asset_list.Add(asset_loaded);

					++i;
				}

				it.component.LoadAssets(
					new AssetMockData(
						asset_list.ToArray()
					)
				);
			}
		}

		/// <summary>
		/// Load all the assets from the AssetRefs.
		/// </summary>
		/// <param name="Manager"></param>
		public async UniTask LoadAssetsAsync(AssetManager Manager)
		{
			var asset_list = new List<Object?>();

			// Process all the components
			foreach(AssetRefMockerInstance it in m_AssetMocks)
			{
				asset_list.Clear();

				// Process all the assets
				var i = 0;
				foreach(var it_ref in it.component.assetRefs)
				{
					// Find the right type
					Type asset_type = it.component.assetType.Length == 1 ? it.component.assetType[0] : it.component.assetType[i];

					Object? asset_loaded = null;

					if(it_ref.isValid)
					{
						asset_loaded = await Manager.LoadAssetAsync(it_ref,
							asset_type
						);
					}

					asset_list.Add(asset_loaded);

					++i;
				}

				it.component.LoadAssets(
					new AssetMockData(
						asset_list.ToArray()
					)
				);
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Initialization.
		/// </summary>
		public void Initialize()
		{
			m_AssetMocks =  new List<AssetRefMockerInstance>();
		}
#endregion
	}

	/// <summary>
	/// Helper structure for the mock data.
	/// </summary>
	[Serializable]
	internal struct AssetRefMockerInstance
	{
#region Fields
		/// <summary>
		/// Component instance.
		/// </summary>
		[SerializeField]
		public Component m_Component;
#endregion

#region Properties
		/// <summary>
		/// Automatic casting to the interface.
		/// </summary>
		public AssetRefMockerBase component => (AssetRefMockerBase)m_Component;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Component">Component used for the given data.</param>
		public AssetRefMockerInstance(AssetRefMockerBase Component)
		{
			m_Component = Component;
		}
#endregion
	}
}
