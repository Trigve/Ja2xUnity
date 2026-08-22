using System.IO;

using UnityEditor.AssetImporters;

using UnityEngine;

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
#endregion

#region Methods Public
		/// <inheritdoc/>
		public override void OnImportAsset(AssetImportContext Context)
		{
			// Parse the STCI as first
			STCIData stci_data = STCIUtils.Load(
				File.ReadAllBytes(Context.assetPath)
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
			var data = AssetStci.Create(Path.GetFileNameWithoutExtension(Context.assetPath),
				stci_data.m_Width,
				stci_data.m_Height,
				textures,
				sprites,
				sub_image_data
			);

			// Register everything as sub-assets of this single import
			for(var i = 0; i < sprites.Length; ++i)
			{
				Context.AddObjectToAsset($"texture_{i}",
					textures[i],
					textures[i]
				);
				Context.AddObjectToAsset($"sprite_{i}",
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
#endregion
}
