using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Assertions;

namespace Ja2.Editor
{
	/// <summary>
	/// SLF archive manager.
	/// </summary>
	internal static class SlfManager
	{
#region Constants
		/// <summary>
		/// Size of file name.
		/// </summary>
		private const int FilenameSize = 256;
		/// <summary>
		/// Size of library header.
		/// </summary>
		private const int LibHeaderSize = 532;
		/// <summary>
		/// Size of dir entry.
		/// </summary>
		private const int DirEntrySize = 280;
#endregion

#region Methods Static
		/// <summary>
		/// Extract SLF data.
		/// </summary>
		/// <param name="PathIn">Input file path.</param>
		internal static IEnumerable<FileData> ExtractPath(string PathIn)
		{
			Assert.IsTrue(File.Exists(PathIn), "Input path file not found");

			// SLF reader
			using var slf_reader = new BinaryReader(
				new FileStream(PathIn,
					FileMode.Open,
					FileAccess.Read
				)
			);

			// Get the file name of the SLF
			string input_file_name = Path.GetFileNameWithoutExtension(PathIn).ToLower();

			// skip library name
			slf_reader.BaseStream.Seek(FilenameSize,
				SeekOrigin.Current
			);

			// Get library path
			string lib_path = ReadString(
				slf_reader.ReadBytes(FilenameSize)

			);

			// Skip the delimiter
			if(lib_path[^1] == '\\')
			{
				lib_path = lib_path[..^1];
			}

			// Number of entries in library
			int entries_len = slf_reader.ReadInt32();

			// Used, Sort, Version, Contain sub directories
			slf_reader.BaseStream.Seek(4 + 2 + 2 + 4,
				SeekOrigin.Current
			);

			// Loop through all the entries
			for(var i = 0; i < entries_len; i++)
			{
				// Seek to end of file where list of dir entries is located
				slf_reader.BaseStream.Seek(-(entries_len * DirEntrySize) + i * DirEntrySize,
					SeekOrigin.End
				);

				// Read file name, use native separator
				string file_name = string.Join('/',
					ReadString(
						slf_reader.ReadBytes(FilenameSize)
					).Split('\\')
				);

				// Read offset
				int data_offset = slf_reader.ReadInt32();
				// Read size
				int data_size = slf_reader.ReadInt32();

				byte file_ok = slf_reader.ReadByte();

				// File ok
				if(file_ok == 0)
				{
					// Read the data
					slf_reader.BaseStream.Seek(data_offset,
						SeekOrigin.Begin
					);

					byte[] buffer = slf_reader.ReadBytes(data_size);

					yield return new FileData(
						string.Join('/',
							input_file_name,
							file_name
						),
						buffer
					);
				}
			}
		}

		/// <summary>
		/// Read the string from the buffer.
		/// </summary>
		/// <param name="Buffer">Source buffer</param>
		private static string ReadString(IEnumerable<byte> Buffer)
		{
			return System.Text.Encoding.ASCII.GetString(
				Buffer.TakeWhile(Value => Value != 0).ToArray()
			);
		}
#endregion
	}
}
