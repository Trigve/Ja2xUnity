using System;
using System.IO;
using System.Text;

namespace Ja2.Vfs
{
	/// <summary>
	/// Line reader.
	/// </summary>
	internal sealed class LineReader
	{
#region Constants
		/// <summary>
		/// Buffer size.
		/// </summary>
		private const int BufferSize = 1024;

		/// <summary>
		/// UTF-8 BOM.
		/// </summary>
		private static readonly byte[] UTF8Bom = { 0xef, 0xbb, 0xbf };
#endregion

#region Fields
		/// <summary>
		/// File used.
		/// </summary>
		private readonly IFileReadable m_File;

		/// <summary>
		/// Buffer position.
		/// </summary>
		private long m_BufferPos;

		/// <summary>
		/// Bytes left for reading.
		/// </summary>
		private long m_BytesLeft;

		/// <summary>
		/// Internal buffer real size.
		/// </summary>
		private long m_BufferSize;

		/// <summary>
		/// Internal buffer.
		/// </summary>
		private readonly byte[] m_Buffer = new byte[BufferSize + 1];

		/// <summary>
		/// If false the object user checks for EOF and closes file
		/// </summary>
		private readonly bool m_AutoCtrlFile;

		/// <summary>
		/// Is data in UTF-8 encoding.
		/// </summary>
		private readonly bool m_IsUtf8;
#endregion

#region Methods
		/// <summary>
		/// Read the line from the file.
		/// </summary>
		/// <param name="Line">Output string.</param>
		/// <returns>True if line was read. Otherwise, false.</returns>
		public bool ReadLine(out string Line)
		{
			bool ret = FromBuffer(out Line);

			// If file handler is controlled by caller, we have to read to EOL or EOF. So if happened to read over EOL (due to buffering),
			// then we need to move caret back to a position where just fetched line ends (EOL).
			if(!m_AutoCtrlFile && m_BufferPos < m_BufferSize)
			{
				// "lesser - greater" intentionally to get negative offset
				long offset = m_BufferPos - m_BufferSize;
				m_File.SetReadPosition(offset,
					SeekOrigin.Current
				);
			}

			return ret;
		}

		/// <summary>
		/// Fill the buffer with data from file.
		/// </summary>
		/// <param name="Refill"></param>
		/// <returns></returns>
		private bool FillBuffer(bool Refill = false)
		{
			if(m_BytesLeft == 0 || (!m_AutoCtrlFile && Refill))
				return false;

			long bytes_read = m_BytesLeft < BufferSize ? m_BytesLeft : BufferSize;

			// Fill the buffer from the start, BufferSize bytes at max
			bytes_read = m_File.Read(
				m_Buffer.AsSpan(0, (int)bytes_read)
			);


			m_BytesLeft -= bytes_read;

			// Always terminate the string with 0
			m_Buffer[bytes_read] = 0;

			m_BufferPos = 0;
			m_BufferSize = bytes_read;

			return true;
		}

		/// <summary>
		/// Read the data from buffer.
		/// </summary>
		/// <param name="Line"></param>
		/// <returns></returns>
		private bool FromBuffer(out string Line)
		{
			// Default value
			Line = string.Empty;
			var str_builder = new StringBuilder();

			for(var done = false; !done;)
			{
				if(m_BufferPos < m_BufferSize)
				{
					// Start where left last time
					byte value = m_Buffer[m_BufferPos];

					long start_pos = m_BufferPos;

					// Unity end of buffer or delimiter is found
					foreach(byte it in m_Buffer.AsSpan((int)m_BufferPos, (int)(m_BufferSize - m_BufferPos)))
					{
						value = it;

						if(m_BufferPos >= m_BufferSize || it == 0 || it == Constants.CarriageReturn || it == Constants.LineFeed)
							break;

						++m_BufferPos;
					}

					var data_append = m_Buffer.AsSpan((int)start_pos, (int)(m_BufferPos - start_pos));

					// Need to append substring, as the buffer might need refill (because there was no \n or \r\n terminator)
					str_builder.Append(m_IsUtf8 ? Encoding.UTF8.GetString(data_append) : Encoding.Default.GetString(data_append));

					// If (real) end of the buffer (that always terminate with 0) is reach, this means
					// that there was no line terminator and that we have to refill the buffer.
					if(m_BufferPos < BufferSize && (value == Constants.LineFeed || value == Constants.CarriageReturn || value == 0))
					{
						// Found the line terminator
						if(value == '\r')
						{
							// the \r is most probably followed by \n. 'swallow' both characters
							++m_BufferPos;

							if((m_BufferPos < BufferSize) && (m_Buffer[m_BufferPos] == '\n' || m_Buffer[m_BufferPos] == 0))
							{
								// Increase buffer position, so that we can start with a valid character in the next run
								++m_BufferPos;
								Line = str_builder.ToString();

								return true;
							}

							done = !FillBuffer(true);
						}
						else if(value == '\n' || value == 0)
						{
							// increase buffer position, so that we can start with a valid character in the next run
							++m_BufferPos;
							Line = str_builder.ToString();

							return true;
						}
					}
					else
						done = !FillBuffer(true);
				}
				else
					done = !FillBuffer(true);
			}

			return false;
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="File">Input file.</param>
		/// <param name="AutoControlFile"></param>
		public LineReader(IFileReadable File, bool AutoControlFile = true)
		{
			m_File = File;
			m_BufferPos = 0;
			m_AutoCtrlFile = AutoControlFile;

			// Current position in file
			long start_read_position = File.readPosition;
			m_BytesLeft = (int)(start_read_position >= File.file.size ? 0 : File.file.size - start_read_position);

			// Initial buffer read
			FillBuffer();

			// Beginning of file
			if(start_read_position == 0)
			{
				// If there is a BOM, skip it
				if(m_Buffer.AsSpan(0, 3).SequenceEqual(UTF8Bom.AsSpan()))
				{
					m_IsUtf8 = true;
					m_BufferPos += 3;
				}
			}
		}
#endregion
	}
}
