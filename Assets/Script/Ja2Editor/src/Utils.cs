namespace Ja2.Editor
{
	/// <summary>
	/// Generic utils.
	/// </summary>
	internal static class Utils
	{
#region Constants
		/// <summary>
		/// String used for the <see cref="PathAttribute.Ignore"/> attribute.
		/// </summary>
		private const string IgnoreStr = "_!";
#endregion

#region Enums
		/// <summary>
		/// Path attribute.
		/// </summary>
		internal enum PathAttribute
		{
			Normal,
			Ignore,
		}
#endregion

#region Methods Public Static
		/// <summary>
		/// Generate the path with the given path attribute.
		/// </summary>
		/// <param name="Name">Source name/path.</param>
		/// <param name="Attribute">Path attribue.</param>
		/// <returns>Generated path</returns>
		public static string GeneratePath(string Name, PathAttribute Attribute = PathAttribute.Normal)
		{
			var prefix = string.Empty;

			switch(Attribute)
			{
			case PathAttribute.Ignore:
				prefix += IgnoreStr;
				break;
			case PathAttribute.Normal:
			default:
				// Do nothing
				break;
			}

			return prefix + Name;
		}

		/// <summary>
		/// Find out if the given path attribue is set for the given path.
		/// </summary>
		/// <param name="Path">Path to check for.</param>
		/// <param name="Attribute">Path attribute to check.</param>
		/// <returns>True, if the path contains the attribute. Otherwise, false.</returns>
		public static bool IsPathAttributeSet(string Path, PathAttribute Attribute)
		{
			string str_to_test;

			switch(Attribute)
			{
			case PathAttribute.Ignore:
				str_to_test = IgnoreStr;
				break;
			case PathAttribute.Normal:
			default:
				return true;
			}

			// Only if provided
			if(!string.IsNullOrEmpty(str_to_test))
				return Path[0..str_to_test.Length] == str_to_test;


			return false;
		}
#endregion
	}
}
