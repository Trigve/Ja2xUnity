using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// Settings for the files imported from JA2.
	/// </summary>
	public sealed class ImportDataSettings : ScriptableObjectSettings<ImportDataSettings>
	{
#region Fields Component
		/// <summary>
		/// All the sections.
		/// </summary>
		[SerializeField]
		private List<ImportDataSection> m_Sections = new();
#endregion

#region Methods Public
		/// <summary>
		/// Find the section with the given name.
		/// </summary>
		/// <param name="SectionName">Section name to search for.</param>
		/// <returns><see cref="ImportDataSection"/> instance if found. Otherwise, null.</returns>
		public ImportDataSection? FindSection(string SectionName)
		{
			return m_Sections.FirstOrDefault(Section => Section.folderName == SectionName);
		}
#endregion

#region Methods Public Static
		/// <summary>
		/// Context menu asset creation.
		/// </summary>
		[UnityEditor.MenuItem("Assets/Create/JA2 Import Daa Settings", false, 1)]
		public static void CreateContext()
		{
			UnityEditor.ProjectWindowUtil.CreateAsset(Create(),
				"Ja2ImportSettings.asset"
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <returns>New instance.</returns>
		private static ImportDataSettings Create()
		{
			var ret = CreateInstance<ImportDataSettings>();

			return ret;
		}
#endregion
	}

	/// <summary>
	/// Data section in import settings.
	/// </summary>
	[Serializable]
	public sealed class ImportDataSection
	{
#region Fields
		/// <summary>
		/// See <see cref="folderName"/>.
		/// </summary>
		[SerializeField]
		private string m_FolderName = string.Empty;

		/// <summary>
		/// Settings for all the files.
		/// </summary>
		[Attributes.PolymorphicSelector]
		[SerializeReference]
		[SerializeField]
		private List<ImportDataFile> m_FileSettngs = new();
#endregion

#region Properties
		/// <summary>
		/// Name of the section/folder.
		/// </summary>
		public string folderName => m_FolderName;
#endregion

#region Methods Public
		/// <summary>
		/// Find the data for the given file name.
		/// </summary>
		/// <param name="FileName">File name to find the data for.</param>
		/// <returns><see cref="ImportDataFile"/> instance if found. Otherwise, null.</returns>
		public ImportDataFile? FindFile(string FileName)
		{
			return m_FileSettngs.FirstOrDefault(File => File.fileName == FileName);
		}
#endregion
	}

	/// <summary>
	/// Base class for per file settings.
	/// </summary>
	[Serializable]
	public abstract class ImportDataFile
	{
#region Fields
		/// <summary>
		/// See <see cref="assetType"/>.
		/// </summary>
		[SerializeField]
		private StciImporter.AssetType m_AssetType;

		/// <summary>
		/// See <see cref="fileName"/>.
		/// </summary>
		[SerializeField]
		private string m_FileName = string.Empty;
#endregion

#region Properties
		/// <summary>
		/// Asset type.
		/// </summary>
		public StciImporter.AssetType assetType => m_AssetType;

		/// <summary>
		/// File name to which data applies.
		/// </summary>
		public string fileName => m_FileName;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="AssetType">Asset type.</param>
		protected ImportDataFile(StciImporter.AssetType AssetType)
		{
			m_AssetType = AssetType;
		}
#endregion
	}
}
