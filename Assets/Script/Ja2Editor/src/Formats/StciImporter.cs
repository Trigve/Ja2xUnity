using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.TextCore;

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
#region Fields Component
		/// <summary>
		/// PPU for the sprite.
		/// </summary>
		[Header("Import Settings")]
		[SerializeField]
		private float m_PixelsPerUnit = 100f;

		/// <summary>
		/// Filter mode for th textures.
		/// </summary>
		[SerializeField]
		private FilterMode m_FilterMode = FilterMode.Point;

		/// <summary>
		/// Should the textures be kept as readable during runtime.
		/// </summary>
		[SerializeField]
		private bool m_KeepTextureReadable;

		/// <summary>
		/// Is it a font asset.
		/// </summary>
		[SerializeField]
		private bool m_IsFont;

		/// <summary>
		/// Font point size.
		/// </summary>
		[SerializeField]
		private int m_FontPointSize;

		/// <summary>
		/// Descent line position.
		/// </summary>
		[SerializeField]
		private int m_DescentLine;
#endregion

#region Methods Public
		/// <inheritdoc/>
		public override void OnImportAsset(AssetImportContext Context)
		{
			string asset_file_name = Path.GetFileNameWithoutExtension(Context.assetPath);

			// Font asset
			if(m_IsFont)
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
						texture.name = string.Format("texture_{0}",
							i
						);

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

				var textures = new Texture2D[stci_data.m_SubImageData.Count];
				var sprites = new Sprite[stci_data.m_SubImageData.Count];
				var sub_image_data = new STCISubImageData[stci_data.m_SubImageData.Count];

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
						texture.name = string.Format("texture_{0}",
							i
						);

						texture.SetPixels32(it.texture);

						texture.Apply(false,
							!m_KeepTextureReadable
						);

						textures[i] = texture;

						// JA2 stores a per-tile draw offset rather than a centered pivot, therfore conversion into the
						// sprite pivot space is needed.
						var pivot = new Vector2(
							0.5f - it.offsetX / it.width,
							0.5f + it.offsetY / it.height
						);

						var sprite = Sprite.Create(
							texture,
							new Rect(0,
								0,
								it.width,
								it.height
							),
							pivot,
							m_PixelsPerUnit,
							0,
							SpriteMeshType.FullRect
						);

						sprite.name = string.Format("sprite_{0}",
							i
						);
						sprites[i] = sprite;

						// Sub-image data
						sub_image_data[i] = new STCISubImageData
						{
							m_Index = i,
							m_Offset = new Vector2Int(it.offsetX,
								it.offsetY
							)
						};

						++i;
					}
				}

				// Build the metadata asset that ties everything together
				var data = AssetStci.Create(asset_file_name,
					stci_data.m_Width,
					stci_data.m_Height,
					textures,
					sprites,
					sub_image_data
				);

				// Register everything as sub-assets of this single import
				for(var i = 0; i < sprites.Length; ++i)
				{
					Context.AddObjectToAsset(
						string.Format("texture_{0}",
							i
						),
						textures[i],
						textures[i]
					);
					Context.AddObjectToAsset(
						string.Format("sprite_{0}",
							i
						),
						sprites[i],
						sprites[i].texture
					);
				}

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
