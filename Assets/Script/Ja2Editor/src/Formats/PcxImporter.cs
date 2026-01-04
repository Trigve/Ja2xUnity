using System.IO;

using UnityEditor;
using UnityEditor.AssetImporters;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// PCX importer.
	/// </summary>
	[ScriptedImporter(1, "pcx")]
	public class PcxImporter : ScriptedImporter
	{
#region Methods Public
		/// <inheritdoc />
		public override void OnImportAsset(AssetImportContext Ctx)
		{
			using var filestream = new FileStream(Ctx.assetPath, FileMode.Open);
			using var reader = new BinaryReader(filestream);

			// Wrong header
			if(reader.ReadByte() != 0x0A)
				throw new InvalidDataException("Invalid PCX header");

			// PCX version - not used
			reader.ReadByte();

			// Encoding - not used
			reader.ReadByte();

			var bits_per_pixel = (int)reader.ReadByte();

			// Image size
			ushort x_min = reader.ReadUInt16();
			ushort y_min = reader.ReadUInt16();
			ushort x_max = reader.ReadUInt16();
			ushort y_max = reader.ReadUInt16();

			// Horizontal/vertical DPI resolution - not used
			reader.ReadUInt16();
			reader.ReadUInt16();

			// 16 color palette
			var palette48 = new byte[48];
			int bytes_read = reader.Read(palette48,
				0,
				palette48.Length
			);

			// Bytes read mismatch
			if(bytes_read != palette48.Length)
			{
				throw new InvalidDataException(
					string.Format("Wrong number of bytes read, while reading the palette: {0} != {1}",
						palette48.Length,
						bytes_read
					)
				);
			}

			// Reserved
			reader.ReadByte();

			// Number of the color "planes"
			byte num_planes = reader.ReadByte();
			ushort bytes_per_line = reader.ReadUInt16();

			// Palette type - not used
			reader.ReadUInt16();
			// Horizontal/Vertical source resolution - not used
			reader.ReadUInt16();
			reader.ReadUInt16();

			// Need to read till the end of the header
			var reserved = new byte[54];
			bytes_read = reader.Read(reserved,
				0,
				reserved.Length
			);

			// Bytes read mismatch
			if(bytes_read != reserved.Length)
			{
				throw new InvalidDataException(
					string.Format("Wrong number of reserved bytes read: {0} != {1}",
						reserved.Length,
						bytes_read
					)
				);
			}

			// Read till the end of the file
			byte[] pixel_data = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));

			// Image dimensions
			int width = x_max - x_min + 1;
			int height = y_max - y_min + 1;

			if(width <= 0 || height <= 0)
			{
				throw new InvalidDataException(
					string.Format("Invalid PCX dimensions: {0}x{1}",
						width,
						height
					)
				);
			}

			// Create the texture and the buffer
			var texture = new Texture2D(width,
				height,
				TextureFormat.RGBA32,
				false
			);
			var pixels = new Color32[width * height];

			// 24-bit RGB
			if(bits_per_pixel == 8 && num_planes == 3)
			{
				var scanline_r = new byte[bytes_per_line];
				var scanline_g = new byte[bytes_per_line];
				var scanline_b = new byte[bytes_per_line];
				var i = 0;

				for(var y = 0; y < height; y++)
				{
					DecodeScanline(pixel_data,
						ref i,
						scanline_r,
						bytes_per_line
					);

					DecodeScanline(pixel_data,
						ref i,
						scanline_g,
						bytes_per_line
					);

					DecodeScanline(pixel_data,
						ref i,
						scanline_b,
						bytes_per_line
					);

					// Copy scanline to pixel array (flip Y for Unity)
					for(var x = 0; x < width; ++x)
					{
						int pixel_index = (height - 1 - y) * width + x;
						pixels[pixel_index] = new Color32(scanline_r[x],
							scanline_g[x],
							scanline_b[x],
							255
						);
					}
				}
			}
			// Indexed image
			else if(bits_per_pixel == 8 && num_planes == 1)
			{
				// Palette
				var palette = new Color32[256];

				// Palette is located 768 bytes from the end of data/file
				int palette_start = pixel_data.Length - 768;

				// RGB palette, 12 is magic number identificater
				if(palette_start >= 0 && pixel_data[palette_start - 1] == 12)
				{
					for(var i = 0; i < 256; i++)
					{
						palette[i] = new Color32(
							pixel_data[palette_start + i * 3],
							pixel_data[palette_start + i * 3 + 1],
							pixel_data[palette_start + i * 3 + 2],
							255
						);
					}
				}
				else
				{
					// Use header palette for 16 colors or create grayscale
					for(var i = 0; i < 256; i++)
					{
						var gray = (byte)i;
						palette[i] = new Color32(gray,
							gray,
							gray,
							255
						);
					}
				}

				// Decode RLE data
				var scan_line = new byte[bytes_per_line];
				var data_index = 0;

				for(var y = 0; y < height; ++y)
				{
					DecodeScanline(pixel_data,
						ref data_index,
						scan_line,
						bytes_per_line
					);

					// Copy scanline to pixel array (flip Y for Unity)
					for(var x = 0; x < width; ++x)
					{
						int pixel_index = (height - 1 - y) * width + x;
						pixels[pixel_index] = palette[scan_line[x]];
					}
				}
			}
			else
			{
				throw new InvalidDataException(
					string.Format("Unsupported PCX format: {0} bits per pixel, {1} planes",
						bits_per_pixel,
						num_planes
					)
				);
			}

			texture.SetPixels32(pixels);
			texture.Apply();

			Ctx.AddObjectToAsset(
				Path.GetFileName(Ctx.assetPath),
				texture
			);
			Ctx.SetMainObject(texture);

		}
