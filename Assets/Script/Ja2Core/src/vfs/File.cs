namespace Ja2.Vfs
{
	/// <summary>
	/// Base class for all files.
	/// </summary>
	internal abstract class File
	{
#region Properties
		/// <summary>
		/// File path.
		/// </summary>
		public Path filePath { get; private set; }

		/// <summary>
		/// Attributes for file.
		/// </summary>
		public abstract FileAttributes attributes { get; }

		/// <summary>
		/// File size.
		/// </summary>
		public abstract long size { get;}
#endregion

#region Methods
		/// <summary>
		/// Close the file.
		/// </summary>
		public abstract void Close();

		/// <summary>
		/// Convert the instance to readable file.
		/// </summary>
		public abstract bool AsReadable(out IFileReadable Readable);

		/// <summary>
		/// Convert instance to writeable file.
		/// </summary>
		public abstract bool AsWriteable(out IFileWriteable Writeable);
#endregion

#region Constructors
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="FilePath">File path.</param>
		protected File(in Path FilePath)
		{
			filePath = FilePath;
		}
#endregion
	}
}
