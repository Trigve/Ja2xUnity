using System.IO;

using UnityEditor;

namespace Ja2.Editor
{
	/// <summary>
	/// STCI asset Preprocessor.
	/// </summary>
	public sealed class StciAssetProcessor : AssetPostprocessor
	{
#region Messages
		public void OnPreprocessAsset()
		{
			// Only STCI importer
			if(assetImporter is StciImporter stci_importer)
			{
				// Only font asset
				if(assetPath.Contains(SettingsDev.instance.m_SlfExtractDir + "/fonts"))
				{
					// Imported first time
					if(assetImporter.importSettingsMissing)
					{
						// All are fonts
						stci_importer.assetType = StciImporter.AssetType.Font;

						// Only file name of the asset
						string asset_file_name = Path.GetFileName(assetPath);

						// Set the importer fields (brute force)
						if(asset_file_name == "font10arial.sti")
						{
							stci_importer.fontPointSize = 10;
							stci_importer.descentLine = -2;
						}
						else if(asset_file_name == "font12arial.sti")
						{
							stci_importer.fontPointSize = 12;
							stci_importer.descentLine = -2;
						}
						else if(asset_file_name == "font14arial.sti")
						{
							stci_importer.fontPointSize = 14;
							stci_importer.descentLine = -3;
						}
					}
				}
			}
		}
#endregion
	}
}
