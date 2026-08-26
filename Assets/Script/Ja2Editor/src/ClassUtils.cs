using System;
using System.Reflection;

namespace Ja2.Editor
{
	/// <summary>
	/// Helper class for class operations.
	/// </summary>
	internal static class ClassUtils
	{
#region Methods Static Public
		/// <summary>
		/// Set the property with reflection.
		/// </summary>
		/// <param name="Target">Target class.</param>
		/// <param name="PropertyName">Property name.</param>
		/// <param name="Value">Value to be set.</param>
		/// <exception cref="MissingMemberException">Thrown, if the property with the given name wasn't found.</exception>
		public static void PropertySet(object Target, string PropertyName, object Value)
		{
			Type type = Target.GetType();

			// Try to find the property
			PropertyInfo? prop = type.GetProperty(PropertyName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
			);
			if(prop is not null && prop.CanWrite)
			{
				prop.SetValue(Target,
					Value
				);

				return;
			}

			throw new MissingMemberException(
				string.Format("Could not find a settable property for '{0}' on {1}.",
					PropertyName,
					type.Name
				)
			);
		}

		/// <summary>
		/// Set the field with reflection.
		/// </summary>
		/// <param name="Target">Target class.</param>
		/// <param name="FieldName">Field name</param>
		/// <param name="Value">Value to be set.</param>
		/// <exception cref="MissingMemberException">Thrown, if the field with the given name wasn't found.</exception>
		public static void FieldSet(object Target, string FieldName, object Value)
		{
			Type type = Target.GetType();

			// Try to find the field
			FieldInfo? field = type.GetField(FieldName,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
			);
			if(field is not null)
			{
				field.SetValue(Target,
					Value
				);

				return;
			}

			throw new MissingMemberException(
				string.Format("Could not find a field for '{0}' on {1}.",
					FieldName,
					type.Name
				)
			);
		}
#endregion
	}
}
