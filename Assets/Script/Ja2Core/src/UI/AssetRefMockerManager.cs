using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Helper class for managing the <see cref="AssetRefMocker{T}"/>.
	/// </summary>
	public sealed class AssetRefMockerManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// All the mock data.
		/// </summary>
		[SerializeField]
		private List<AssetRefMockerInstance> m_AssetMocks = new();
#endregion

#region Methods Public
#if UNITY_EDITOR
		/// <summary>
		/// Add new ref mocker to the manager. Only used in the editor.
		/// </summary>
		/// <param name="MockerComponent">Component to add to the asset list.</param>
		public void AddRefMocker(IAssetRefMocker MockerComponent)
		{
			UnityEditor.Undo.RecordObject(MockerComponent.componentsModified,
				"Clear component data "
			);

			var asset_mock = MockerComponent.GatherAssets();

			if(asset_mock == null)
			{
				Debug.LogWarningFormat("{0}: Component not set for the '{1}'",
					nameof(AssetRefMockerManager),
					((Component)MockerComponent).gameObject
				);

				return;
			}

			var asset_refs = new List<AssetRef>();

			// Load all the asset refs
			foreach(Object? it_asset in asset_mock.Value.m_Assets)
			{
				var asset_ref = new AssetRef();

				// Only if there is some valid asset
				if(it_asset != null)
				{
					var asset_ref_found = EditorAssetManager.instance.GetAssetRefFromAsset(it_asset);

					// \FIXME Asset ref may not be valid when???
					if(asset_ref_found.HasValue)
						asset_ref = asset_ref_found.Value;
				}

				asset_refs.Add(asset_ref);
			}

			// Need to mark it as modified, otherwise, it wouldn't be saved to scene, see
			// https://discussions.unity.com/t/updating-prefab-variable-via-script-doesnt-save-override/727795/5
			UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(MockerComponent.componentsModified);

			// Add new item
			m_AssetMocks.Add(
				new AssetRefMockerInstance(MockerComponent,
					asset_refs.ToArray()
				)
			);
		}
#endif

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
				foreach(AssetRef it_ref in it.m_AssetRefs)
				{
					Object? asset_loaded = null;

					if(it_ref.isValid)
					{
						asset_loaded = await Manager.LoadAssetAsync(it_ref,
							it.component.assetType[0]
						);
					}

					asset_list.Add(asset_loaded);
				}

				it.component.LoadAssets(
					new AssetMockData(
						asset_list.ToArray()
					)
				);
			}
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
		/// Version of the data. Should be updated on the field changes.
		/// </summary>
		[SerializeField]
		public uint m_Version;

		/// <summary>
		/// Component instance.
		/// </summary>
		[SerializeField]
		public Component m_Component;

		/// <summary>
		/// AssetRefs. We cannot use Nullable here, because Unity doesn't support serialization
		/// of the Nullable structs.
		/// </summary>
		[SerializeField]
		public AssetRef[] m_AssetRefs;
#endregion

#region Properties
		/// <summary>
		/// Automatic casting to the interface.
		/// </summary>
		public IAssetRefMocker component => (IAssetRefMocker)m_Component;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Component">Componen used for the given data.</param>
		/// <param name="Data">Asset data.</param>
		public AssetRefMockerInstance(IAssetRefMocker Component, AssetRef[] Data)
		{
			// The most recent one
			m_Version = 1;

			m_Component = (Component)Component;
			m_AssetRefs = Data;
		}
#endregion
	}
}
