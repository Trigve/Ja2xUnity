using System.IO;

using UnityEngine;
using UnityEditor;

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
	    /// <param name="FilePath"></param>
	    public static void Extract(byte[] Data, string FilePath)
	    {
		    // Root path
		    string root_path = Directory.GetParent(FilePath)!.Name;
		    // File name
		    string file_name = Path.GetFileName(FilePath).ToLower();
			// File extension
            string file_ext = Path.GetExtension(FilePath).ToLower();

            // Process file by extension
            if(file_ext == ".sti")
            {
            	STCIData stci_data = STCIUtils.Load(Data);

            	// File name for the asset
            	string stci_file_path = Path.Combine(SettingsDev.instance.m_DataDir,
		            FilePath,
		            file_name
            	);

            	// Need to create any directories, that doesn't exists
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
            		sprite_so.m_NameOrig = Path.GetFileName(FilePath).ToLower();
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
            				new Vector2(sub_image_data.offsetX,
            					sub_image_data.offsetY
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
            else
            	Debug.LogWarning(string.Format("Unsupported file type '{0}'", file_ext));
	    }
#endregion
    }
}
