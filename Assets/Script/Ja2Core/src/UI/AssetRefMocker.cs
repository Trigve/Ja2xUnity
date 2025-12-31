using System;

using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Helper component for mananing the assets during editor/playmode as <see cref="AssetRef"/>. During edit time,
	/// only "mocks" are saved, not actual assets. Then, during the runtime, the real assets are loaded from the
	/// mocks.
	/// </summary>
	public abstract class AssetRefMocker<T> : MonoBehaviour, IAssetRefMocker where T : Component
	{
#region Fields component
		/// <summary>
		/// Component.
		/// </summary>
		[SerializeField]
		protected T? m_Component;
#endregion

#region Properties
		/// <inheritdoc />
		public abstract Type[] assetType { get; }

#if UNITY_EDITOR
		/// <inheritdoc />
		public Component componentsModified => m_Component!;
#endif

#endregion

#region Messages
		public void Awake()
		{
			// Try to find component, if not assigned already
			if(m_Component == null)
				m_Component = GetComponent<T>();
		}
#endregion

#region Methods Public
		/// <inheritdoc />
		public void LoadAssets(AssetMockData MockData)
		{
			if(m_Component == null)
			{
				Debug.LogErrorFormat("{0}: Component is Null",
					nameof(AssetRefMockerImage)
				);

				return;
			}

			DoLoadAssets(MockData);
		}

#if UNITY_EDITOR
		/// <inheritdoc />
		public AssetMockData? GatherAssets()
		{
			if(m_Component == null)
			{
				Debug.LogErrorFormat("{0}: Component is Null",
					nameof(AssetRefMockerImage)
				);

				return null;
			}

			return DoGatherAssets();
		}
#endif

#endregion

#region Methods Private
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
#endif

#endregion
	}
}
