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
		private AssetRefMockerInstance[] m_AssetMocks = Array.Empty<AssetRefMockerInstance>();
#endregion

#region Methods Public
		/// <summary>
		/// Load all the assets from the AssetRefs.
		/// </summary>
		/// <param name="Manager"></param>
		public async UniTask LoadAssets(AssetManager Manager)
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
