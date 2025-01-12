using System;
using System.IO;

namespace Ja2.Vfs
{
	/// <summary>
	/// Interface for readable file.
	/// </summary>
	internal interface IFileReadable : IDisposable
	{
#region Properties
		/// <summary>
		/// Is opened for read.
		/// </summary>
		public bool isOpenRead { get; }

		/// <summary>
		/// Current read position offset.
		/// </summary>
		public long readPosition { get; }

		/// <summary>
		/// File that is owner of this instance.
		/// </summary>
		public File file { get; }
#endregion

#region Methods
		/// <summary>
		/// Read the data.
		/// </summary>
		/// <param name="Buffer">Destination buffer</param>
		/// <returns>Data read.</returns>
		public long Read(Span<byte> Buffer);

		/// <summary>
		/// Set the read position offset.
		/// </summary>
		/// <param name="OffsetInBytes"></param>
		/// <param name="SeekDir"></param>
		public void SetReadPosition(long OffsetInBytes, SeekOrigin SeekDir);
#endregion
	}
}
