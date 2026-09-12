using System;

namespace Ja2.Attributes
{
	/// <summary>
	/// Attribute used for the AssetRef types. Only valid for property drawer.
	/// </summary>
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class AssetRefTypeAttribute : Attribute
	{
#region properties
		/// <summary>
		/// Type allowed.
		/// </summary>
		public Type type { get; }
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Type">See <see cref="type"/>.</param>
		public AssetRefTypeAttribute(Type Type)
		{
			type = Type;
		}
#endregion
	}
}