#endregion

#region Methods Static Private
		/// <summary>
		/// Decode the scan line.
		/// </summary>
		/// <param name="Data">Data to decode</param>
		/// <param name="Index">Index of the data.</param>
		/// <param name="Scanline">Output scanline data.</param>
		/// <param name="BytesPerLine">Number of bytes per line.</param>
		private static void DecodeScanline(byte[] Data, ref int Index, byte[] Scanline, int BytesPerLine)
		{
			var scanline_index = 0;

			// Do while full line is processed
			while(scanline_index < BytesPerLine && Index < Data.Length)
			{
				byte value = Data[Index++];

				// RLE encoded
				if((value & 0xC0) == 0xC0)
				{
					int count = value & 0x3F;

					if(Index >= Data.Length)
						break;

					byte color_value = Data[Index++];

					for(var i = 0; i < count && scanline_index < BytesPerLine; ++i)
						Scanline[scanline_index++] = color_value;
				}
				// Raw value
				else
					Scanline[scanline_index++] = value;
			}
		}
#endregion
	}

	/// <summary>
	/// Post processor for the .pcx files.
	/// </summary>
	public class PcxPostProcess : AssetPostprocessor
	{
#region Methods Public
		/// <summary>
		/// Using this message, because sprite asset need to be created, and it can't be done in
		/// OnPostprocessTexture() at all.
		/// </summary>
		public static void OnPostprocessAllAssets(string[] ImportedAssets, string[] DeletedAssets, string[] MovedAssets, string[] MovedFromAssetPaths)
		{
			var cfg = SettingsDev.instance;
			if(cfg != null)
			{
				// Only interested in imported asset.
				foreach(string it in ImportedAssets)
				{
					// Only from .pcx importer
					if(AssetImporter.GetAtPath(it) is PcxImporter)
					{
						var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(it);

						// Create the sprite from the texture
						var sprite = Sprite.Create(texture,
							new Rect(0,
								0,
								texture.width,
								texture.height
							),
							new Vector2(0.5f, 0.5f),
							100.0f
						);

						AssetDatabase.CreateAsset(sprite,
							Path.ChangeExtension(it,
								Path.GetExtension(it) + ".asset"
							)
						);
					}

				}
			}
		}
#endregion
	}
}
