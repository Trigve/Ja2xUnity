using System;
using System.IO;

namespace Ja2.Vfs
{
	/// <summary>
	/// Regular file.
	/// </summary>
	internal sealed class FileRegular : File
	{
#region Methods
		/// <inheritdoc />
		public override FileAttributes attributes => new FileAttributes(FileAttributes.Attribute.AttribNormal, FileAttributes.LocationType.LocDir);

		/// <inheritdoc />
		public override long size => new FileInfo(filePath.ToString()).Length;

		/// <inheritdoc />
		public override void Close()
		{
			// Do nothing
		}

		/// <inheritdoc />
		public override bool AsReadable(out IFileReadable Readable)
		{
			Readable = new FileRegularStream(this);

			return true;
		}

		/// <inheritdoc />
		public override bool AsWriteable(out IFileWriteable Writeable)
		{
			Writeable = new FileRegularStream(this);

			return true;
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Path"></param>
		public FileRegular(string Path)
		: base(new Path(Path))
		{
		}
#endregion
	}

	/// <summary>
	/// Implementation of interfaces.
	/// </summary>
	internal sealed class FileRegularStream : IFileReadable, IFileWriteable
	{
#region Fields
		/// <summary>
		/// File instance.
		/// </summary>
		private FileRegular m_File;

		/// <summary>
		/// File stream used.
		/// </summary>
		private readonly FileStream m_FileStream;
#endregion

#region Properties
		/// <inheritdoc />
		public bool isOpenRead => m_FileStream.CanRead;

		/// <inheritdoc />
		public long readPosition => m_FileStream.Position;

		/// <inheritdoc />
		public File file => m_File;
#endregion

#region Methods
		/// <inheritdoc />
		public void Dispose()
		{
			m_FileStream.Dispose();
		}

		/// <inheritdoc />
		public long Read(Span<byte> Buffer)
		{
			return m_FileStream.Read(Buffer);
		}

		/// <inheritdoc />
		public void SetReadPosition(long OffsetInBytes, SeekOrigin SeekDir)
		{
			m_FileStream.Seek(OffsetInBytes,
				SeekDir
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="File">Source file.</param>
		public FileRegularStream(FileRegular File)
		{
			m_File = File;

			m_FileStream = new FileStream(m_File.filePath.ToString(),
				FileMode.Open,
				FileAccess.Read,
				FileShare.Read
			);
		}
#endregion
	}
}
