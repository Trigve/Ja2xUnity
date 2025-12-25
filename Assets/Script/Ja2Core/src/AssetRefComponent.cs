using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Component for holding the asset reference instance.
	/// </summary>
	public sealed class AssetRefComponent : MonoBehaviour
	{
#region Fields
		/// Asset reference instance.
		[SerializeField]
		private AssetRef m_AssetRef;
#endregion

#region Properties
		/// <summary>
		/// Asset reference instance.
		/// </summary>
		public AssetRef assetRef => m_AssetRef;
#endregion
	}
}
