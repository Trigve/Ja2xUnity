using System.Collections.Generic;
using System.IO;
using System.Linq;

using TMPro;

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
				string bundle_name = UtilsPath.NormalizePath(
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

		/// <summary>
		/// Post-process the imported fonts.
		/// </summary>
		[MenuItem("JA2/Post-process fonts")]
		private static void MenuFontPostProcess()
		{
			// JA2 font path
			string font_path = Path.Combine(SettingsDev.instance.m_SlfExtractDir, "fonts");

			// Find all the JA2 font assets
			var asset_paths = AssetDatabase.FindAssets("t:AssetJa2Font",
				new[]
				{
					font_path
				}
			).Select(Guid => AssetDatabase.GUIDToAssetPath(Guid));

			// Dictionary for the font classes
			var font_class_dict = new Dictionary<string, AssetFontClass>();

			// Proces one by one
			foreach(string it in asset_paths)
			{
				// Load the main asset
				var asset_ja2_font = (AssetDatabase.LoadMainAssetAtPath(it) as AssetJa2Font)!;

				string font_name = asset_ja2_font.name;

				// Arial 10
				if(asset_ja2_font.name is "font10arial" or "font10arialbold")
					font_name = "font10arial";

				// Make it a "full" path
				string font_asset_path = Path.Combine(Path.GetDirectoryName(it)!,
					font_name
				);

				// Get or create the asset in the dict
				if(!font_class_dict.TryGetValue(font_asset_path, out AssetFontClass font_class))
				{
					font_class = AssetFontClass.Create(AssetFontClass.FontType.BitmapFont,
						(int)asset_ja2_font.font.faceInfo.pointSize
					);
					font_class.name = font_name;

					font_class_dict[font_asset_path] = font_class;
				}

				// Load the fonts and palette
				var font_asset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(it);
				var palette_asset = AssetDatabase.LoadAssetAtPath<AssetStciPalette>(it);

				// Arial 10
				if(asset_ja2_font.name is "font10arial" or "font10arialbold")
				{
					if(asset_ja2_font.name == "font10arial")
					{
						font_class.AddFontBase(font_asset,
							palette_asset
						);
					}
					else if(asset_ja2_font.name == "font10arialbold")
					{
						font_class.AddFontBold(font_asset,
							palette_asset
						);
					}
				}
				else
				{
					font_class.AddFontBase(font_asset,
						palette_asset
					);
				}
			}

			// Save the new assets
			foreach(var it in font_class_dict)
			{
				// Asset path for the font class
				string font_class_path = it.Key + ".asset";

				// New asset
				if(!AssetDatabase.AssetPathExists(font_class_path))
				{
					// Save the asset
					AssetDatabase.CreateAsset(it.Value,
						font_class_path
					);
				}
				// Asset update
				else
				{
					var loaded_asset = AssetDatabase.LoadAssetAtPath<AssetFontClass>(font_class_path);
					EditorUtility.CopySerialized(it.Value,
						loaded_asset
					);

				}
			}
		}
#endregion
	}
}
