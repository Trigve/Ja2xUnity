using System.Diagnostics;
using System.IO;

using UnityEngine;
using UnityEditor;

using Debug = UnityEngine.Debug;

namespace Ja2.Editor
{
    /// <summary>
    /// Asset extractor
    /// </summary>
    public static class AssetExtractor
    {
#region Methods Static
		/// <summary>
		/// Extract the asset from the given data.
		/// </summary>
		/// <param name="Data"></param>
		/// <param name="BinUtilsDir">Directory for binary utils.</param>
		/// <param name="PathInput"></param>
		/// <param name="PathDirOutput">Output directory path</param>
		public static void Extract(byte[] Data, string BinUtilsDir, string PathInput, string PathDirOutput)
		{
			string project_path = Path.GetDirectoryName(Application.dataPath)!;
		    // Root path
		    string root_path = Directory.GetParent(PathInput)!.Name;
		    // File name
		    string file_name = Path.GetFileName(PathInput).ToLower();
			// File extension
            string file_ext = Path.GetExtension(PathInput).ToLower();

			// Smacker video
			if(file_ext == ".smk")
			{
				// File name for the asset
				string out_file_path = Path.Combine(PathDirOutput,
					root_path,
					Path.GetFileNameWithoutExtension(file_name) + ".mp4"
				);

				// Be sure directory exist
				if(!Directory.Exists(Path.GetDirectoryName(out_file_path)))
					Directory.CreateDirectory(Path.GetDirectoryName(out_file_path)!);

				// Use pipe for the input
				var process = new Process
				{
					StartInfo = new ProcessStartInfo
					{
						FileName = Path.Combine(project_path,
							BinUtilsDir,
							"ffmpeg.exe"
						),
						// Standard profile (supported by unity), correct colorspace and pixel format for unity, move metadata to the beginning
						Arguments = "-loglevel error -f smk -i pipe:0 -c:v libx264 -profile:v baseline -pix_fmt yuv420p -colorspace bt709 -color_primaries bt709 -color_trc bt709 -color_range pc -movflags +faststart -crf 23 " + Path.Combine(project_path, out_file_path),
						// \FIXME Editor doesn't support VP9 codec, even if it is more multi-plaform than h.264
//						Arguments = "-loglevel error -f smk -i pipe:0 -c:v libvpx-vp9 -crf 35 -b:v 0 " + Path.Combine(project_path, out_file_path),
						RedirectStandardInput = true,
						RedirectStandardError = true,
						UseShellExecute = false,
						CreateNoWindow = true
					}
				};

				process.Start();

				// Write the data to the stdin
				{
					try
					{
						using Stream stdin = process.StandardInput.BaseStream;
						stdin.Write(Data);
						stdin.Flush();
					}
					// Read error stream in each case
					finally
					{
						using StreamReader stderr = process.StandardError;
						string output = stderr.ReadToEnd();

						if(output.Length > 0)
							Debug.LogError(output);
					}
				}

				process.WaitForExit();

				AssetDatabase.ImportAsset(out_file_path);
			}
			// All other files extract as is
			else
			{
				// File name for the asset
				string out_file_path = Path.Combine(PathDirOutput,
					root_path,
					Path.GetFileName(file_name)
				);

				// Be sure directory exist
				if(!Directory.Exists(Path.GetDirectoryName(out_file_path)))
					Directory.CreateDirectory(Path.GetDirectoryName(out_file_path)!);

				// Write the data
				{
					using var file_stream = new FileStream(out_file_path, FileMode.Create);
					file_stream.Write(Data);
				}

				AssetDatabase.ImportAsset(out_file_path);
			}
	    }
#endregion
    }
}
