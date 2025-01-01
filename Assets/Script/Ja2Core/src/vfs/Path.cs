using System;
using System.Text.RegularExpressions;

namespace Ja2.Vfs
{
	/// <summary>
	/// Path representation.
	/// </summary>
	internal readonly struct Path : IEquatable<Path>
	{
#region Constants
		/// <summary>
		/// Path separator string.
		/// </summary>
		private const string SeparatorStr = "\\";
#endregion

#region Nested classes
		/// <summary>
		/// Separator positions.
		/// </summary>
		private readonly struct SeparatorPosition
		{
			public readonly int m_First;
			public readonly int m_Last;

			public static SeparatorPosition Empty()
			{
				return new SeparatorPosition(-1);
			}

			public SeparatorPosition(int First = -1, int Last = -1)
			{
				m_First = First;
				m_Last = Last;
			}

		}
#endregion

#region Fields Static
		/// <summary>
		/// Regex matching any separator.
		/// </summary>
		private static readonly Regex m_RegexSeparator = new Regex(@"[/\\]", RegexOptions.Compiled);

		/// <summary>
		/// Regex matching "./".
		/// </summary>
		private static readonly Regex m_RegexCurDir = new Regex(@"\.[/\\]", RegexOptions.Compiled);
#endregion

#region Fields
		/// <summary>
		/// Asset bundle to use.
		/// </summary>
		private readonly string m_Bundle;

		/// <summary>
		/// Path string.
		/// </summary>
		private readonly string m_Path;

		private readonly SeparatorPosition m_Sep;
#endregion

#region Properties
		/// <summary>
		/// Is path empty.
		/// </summary>
		public bool isEmpty => m_Path.Length == 0;

		/// <summary>
		/// Path length.
		/// </summary>
		public int length => m_Path.Length;
#endregion

#region Methods
		/// <summary>
		/// Convert to string.
		/// </summary>
		/// <returns></returns>
		public new string ToString()
		{
			return m_Path;
		}

		/// <summary>
		/// Generate hash code.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(m_Path);
		}

		/// <summary>
		/// Comparer.
		/// </summary>
		/// <param name="Other"></param>
		/// <returns></returns>
		public bool Equals(Path Other)
		{
			return string.Equals(m_Path,
				Other.m_Path,
				StringComparison.InvariantCultureIgnoreCase
			);
		}

		/// <summary>
		/// Comparer.
		/// </summary>
		/// <param name="Obj"></param>
		/// <returns></returns>
		public override bool Equals(object? Obj)
		{
			return Obj is Path other && Equals(other);
		}

		/// <summary>
		/// Comparer.
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator ==(Path Left, Path Right)
		{
			return Left.Equals(Right);
		}

		/// <summary>
		/// Comparer.
		/// </summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns></returns>
		public static bool operator !=(Path Left, Path Right)
		{
			return !Left.Equals(Right);
		}
#endregion

#region Methods Static
		/// <summary>
		/// Split string based on <paramref name="SepLast"/>.
		/// </summary>
		/// <param name="Path">Input path.</param>
		/// <param name="SepLast"></param>
		/// <param name="Head">Part before the separator.</param>
		/// <param name="Last">Part after the separator.</param>
		private static void PathSplitLast(string Path, int SepLast, out string Head, out string Last)
		{
			// Default values for empty path
			Head = string.Empty;
			Last = Path;

			if(Path.Length == 0)
				return;

			// Valid separator
			if(SepLast != -1)
			{
				Head = Path[..SepLast];

				Last = Path.Substring(SepLast + 1,
					Path.Length - SepLast - 1
				);
			}
		}

#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Path"></param>
		public Path(string Path)
		: this(string.Empty, Path)
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Bundle"></param>
		/// <param name="Path"></param>
		public Path(string Bundle, string Path)
		{
			m_Bundle = Bundle;
			m_Path = Path;
			m_Sep = SeparatorPosition.Empty();

			if(m_Path.Length != 0)
			{
				// Replace separators
				m_Path = m_RegexSeparator.Replace(m_Path,
					SeparatorStr
				);

				// Replace "./"
				m_Path = m_RegexCurDir.Replace(m_Path,
					string.Empty
				);

			}
		}
#endregion
	}
}
