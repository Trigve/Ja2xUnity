using System.IO;
using System.Linq;

using UnityEngine;

using UnityEditor;
using UnityEditor.U2D;

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
						// Only file name of the asset
						string asset_file_name = Path.GetFileName(assetPath);

						// All should be fonts by default
						stci_importer.assetType = StciImporter.AssetType.Font;

						// Found the section and file
						if(ImportDataSettings.instance.FindSection("fonts")?.FindFile(asset_file_name) is ImportDataFileFont font_section)
						{
							stci_importer.assetType = font_section.assetType;
							stci_importer.fontPointSize = font_section.fontPointSize;
							stci_importer.descentLine = font_section.fontDescentLine;
						}
					}
				}
			}
		}

		public static void OnPostprocessAllAssets(string[] ImportedAssets, string[] DeletedAssets, string[] MovedAssets, string[] MovedFromAssetPaths, bool DidDomainReload)
		{
			foreach(string it in ImportedAssets)
			{
				// Intereseted only for STCI sprite atlas assets
				if(AssetImporter.GetAtPath(it) is StciImporter stci_importer && stci_importer.assetType == StciImporter.AssetType.SpriteAtlas)
				{
					// Atlas dir
					string atlas_dir = Path.GetDirectoryName(it) + "/" + StciImporter.AtlasDir;

					// Load the asset
					var asset_stci = AssetDatabase.LoadAssetAtPath<AssetStci>(it);

					// 1. pass - initial import/reimport
					if(!StciImporter.IsApplyingRemap(it))
					{
						// Delete old mappings
						foreach(string it_del in StciImporter.MappingPendingDeletes(it))
							AssetDatabase.DeleteAsset(it_del);

						// Asset path filename only
						string asset_file_name = Path.GetFileName(it);

						// Load all the texture sub-assets
						var textures = AssetDatabase.LoadAllAssetRepresentationsAtPath(it).OfType<Texture2D>().ToArray();

						for(var i = 0; i < textures.Length; ++i)
						{
							Texture2D texture = textures[i];

							// New texture path (ignore in asset bundles, because they aren't used once the sprite
							// atlas is generated)
							string texture_path = atlas_dir + "/" + Utils.GeneratePath(asset_file_name, Utils.PathAttribute.Ignore) + "/" + texture.name + ".asset";

							Directory.CreateDirectory(
								Path.GetDirectoryName(texture_path)!
							);

							// Clone the texture, that will be used as mapping
							Texture2D texture_new = Texture2D.Instantiate(texture);

							AssetDatabase.CreateAsset(texture_new,
								texture_path
							);

							// Detach the old (sub-asset) texture
							AssetDatabase.RemoveObjectFromAsset(texture);

							// Remapping of the texture
							stci_importer.AddRemap(
								StciImporter.GenerateAssetIdentifier<Texture2D>(i),
								texture_new
							);
						}

						// Trigger 2. pass in importer
						if(textures.Length > 0)
						{
							StciImporter.ApplyRemap(it);
							stci_importer.SaveAndReimport();
						}
					}
					// 2. pass - remapping is being applied (which was triggered by our code, see above), create the atlas finally
					else
					{
						// Create the atlas and add all the sprites
						var sprite_atlas = new SpriteAtlasAsset();
						// ReSharper disable once CoVariantArrayConversion
						sprite_atlas.Add(asset_stci.sprites);

						string atlas_path = atlas_dir + "/" + Path.GetFileNameWithoutExtension(it) + ".atlas.spriteatlasv2";

						// Save
						SpriteAtlasAsset.Save(sprite_atlas,
							atlas_path
						);

						// Reimport the asset so the AssetDatabase is refreshed (otherwise there could be warning
						// about the mismatch of the time created and time registered in AssetDatabase)
						AssetDatabase.ImportAsset(atlas_path,
							ImportAssetOptions.ForceSynchronousImport
						);

						// Reimport with new settings
						var sprite_atlas_importer = (SpriteAtlasImporter)AssetImporter.GetAtPath(atlas_path);

						SpriteAtlasPackingSettings packing_settings = sprite_atlas_importer.packingSettings;
						packing_settings.enableAlphaDilation = true;
						packing_settings.enableRotation = false;
						packing_settings.padding = 2;
						packing_settings.enableTightPacking = false;

						SpriteAtlasTextureSettings texture_settings = sprite_atlas_importer.textureSettings;
						texture_settings.filterMode = FilterMode.Point;
						texture_settings.generateMipMaps = false;

						sprite_atlas_importer.includeInBuild = false;
						sprite_atlas_importer.packingSettings = packing_settings;
						sprite_atlas_importer.textureSettings = texture_settings;
						sprite_atlas_importer.SetPlatformSettings(new TextureImporterPlatformSettings()
							{
								textureCompression = TextureImporterCompression.CompressedHQ,
								crunchedCompression = true,
								compressionQuality = 100,
							}
						);

						sprite_atlas_importer.SaveAndReimport();
					}
				}
			}
		}
#endregion
	}
}
