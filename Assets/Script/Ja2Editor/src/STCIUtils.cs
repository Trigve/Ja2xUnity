using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Ja2.Editor
{
	/// <summary>
	/// STCI helper class.
	/// </summary>
	internal static class STCIUtils
	{
#region Constants
		/// <summary>
		/// Union size.
		/// </summary>
		private const int UnionSize = 20;

		/// <summary>
		/// Identificator length.
		/// </summary>
		private const int IDLen = 4;

		/// <summary>
		/// Element size in palette.
		/// </summary>
		private const int PaletteElementSize = 3;

		/// <summary>
		/// Sub image size.
		/// </summary>
		private const int SubimageSize = 16;
#endregion

#region Enums
		/// <summary>
		/// STCI flags.
		/// </summary>
		[Flags]
		private enum Flags
		{
			// RGB image
			FlagRGB = 0x0004,
			// ETRLE compression
			EtrleCompressed = 0x0020
		}

		/// <summary>
		/// Image format type.
		/// </summary>
		private enum FormatType
		{
			Rgb = 1,
			Indexed = 2,
		};

		/// <summary>
		/// ETRLE flags.
		/// </summary>
		/// [Flags]
		private enum EtrleFlags
		{
			STCI_TRANSPARENT = 0x80,
			STCI_NON_TRANSPARENT = 0x00,
			STCI_RUN_LIMIT = 0x7F,
		}
#endregion

#region Methods Static
		/// <summary>
		/// Load the STCI from buffer.
		/// </summary>
		/// <param name="Data">Buffer.</param>
		/// <returns></returns>
		/// <exception cref="InvalidDataException"></exception>
		internal static STCIData Load(byte[] Data)
		{
			var ret = new STCIData();

			// Helper reader for source data
			var reader = new BinaryReader(
				new MemoryStream(Data)
			);

			// Parse the file ID
			string id_text = System.Text.Encoding.ASCII.GetString(
				reader.ReadBytes(IDLen)
			);

			// Not an STCI image
			if(id_text != "STCI")
				throw new InvalidDataException("STCI ID not found");

			// Original size
			reader.BaseStream.Seek(4,
				SeekOrigin.Current
			);

			// Stored size
			var stored_size = (int)reader.ReadUInt32();

			// Transparent color value
			reader.BaseStream.Seek(4,
				SeekOrigin.Current
			);

			// Flags
			uint flags = reader.ReadUInt32();

			FormatType format_type = (flags & (int)Flags.FlagRGB) != 0 ? FormatType.Rgb : FormatType.Indexed;
			bool is_etrle = (flags & (int)Flags.EtrleCompressed) != 0;

			// Height and width
			ret.m_Height = reader.ReadUInt16();
			ret.m_Width = reader.ReadUInt16();

			// Read data for union (variant)
			byte[] union_buffer = reader.ReadBytes(UnionSize);

			// BPP
			reader.ReadByte();

			// Move because of alignment
			reader.BaseStream.Seek(3,
				SeekOrigin.Current
			);

			// Read AppDataSize?
			uint app_data_size = reader.ReadUInt32();

			// Move
			reader.BaseStream.Seek(12,
				SeekOrigin.Current
			);

			// Drtermine the data type stored in file
			switch(format_type)
			{
			case FormatType.Rgb:
			{
					var texture = new Texture2D(ret.m_Width,
						ret.m_Height,
						TextureFormat.RGB565,
						false
					);
					texture.SetPixelData(
						reader.ReadBytes(stored_size),
						0
					);

					// Need to invert pixels
					var pixels_orig = texture.GetPixels32();
					var pixels_flipped = new Color32[pixels_orig.Length];

					// Invert the pixels vertically
					for(var x = 0; x < texture.width; x++)
					{
						for(var y = 0; y < texture.height; y++)
							pixels_flipped[x + y * texture.width] = pixels_orig[x + (texture.height - y - 1) * texture.width];
					}

					texture.SetPixels32(pixels_flipped);

					texture.Apply();

					ret.m_SubImageData.Add(
						new STCIData.SubImage()
						{
							height = ret.m_Height,
							width = ret.m_Width,
							offsetX = 0,
							offsetY = 0,
							texture = texture
						}
					);
				}
				break;
			case FormatType.Indexed:
				{
					// Union data reader
					using var union_reader = new BinaryReader(
						new MemoryStream(union_buffer)
					);

					// Number of colors
					var colors_num = (int)union_reader.ReadUInt32();

					Assert.AreEqual(colors_num, 256, "Number of palletes mismatch");

					// Number of subimages
					int sub_img_count = union_reader.ReadUInt16();

					// Red depth, green depth, Blue depth
					union_reader.BaseStream.Seek(3,
						SeekOrigin.Current
					);

					// Allocate for paletes
					int file_section_size = colors_num * PaletteElementSize;

					// Read pallete
					byte[] pallete = reader.ReadBytes(file_section_size);

					int sub_image_size = sub_img_count * SubimageSize;

					// Read sub image data
					byte[] etrle_buffer = reader.ReadBytes(sub_image_size);

					// Read image data
					byte[] image_data = reader.ReadBytes(stored_size);

					// Load Application data
					if(app_data_size > 0)
					{
					}

					// Sub-image data reader
					using var reader_sub_image = new BinaryReader(
						new MemoryStream(etrle_buffer)
					);

					// Create pixel buffer for each subimage and add it to sprite description
					for(var i = 0; i < sub_img_count; i++)
					{
						uint data_offset = reader_sub_image.ReadUInt32();
						uint data_length = reader_sub_image.ReadUInt32();

						short offset_x = reader_sub_image.ReadInt16();
						short offset_y = reader_sub_image.ReadInt16();

						int sub_img_height = reader_sub_image.ReadUInt16();
						int sub_img_width = reader_sub_image.ReadUInt16();

						// Buffer of new pixels
						var p_new_px_buffer = new Color32[sub_img_width * sub_img_height];
						// Buffer for alternative texture
						var p_new_px_buffer_alt = new Color32[sub_img_width * sub_img_height];

						// Need to uncompress data
						if(is_etrle)
						{
							var etrle_data = image_data.AsSpan((int)data_offset,
								(int)data_length
							);

							// Current offset in ETRLE buffer
							var etrle_offset = 0;

							for(var j = 0; j < data_length;)
							{
								ushort no_pixels;

								// Transparent pixels
								if((etrle_data[j] & (byte)EtrleFlags.STCI_TRANSPARENT) != 0)
								{
									// Number of pixel
									no_pixels = (ushort)(etrle_data[j++] & (byte)EtrleFlags.STCI_RUN_LIMIT);

									// Set pixels and move forward
									p_new_px_buffer.AsSpan(etrle_offset,
										no_pixels
									).Fill(
										new Color32(255,
											255,
											255,
											0
										)
									);

									p_new_px_buffer_alt.AsSpan(etrle_offset,
										no_pixels
									).Fill(
										new Color32(255,
											255,
											255,
											0
										)
									);

									etrle_offset += no_pixels;

									if(etrle_data[j] == 0)
										j++;
								}
								// Non-transparent pixels
								else
								{
									// Number of pixels to copy
									no_pixels = etrle_data[j++];
									// Set pixel data
									for(var k = 0; k < no_pixels; ++k, ++etrle_offset)
									{
										int palette_index = etrle_data[j++];

										byte[] pallete_entries = pallete[(palette_index * 3)..(palette_index * 3 + 3)];

										// If we don't want to have shadow font
										if(palette_index == 1)
										{
											p_new_px_buffer[etrle_offset] = new Color32(pallete_entries[0],
												pallete_entries[1],
												pallete_entries[2],
												255
											);

											// For alternative texture
											p_new_px_buffer_alt[etrle_offset] = new Color32(255,
												255,
												255,
												0
											);
										}
										// We want font with shadow or loading texture
										else
										{
											// Set color from pallete
											p_new_px_buffer[etrle_offset] = p_new_px_buffer_alt[etrle_offset] = new Color32(pallete_entries[0],
												pallete_entries[1],
												pallete_entries[2],
												255
											);
										}
									}
								}
							}
						}

						// Need to mirror the texture upside down and left to right
						Array.Reverse(p_new_px_buffer,
							0,
							p_new_px_buffer.Length
						);
						for(var j = 0; j < sub_img_width / 2; ++j)
						{
							for(var k = 0; k < sub_img_height; ++k)
							{
								// Swap pixels
								(p_new_px_buffer[sub_img_width * k + j], p_new_px_buffer[sub_img_width * k + sub_img_width - 1 - j]) = (p_new_px_buffer[sub_img_width * k + sub_img_width - 1 - j], p_new_px_buffer[sub_img_width * k + j]);
							}
						}

						Array.Reverse(p_new_px_buffer_alt,
							0,
							p_new_px_buffer_alt.Length
						);
                        for(var j = 0; j < sub_img_width / 2; ++j)
                        {
                        	for(var k = 0; k < sub_img_height; ++k)
                        	{
                        		// Swap pixels
                        		(p_new_px_buffer_alt[sub_img_width * k + j], p_new_px_buffer_alt[sub_img_width * k + sub_img_width - 1 - j]) = (p_new_px_buffer_alt[sub_img_width * k + sub_img_width - 1 - j], p_new_px_buffer_alt[sub_img_width * k + j]);
                        	}
                        }

                        // Create the textures
						var texture = new Texture2D(sub_img_width,
							sub_img_height,
							TextureFormat.RGBA32,
							false
						);
						texture.filterMode = FilterMode.Point;
						texture.SetPixels32(p_new_px_buffer);

						var texture_alt = new Texture2D(sub_img_width,
							sub_img_height,
							TextureFormat.RGBA32,
							false
						);
						texture_alt.SetPixels32(p_new_px_buffer_alt);

						// Add all the data to the subimage data
						ret.m_SubImageData.Add(
							new STCIData.SubImage()
							{
								height = sub_img_height,
								width = sub_img_width,
								offsetX = offset_x,
								offsetY = offset_y,
								texture = texture,
								textureAlt = texture_alt
							}
						);
					}
				}
				break;
			}

			// Return sprite descriptor
			return ret;
		}
#endregion
	}
}
