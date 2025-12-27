using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Asset bundle descriptor.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create AssetBunde Descriptor", fileName = "BundleDesc.asset")]
	public sealed class AssetBundleDesc : ScriptableObject
	{
#region Fields
		/// <summary>
		/// Version of the bundle.
		/// </summary>
		[SerializeField]
		private uint m_Version;

		/// <summary>
		/// ID of the asset bundle.
		/// </summary>
		[SerializeField]
		private uint m_BundleId;

		/// See <see cref="bundleName"/>.
		[SerializeField]
		private string m_BundleName = string.Empty;

		/// See <see cref="fileName"/>.
		[SerializeField]
		private string m_FileName = string.Empty;
#endregion

#region Properties
		/// <summary>
		/// Unique ID of the bundle.
		/// </summary>
		public uint bundleId => m_BundleId;

		/// <summary>
		/// Bundle name.
		/// </summary>
		public string bundleName => m_BundleName;

		/// <summary>
		/// Exported bunde file name without extension.
		/// </summary>
		public string fileName => m_FileName;
#endregion
	}
}
