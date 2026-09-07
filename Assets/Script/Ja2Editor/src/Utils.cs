namespace Ja2.Editor
{
	/// <summary>
	/// Generic utils.
	/// </summary>
	internal static class Utils
	{
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
				prefix += "_!";
				break;
			case PathAttribute.Normal:
			default:
				// Do nothing
				break;
			}

			return prefix + Name;
		}
#endregion
	}
}
