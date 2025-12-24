using System;

namespace Ja2
{
	/// <summary>
	/// Attribute used for fields, when need to store the historic name of the field.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class HistoricNameAttribute : Attribute
	{
#region Properties
		/// <summary>
		/// Historic name.
		/// </summary>
		public string name { get; }
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Name">Name to store.</param>
		public HistoricNameAttribute(string Name)
		{
			name = Name;
		}
#endregion
	}
}
