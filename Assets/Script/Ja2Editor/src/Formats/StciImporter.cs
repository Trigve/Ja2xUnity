using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.TextCore;

using UnityEditor;
using UnityEditor.AssetImporters;

using TMPro;

namespace Ja2.Editor
{
	/// <summary>
	/// STCI format importer.
	/// </summary>
	[ScriptedImporter(1, "sti")]
	public sealed class StciImporter : ScriptedImporter
	{
#region Constants
		/// <summary>
		/// Directory name where the atlases are stored.
		/// </summary>
		public const string AtlasDir = "atlas";
#endregion

#region Enums
		/// <summary>
		/// Type of the asset produced.
		/// </summary>
		public enum AssetType
		{
			Generic,
			Font,
			SpriteAtlas,
		}
#endregion

#region Fields Component
		/// <summary>
		/// See <see cref="assetType"/>.
		/// </summary>
		[Header("Import Settings")]
		[SerializeField]
		private AssetType m_AssetType;

		/// <summary>
		/// PPU for the sprite.
		/// </summary>
		[SerializeField]
		private float m_PixelsPerUnit = 100f;

		/// <summary>
		/// Filter mode for the textures.
		/// </summary>
		[SerializeField]
		private FilterMode m_FilterMode = FilterMode.Point;

		/// <summary>
		/// Should the textures be kept as readable during runtime.
		/// </summary>
		[SerializeField]
		private bool m_KeepTextureReadable;

		/// <summary>
		/// See <see cref="fontPointSize"/>.
		/// </summary>
		[SerializeField]
		private int m_FontPointSize;

		/// <summary>
		/// See <see cref="descentLine"/>.
		/// </summary>
		[SerializeField]
		private int m_DescentLine;
#endregion

#region Fields Static
		/// <summary>
		/// Paths to asssets which import was triggered by our own postprocessor and not by ueser.
		/// </summary>
		private static readonly HashSet<string> m_ApplyingRemap = new();

		/// <summary>
		/// Old external assets that needs to be deleted by asset postprocessor.
		/// </summary>
		private static readonly Dictionary<string, List<string>> m_PendingDeletes = new();
#endregion

#region Properties
		/// <summary>
		/// Font point size.
		/// </summary>
		public int fontPointSize
		{
			set => m_FontPointSize = value;
		}

		/// <summary>
		/// Descent line position.
		/// </summary>
		public int descentLine
		{
			set => m_DescentLine = value;
		}

		/// <summary>
		/// Is it a font asset.
		/// </summary>
		public AssetType assetType
		{
			get => m_AssetType;
			set => m_AssetType = value;
		}
#endregion

#region Methods Public Static
		/// <summary>
		/// Generate the texture name.
		/// </summary>
		/// <param name="Index">Index of the texture.</param>
		/// <returns></returns>
		internal static string GenerateTextureName(int Index)
		{
			return "texture_" + Index;
		}

		/// <summary>
		/// Generate the asset identifier.
		/// </summary>
		/// <param name="Index">Index of the asset.</param>
		/// <typeparam name="T">Type of the asset.</typeparam>
		/// <returns>New identifier based on the type and index.</returns>
		internal static SourceAssetIdentifier GenerateAssetIdentifier<T>(int Index)
		{
			return new SourceAssetIdentifier(
				typeof(T),
				typeof(T).Name + "_" + Index
			);
		}

		/// <summary>
		/// Check if for given asset the remapping is being done.
		/// </summary>
		/// <param name="AssetPath">Asset path.</param>
		/// <returns>True, if the remapping is being done. Otherwise, false.</returns>
		internal static bool IsApplyingRemap(string AssetPath)
		{
			return m_ApplyingRemap.Remove(AssetPath);
		}

		/// <summary>
		/// Return all mapping asset paths, that needs to be deleted for the given asset.
		/// </summary>
		/// <param name="AssetPath">Asset path, for which pending deletes are obtained.</param>
		/// <returns>Assets that needs to be deleted.</returns>
		internal static IEnumerable<string> MappingPendingDeletes(string AssetPath)
		{
			// Remove from the dict only if exist
			if(m_PendingDeletes.Remove(AssetPath, out var deletes))
				return deletes;

			// Nothing here to remove
			return Enumerable.Empty<string>();
		}

