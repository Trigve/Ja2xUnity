using System;

using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Interface for AssetRef mockers.
	/// </summary>
	public interface IAssetRefMocker
	{
#region Properties
		/// <summary>
		/// Asset types used.
		/// </summary>
		public Type[] assetType { get; }

#if UNITY_EDITOR
		/// <summary>
		/// Return the managed components, that's need to be tracked in the editor.
		/// </summary>
		public Component componentsModified { get; }
#endif

#endregion

#region Methods Public
		/// <summary>
		/// Load the assets from the mock data.
		/// </summary>
		/// <param name="MockData">Mock data for the given component.</param>
		public void LoadAssets(AssetMockData MockData);

#if UNITY_EDITOR
		/// <summary>
		/// Gather the mock data for the given component.
		/// </summary>
		/// <returns>Data for the current component. Null, if some error occured.</returns>
		public AssetMockData? GatherAssets();
#endif

#endregion
	}
}
