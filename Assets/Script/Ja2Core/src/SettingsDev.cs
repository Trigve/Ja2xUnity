using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Settings used while developement.
	/// </summary>
	public sealed class SettingsDev : ScriptableObjectSettings<SettingsDev>
	{
#region Fields
		/// <summary>
		/// Directory used for imports (.slf, ...)
		/// </summary>
		public string m_InputDir = string.Empty;

		/// <summary>
		/// Data directory used.
		/// </summary>
		public string m_DataDir = string.Empty;

		/// <summary>
		/// Directory used for extracting the .SLF to.
		/// </summary>
		public string m_SlfExtractDir = string.Empty;

		/// <summary>
		/// Directory with user saved data.
		/// </summary>
		public string m_UserDir = string.Empty;

		/// <summary>
		/// Directory for the utilities.
		/// </summary>
		public string m_BinDir = string.Empty;

		/// See <see cref="bundleExportDir"/>.
		[SerializeField]
		private string m_BundleExportDir = string.Empty;

		/// See <see cref="useAssetBundles"/>.
		[SerializeField]
		private bool m_UseAssetBundles;
#endregion

#region Properties
		/// <summary>
		/// Asset bundle export directory.
		/// </summary>
		public string bundleExportDir => m_BundleExportDir;

		/// <summary>
		/// Uset asset bundles when loading the assets.
		/// </summary>
		public bool useAssetBundles => m_UseAssetBundles;
#endregion

#region Methods Static
#if UNITY_EDITOR
		/// <summary>
		/// Context menu asset creation.
		/// </summary>
		[UnityEditor.MenuItem("Assets/Create/JA2 Devel Settings", false, 1)]
		private static void CreateContext()
		{
			UnityEditor.ProjectWindowUtil.CreateAsset(Create(),
				"Ja2SettingsDev.asset"
			);
		}
#endif
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>New instance.</returns>
		private static SettingsDev Create()
		{
			var ret = CreateInstance<SettingsDev>();
			ret.m_InputDir = Application.streamingAssetsPath;
			ret.m_DataDir = "Assets/Data";

			return ret;
		}
#endregion
	}
}
