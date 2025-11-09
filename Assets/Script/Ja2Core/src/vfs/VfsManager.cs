namespace Ja2.Vfs
{
	/// <summary>
	/// Virtual file system manager.
	/// </summary>
	internal class VfsManager : ScopedSingleton<VfsManager>
	{
#region Methods
		/// <summary>
		/// Open the real file.
		/// </summary>
		/// <param name="LocalFilePath"></param>
		/// <returns></returns>
		public File OpenFileRegular(in Path LocalFilePath)
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
