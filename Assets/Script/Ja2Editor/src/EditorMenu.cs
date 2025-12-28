using System.IO;

using UnityEditor;

namespace Ja2.Editor
{
	/// <summary>
	/// Menu class.
	/// </summary>
	internal static class EditorMenu
	{
#region Methods Static
		/// <summary>
		/// Extract SLF.
		/// </summary>
		[MenuItem("JA2/Extract SLF")]
		private static void MenuSlfExtract()
		{
			// Read all the SLF
			foreach(string it in Directory.EnumerateFiles(SettingsDev.instance.m_InputDir, "*.slf"))
			{
				// Bundle name
				string? bundle_name = UtilsPath.NormalizePath(
					Path.GetFileNameWithoutExtension(it)
				);

				// Bundle path
				string bundle_dir = UtilsPath.Combine(SettingsDev.instance.m_SlfExtractDir,
					bundle_name
				);

				// Be sure directory exist
				if(!Directory.Exists(bundle_dir))
					Directory.CreateDirectory(bundle_dir);

				// Create the asset bundle descriptor
				var bundle_desc = AssetBundleDesc.Create(1,
					(uint)bundle_name.GetHashCode(),
					bundle_name,
					bundle_name + ".bundle"
				);

				// Save the asset bundle descriptor to dir where all the files will be extracted
				AssetDatabase.CreateAsset(bundle_desc,
					UtilsPath.Combine(bundle_dir,
						AssetBundleDesc.FileName
					)
				);

				// As first SLF
				foreach(FileData file_data in SlfManager.ExtractPath(it))
				{
					AssetExtractor.Extract(file_data.data,
						SettingsDev.instance.m_BinDir,
						file_data.path,
						SettingsDev.instance.m_SlfExtractDir
					);
				}
			}

			// Need to reload so the new bundle descriptors are loaded
			EditorAssetManager.instance.Reload();
		}
#endregion
	}
}
