using System;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ja2
{
	/// <summary>
	/// Base class for all asset ref mockers.
	/// </summary>
	public abstract class AssetRefMockerBase : MonoBehaviour
	{
#if UNITY_EDITOR
#region Editor Fields
		/// Field names for editor.
		internal const string PropertyNameAssetRefs = nameof(m_AssetRefs);
		internal const string PropertyNameComponent = "m_Component";
#endregion
#endif

#region Fields Component
		/// <summary>
		/// All the <see cref="AssetRef"/> associated.
		/// </summary>
		[SerializeField]
		protected AssetRef[] m_AssetRefs = Array.Empty<AssetRef>();
#endregion

#region Properties
		/// <summary>
		/// Asset types used. If array contains only 1 type, it is used for all the assets. Otherwise, fore each asset
		/// different type is used.
		/// </summary>
		public abstract Type[] assetType { get; }

		/// <summary>
		/// Get all the asset references.
		/// </summary>
		public IEnumerable<AssetRef> assetRefs => m_AssetRefs;

		/// <summary>
		/// Get the associated component.
		/// </summary>
		protected abstract Component? component { get; }

#if UNITY_EDITOR
		/// <summary>
		/// Return the managed component, that's need to be tracked in the editor.
		/// </summary>
		public Component componentsModified => component!;
#endif
#endregion

#region Messages
		public void Awake()
		{
			// Register the mocker
			GameState.instance!.SceneManagerForScene(gameObject.scene).assetRefMockRegistry.RegisterAssetRefMocker(this);

			DoAwake();
		}
#endregion

#region Methods Public
		/// <summary>
		/// Load the assets from the mock data.
		/// </summary>
		/// <param name="MockData">Mock data for the given component.</param>
		public void LoadAssets(AssetMockData MockData)
		{
			if(component == null)
			{
				Debug.LogErrorFormat("{0}: Component is Null",
					GetType().Name
				);

				return;
			}

			DoLoadAssets(MockData);
		}

#if UNITY_EDITOR
		/// <summary>
		/// Gather the mock data for the given component.
		/// </summary>
		/// <returns>Data for the current component. Null, if some error occured.</returns>
		public void GatherAssets()
		{
			if(component == null)
			{
				Debug.LogErrorFormat("{0}: Component is Null",
					GetType().Name
				);

				return;
			}

			AssetMockData ret = DoGatherAssets();

			// Recreate the array
			m_AssetRefs = new AssetRef[ret.m_Assets.Length];

			// Load all the asset refs
			for(var i = 0; i < ret.m_Assets.Length; ++i)
			{
				Object? it_asset = ret.m_Assets[i];
				var asset_ref = new AssetRef();

				// Only if there is some valid asset
				if(it_asset != null)
				{
					var asset_ref_found = EditorAssetManager.instance.GetAssetRefFromAsset(it_asset);

					// \FIXME Asset ref may not be valid when???
					if(asset_ref_found.HasValue)
						asset_ref = asset_ref_found.Value;
				}

				m_AssetRefs[i] = asset_ref;
			}
		}

		/// <summary>
		/// Reset the asset from the component.
		/// </summary>
		public void ResetAssets()
		{
			if(component == null)
			{
				Debug.LogErrorFormat("{0}: Component is Null",
					this.GetType().Name
				);

				return;
			}

			DoResetAssets();
		}
#endif
#endregion

#region Methods Private
		/// <summary>
		/// Implementation on children.
		/// </summary>
		protected virtual void DoAwake()
		{}

		/// <summary>
		/// Implementation.
		/// </summary>
		protected abstract void DoLoadAssets(AssetMockData MockData);

#if UNITY_EDITOR
		/// <summary>
		/// Implementation.
		/// </summary>
		/// <returns></returns>
		protected abstract AssetMockData DoGatherAssets();

		/// <summary>
		/// Implementation.
		/// </summary>
		protected abstract void DoResetAssets();
#endif
#endregion
	}
}
