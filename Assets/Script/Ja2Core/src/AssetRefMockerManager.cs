using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

using Cysharp.Threading.Tasks;

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
		/// True, if the system is in a batch mode.
		/// </summary>
		private bool m_IsBatchMode;

		/// <summary>
		/// Asset manager.
		/// </summary>
		private AssetManager m_AssetManager = null!;

		/// <summary>
		/// All the mock data.
		/// </summary>
		private List<AssetRefMockerInstance> m_AssetMocks = new();
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void StartBatchMode()
		{
			Assert.IsFalse(m_IsBatchMode);
			m_IsBatchMode = true;
		}

		/// <inheritdoc/>
		public void StopBatchMode(bool DoLoadAssets)
		{
			m_IsBatchMode = false;

			if(DoLoadAssets)
				LoadAssets();
		}

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

			// Not called during batch mode, load immediately
			if(!m_IsBatchMode)
				LoadAssets();
		}

		/// <inheritdoc/>
		public void LoadAssets()
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
						asset_loaded = m_AssetManager.LoadAsset(it_ref,
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

			MarkAssetsLoaded();
		}

		/// <inheritdoc/>
		public async UniTask LoadAssetsAsync()
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
						asset_loaded = await m_AssetManager.LoadAssetAsync(it_ref,
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

			MarkAssetsLoaded();
		}
#endregion

#region Methods Private
		/// <summary>
		/// Mark, that asset were loaded.
		/// </summary>
		private void MarkAssetsLoaded()
		{
			m_AssetMocks.Clear();
		}
#endregion

#region Construction
		/// <summary>
		/// Initialization.
		/// </summary>
		/// <param name="AssetManager">Asset manager instance.</param>
		public void Initialize(AssetManager AssetManager)
		{
			m_AssetManager = AssetManager;
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
