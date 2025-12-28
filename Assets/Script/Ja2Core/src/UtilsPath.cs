using System.Runtime.CompilerServices;

namespace Ja2
{
	/// <summary>
	/// Utils for path operations.
	/// </summary>
	public static class UtilsPath
	{
#region Methods Static Public
		/// <summary>
		/// Normalize the given path, to use lowercase and forward slash ("/") as separator.
		/// </summary>
		/// <param name="Path">Path.</param>
		/// <returns>Normalized path.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string NormalizePath(string Path)
		{
			return Path.ToLower().Replace('\\', '/');
		}
#endregion
	}
}
