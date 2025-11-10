using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
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
		[MenuItem("Ja2/Extract SLF")]
		private static void MenuSlfExtract()
		{
			// Read all the SLF
			foreach(string it in Directory.EnumerateFiles(SettingsDev.instance.m_InputDir, "*.slf"))
			{
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
		}
#endregion
	}
}