		/// <summary>
		/// Signal that for the given asset, the remapping is being done.
		/// </summary>
		/// <param name="AssetPath">Asset, for which remapping is being done.</param>
		internal static void ApplyRemap(string AssetPath)
		{
			m_ApplyingRemap.Add(AssetPath);
		}
#endregion

#region Methods Public
		/// <inheritdoc/>
		public override void OnImportAsset(AssetImportContext Context)
		{
			string asset_file_name = Path.GetFileNameWithoutExtension(Context.assetPath);

			// Font asset
			if(m_AssetType == AssetType.Font)
			{
				// All the letters inside the font
				const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_+=|\\{}[]:;\"'<>,.?/ ÄÖÜäöüßабвгдеёжзийклмнопрстуфхцчшщыьэюяÀÁÂÇËÈÉÊÏÒÓÔÙÚÛàáâçëèéêïòóôùúûÌìĄĆĘŁŃŚŻŹąćęłńśżź";

				// No need to have texture readable
				m_KeepTextureReadable = false;

				// Parse the STCI as font
				STCIData stci_data = STCIUtils.Load(
					File.ReadAllBytes(Context.assetPath),
					STCIUtils.ExtractionFlags.FontShadow | STCIUtils.ExtractionFlags.Palette
				);

				var textures = new Texture2D[stci_data.m_SubImageData.Count];

				var max_height = 0;

				{
					var i = 0;
					// Process all the subimages
					foreach(STCIData.SubImage it in stci_data.m_SubImageData)
					{
						// As first, create the texture
						var texture = new Texture2D(it.width,
							it.height,
							stci_data.m_ImageFormat,
							false
						);
						texture.filterMode = m_FilterMode;
						texture.wrapMode = TextureWrapMode.Clamp;
						texture.name = GenerateTextureName(i);

						texture.SetPixels32(it.texture);

						// For fonts, texture needs to be readable
						texture.Apply(false,
							false
						);

						textures[i] = texture;

						max_height = Mathf.Max(max_height,
							texture.height
						);

						++i;
					}
				}

				// \TODO Could use 16 bit format to save some space
				// Create and fill the texture atlas
				var texture_atlas = new Texture2D(256,
					256,
					TextureFormat.ARGB32,
					1,
					false
				);
				texture_atlas.filterMode = m_FilterMode;
				texture_atlas.wrapMode = TextureWrapMode.Clamp;
				texture_atlas.name = asset_file_name + " Font Atlas";

				// Clear the texture
				texture_atlas.SetPixels32(
					Enumerable.Repeat(new Color32(),
						texture_atlas.width * texture_atlas.height
					).ToArray()
				);

				// Pack all the font textures to the atlas. Using our packer, because Texture2D.PackTextures() doesn't
				// add padding to the texture, with are near the edge (x = 0 for instance) and the font then renders
				// incorrectly
				var uvs = TextureAtlas.Create(texture_atlas,
					textures,
					1
				);

				// Destroy the source textures
				foreach(Texture2D it in textures)
					DestroyImmediate(it);

				// Create the palette as texture array
				var palette_texture_asset = new Texture2DArray(1,
					1,
					stci_data.m_Palette!.count,
					TextureFormat.ARGB32,
					0,
					false
				);
				palette_texture_asset.filterMode = FilterMode.Point;
				palette_texture_asset.wrapMode = TextureWrapMode.Clamp;
				palette_texture_asset.name = asset_file_name + " Palette Texture";

				// Fill the colors
				for(var i = 0; i < palette_texture_asset.depth; ++i)
				{
					palette_texture_asset.SetPixels32(new [] {stci_data.m_Palette![i]},
						i
					);
				}

				// Also store palette asset as is
				AssetStciPalette palette_asset = stci_data.m_Palette;
				palette_asset.name = asset_file_name + " Palette";

				// Create the font and fill the data
				var font_asset = ScriptableObject.CreateInstance<TMP_FontAsset>();
				// Avoid updating the data (crash would happen)
				ClassUtils.FieldSet(font_asset,
					"m_Version",
					"1.1.0"
				);
				font_asset.name = asset_file_name + " Font";
				font_asset.atlasTextures = new[]
				{
					texture_atlas
				};
				ClassUtils.PropertySet(font_asset,
					"atlasWidth",
					texture_atlas.width
				);
				ClassUtils.PropertySet(font_asset,
					"atlasHeight",
					texture_atlas.height
				);
				// Mark that texture atlas should not be re-generated
				font_asset.atlasPopulationMode = AtlasPopulationMode.Static;
				font_asset.isMultiAtlasTexturesEnabled = false;

				ClassUtils.PropertySet(font_asset,
					"atlasPadding",
					0
				);

				// Face metrics
				FaceInfo face_info = font_asset.faceInfo;
				face_info.familyName = font_asset.name;
				face_info.styleName = "Regular";
				face_info.pointSize = m_FontPointSize;
				face_info.baseline = 0;
				face_info.ascentLine = max_height + m_DescentLine;
				face_info.capLine = max_height + m_DescentLine;
				face_info.descentLine = m_DescentLine;
				face_info.scale = 1f;
				// \FIXME What number to use?
				face_info.lineHeight = m_FontPointSize + 1;
				face_info.underlineOffset = face_info.descentLine * 0.5f;
				face_info.underlineThickness = 1f;
				face_info.strikethroughOffset = face_info.ascentLine * 0.4f;
				face_info.strikethroughThickness = 1f;
				// \FIXME What number to use?
				face_info.tabWidth = 5 * 4f;

				font_asset.faceInfo = face_info;

				// Material
				Shader shader = Shader.Find("Ja2/FontShadow");
				var material = new Material(shader)
				{
					name = asset_file_name + " Font Material"
				};
				material.SetTexture(ShaderUtilities.ID_MainTex,
					texture_atlas
				);
				// No SDF spread for raw bitmap
				material.SetFloat(ShaderUtilities.ID_GradientScale,
					1
				);
				// Assign the palette texture to the font material
				material.SetTexture(
					Shader.PropertyToID("_FontPaletteTex"),
					palette_texture_asset
				);

				font_asset.material = material;

				// Glyph + character tables
				var glyph_table = new List<Glyph>();
				var char_table = new List<TMP_Character>();

				for(var i = 0; i < letters.Length; ++i)
				{
					// Letter that is being processed
					char letter = letters[i];

					// If font for the given letter doesn't exist, quit
					if(i >= uvs.Length)
						break;

					// UV coordinates from texture atlas
					Rect letter_uv = uvs[i];

					// Coordinates in the pixels
					var letter_rect = new RectInt(Mathf.RoundToInt(letter_uv.xMin * texture_atlas.width),
						Mathf.RoundToInt(letter_uv.yMin * texture_atlas.height),
						Mathf.RoundToInt(letter_uv.width * texture_atlas.width),
						Mathf.RoundToInt(letter_uv.height * texture_atlas.height)
					);

					var metrics = new GlyphMetrics(letter_rect.width,
						letter_rect.height,
						0,
						face_info.ascentLine,
						// \FIXME What value to use?
						letter_rect.width
					);

					var glyph_rect = new GlyphRect(letter_rect.xMin,
						letter_rect.yMin,
						letter_rect.width,
						letter_rect.height
					);

					glyph_table.Add(
						new Glyph(letter,
							metrics,
							glyph_rect,
							1f,
							0
						)
					);

					char_table.Add(
						new TMP_Character(letter,
							glyph_table[^1]
						)
					);
				}

				ClassUtils.PropertySet(font_asset,
					"glyphTable",
					glyph_table
				);
				ClassUtils.PropertySet(font_asset,
					"characterTable",
					char_table
				);

				// Rebuild the asset
				font_asset.ReadFontAssetDefinition();

				// Create the main asset
				var ja2_font_asset = AssetJa2Font.Create(font_asset,
					true

				);
				ja2_font_asset.name = asset_file_name;

				Context.AddObjectToAsset(ja2_font_asset.name,
					ja2_font_asset
				);

				// Palette
				Context.AddObjectToAsset(palette_texture_asset.name,
					palette_texture_asset
				);

				// Font asset
				Context.AddObjectToAsset(font_asset.name,
					font_asset
				);

				// Texture atlas asset
				Context.AddObjectToAsset(texture_atlas.name,
					texture_atlas
				);

				// Font material asset
				Context.AddObjectToAsset(material.name,
					material
				);

				// Paletter asset
				Context.AddObjectToAsset(palette_asset.name,
					palette_asset
				);

				// Main object is the font asset
				Context.SetMainObject(ja2_font_asset);
			}
			else
			{
				// Parse the STCI as first
				STCIData stci_data = STCIUtils.Load(
					File.ReadAllBytes(Context.assetPath),
					STCIUtils.ExtractionFlags.None
				);

				var textures = new List<Texture2D>();
				var sprites = new List<Sprite>();
				var sub_image_data = new STCISubImageData[stci_data.m_SubImageData.Count];

				// The next steps are done for remapping the sub-asset textures to an external one. This is needed so,
				// the textures, that were packed to sprite atlas, wouldn't be embedded in the asset bundle (which
				// doesn't work with sub-assets, therefore they need to be moved outside).
				//
				// This is done in 2 passes. In the 1. pass, textures are generated as sub-assets. Then in asset
				// postprocessor, those textures are moved outside the asset and new reimport is forced.
				// In this 2. pass, the moved textures are read back and then sprites are created and
				// then in postprocessor sprite atlas is created finally
				if(m_AssetType == AssetType.SpriteAtlas)
				{
					// Textures needs to readable, as they would be read from
					m_KeepTextureReadable = true;

					// Is this a ramapping pass
					bool is_remap_apply_pass = m_ApplyingRemap.Contains(Context.assetPath);

					// Genuine import (first import, manual "Reimport", source file changed, ...), everyting needs
					// to be wiped clear and rebuild from scratch
					if(!is_remap_apply_pass)
					{
						// The mappings that needs to be deleted
						var old_paths = new List<string>();

						// Remove all the current mappings
						foreach(var it in GetExternalObjectMap())
						{
							if(it.Value != null)
							{
								old_paths.Add(
									AssetDatabase.GetAssetPath(it.Value)
								);
							}

							RemoveRemap(it.Key);
						}

						// Mark assets for deletion
						if(old_paths.Count > 0)
							m_PendingDeletes[Context.assetPath] = old_paths;
					}

					// For initial import, this should empty. For the 2. pass, it will contain mappings
					// already
					var asset_map = GetExternalObjectMap();

					for(var i = 0; i < stci_data.m_SubImageData.Count; ++i)
					{
						// Sub-image data from which texture is generated
						STCIData.SubImage sub_image = stci_data.m_SubImageData[i];

						// Unique ID of the asset
						SourceAssetIdentifier texture_id = GenerateAssetIdentifier<Texture2D>(i);

						// 2. pass - found in the map (already move outside the asset)
						if(asset_map.TryGetValue(texture_id, out Object asset) && asset is Texture2D texture)
						{
							string asset_path = AssetDatabase.GetAssetPath(texture);

							// Need to load the asset, otherwise Unity will warn that dependency isn't used
							AssetDatabase.LoadAssetAtPath<Texture2D>(asset_path);

							// Add the source texture as dependency
							Context.DependsOnArtifact(
								AssetDatabase.GUIDFromAssetPath(asset_path)
							);

							// Mark for sprite generation
							textures.Add(texture);
						}
						// 1. pass - need to create a "temporary" texture, don't generate sprites yet
						else
						{
							texture = new Texture2D(sub_image.width,
								sub_image.height,
								stci_data.m_ImageFormat,
								false
							);
							texture.filterMode = m_FilterMode;
							texture.wrapMode = TextureWrapMode.Clamp;
							texture.name = GenerateTextureName(i);

							texture.SetPixels32(sub_image.texture);

							texture.Apply(false,
								!m_KeepTextureReadable
							);

							Context.AddObjectToAsset(texture.name,
								texture,
								texture
							);
						}
					}
				}
				// Generic
				else
				{
					for(var i = 0; i < stci_data.m_SubImageData.Count; ++i)
					{
						// Sub-image data from which texture is generated
						STCIData.SubImage sub_image = stci_data.m_SubImageData[i];

						string texture_name = GenerateTextureName(i);

						var texture = new Texture2D(sub_image.width,
							sub_image.height,
							stci_data.m_ImageFormat,
							false
						);
						texture.filterMode = m_FilterMode;
						texture.wrapMode = TextureWrapMode.Clamp;
						texture.name = GenerateTextureName(i);

						texture.SetPixels32(sub_image.texture);

						texture.Apply(false,
							!m_KeepTextureReadable
						);

						Context.AddObjectToAsset(texture_name,
							texture,
							texture
						);

						textures.Add(texture);
					}
				}

				// Create the sprites, if there are textures (2. pass)
				for(var i = 0; i < textures.Count; ++i)
				{
					// Sub-image data from which texture is generated
					STCIData.SubImage sub_image = stci_data.m_SubImageData[i];

					Texture2D texture = textures[i];

					// JA2 stores a per-tile draw offset rather than a centered pivot, therfore conversion into the
					// sprite pivot space is needed
					var pivot = new Vector2(
						0.5f - sub_image.offsetX / sub_image.width,
						0.5f + sub_image.offsetY / sub_image.height
					);

					var sprite = Sprite.Create(
						texture,
						new Rect(0,
							0,
							sub_image.width,
							sub_image.height
						),
						pivot,
						m_PixelsPerUnit,
						0,
						SpriteMeshType.FullRect
					);

					sprite.name = string.Format("sprite_{0}",
						i
					);
					sprites.Add(sprite);

					// Sub-image data
					sub_image_data[i] = new STCISubImageData
					{
						m_Index = i,
						m_Offset = new Vector2Int(sub_image.offsetX,
							sub_image.offsetY
						)
					};

					// Register as sub-assets
					Context.AddObjectToAsset(
						string.Format("sprite_{0}",
							i
						),
						sprite,
						sprite.texture
					);
				}

				// Build the metadata asset that ties everything together
				var data = AssetStci.Create(asset_file_name,
					stci_data.m_Width,
					stci_data.m_Height,
					textures.ToArray(),
					sprites.ToArray(),
					sub_image_data
				);

				Context.AddObjectToAsset("data",
					data
				);

				// Main object is the data
				Context.SetMainObject(data);
			}
		}
	}
#endregion
}
