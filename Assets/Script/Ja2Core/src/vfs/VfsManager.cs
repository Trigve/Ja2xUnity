namespace Ja2.Vfs
{
	/// <summary>
	/// Virtual file system manager.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create VFS Manager",  fileName = "VFSManager")]
	public sealed class VfsManager : ScriptableObjectManager<VfsManager>
	{
#region Methods
		/// <summary>
		/// Open the real file.
		/// </summary>
		/// <param name="LocalFilePath"></param>
		/// <returns></returns>
		internal File OpenFileRegular(in Path LocalFilePath)
		{
			Ja2Logger.LogVfs("Open file: {0}",
				LocalFilePath
			);

			string real_path =
#if UNITY_EDITOR
				SettingsDev.instance.m_UserDir
#else
				Ja2Settings.userDataPath
#endif
				+ "/" + LocalFilePath.value
			;

			return new FileRegular(real_path);
		}
#endregion
	}
}
