using System.IO;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ja2
{
	/// <summary>
	/// Settings used while developement.
	/// </summary>
	public sealed class SettingsDev : ScriptableObject
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

#region Fields Static
		/// <summary>
		/// Static instance.
		/// </summary>
		private static SettingsDev? m_Instance;
#endregion

#region Properties Static
		/// <summary>
		/// Get current instance.
		/// </summary>
		/// <exception cref="FileNotFoundException"></exception>
		public static SettingsDev instance
		{
			get
			{
#if UNITY_EDITOR
				// Not loaded yet
				if(m_Instance is null)
				{
					string[] assets_found = AssetDatabase.FindAssets("t:SettingsDev");
					if(assets_found.Length != 0)
					{
						m_Instance = AssetDatabase.LoadMainAssetAtPath(
							AssetDatabase.GUIDToAssetPath(assets_found[0])
						) as SettingsDev;
					}
				}

				// Still not found
				if(m_Instance is null)
					throw new FileNotFoundException("Cannor find dev settings asset.");
#endif
				return m_Instance;
			}
		}
#endregion

#region Methods Static
#if UNITY_EDITOR
		/// <summary>
		/// Context menu asset creation.
		/// </summary>
		[MenuItem("Assets/Create/JA2 Devel Settings", false, 1)]
		private static void CreateContext()
		{
			ProjectWindowUtil.CreateAsset(Create(),
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
