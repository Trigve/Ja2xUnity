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

            // Process file by extension
            if(file_ext == ".sti")
            {
            	STCIData stci_data = STCIUtils.Load(Data);

            	// File name for the asset
            	string stci_file_path = Path.Combine(PathDirOutput,
		            PathInput,
		            file_name
            	);

            	// Need to create any directories, that doesn't exist
            	Directory.CreateDirectory(
            		Directory.GetParent(stci_file_path)!.FullName
            	);

            	// Font
            	if(root_path == "font")
            	{
            	}
            	// Textures, sprites
            	else
            	{
            		var j = 0;

            		// Create the texture description
            		var sprite_so = AssetTexture.Create();
            		sprite_so.m_NameOrig = Path.GetFileName(PathInput).ToLower();
            		sprite_so.m_Width = stci_data.m_Width;
            		sprite_so.m_Height = stci_data.m_Height;

            		// Process all the images
            		foreach(STCIData.SubImage sub_image_data in stci_data.m_SubImageData)
            		{
            			// Create the sprite
            			var sprite_stci = Sprite.Create(sub_image_data.texture,
            				new Rect(0,
            					0,
            					sub_image_data.width,
            					sub_image_data.height
            				),
            				new Vector2(0.5f + ((float)sub_image_data.offsetX / sub_image_data.width),
								0.5f + ((float)sub_image_data.offsetY / sub_image_data.height)
            				)
            			);

            			// Save the texture as first
            			AssetDatabase.CreateAsset(sprite_stci.texture,
            				stci_file_path + ".tex." + j + ".asset"
            			);

            			// Save the sprite
            			AssetDatabase.CreateAsset(sprite_stci,
            				stci_file_path + "." + j + ".asset"
            			);

            			sprite_so.m_Sprites.Add(sprite_stci);

            			++j;
            		}

            		AssetDatabase.CreateAsset(sprite_so,
            			stci_file_path + ".data.asset"
            		);
            	}
            }
			// Smacker video
			else if(file_ext == ".smk")
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
            else
            	Debug.LogWarning(string.Format("Unsupported file type '{0}'", file_ext));
	    }
#endregion
    }
}
