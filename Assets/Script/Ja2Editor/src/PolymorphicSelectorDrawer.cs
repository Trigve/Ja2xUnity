using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace Ja2.Editor
{
	/// <summary>
	/// Custom drawer for the <see cref="Attributes.PolymorphicSelectorAttribute"/>.
	///
	/// Code modified from https://discussions.unity.com/t/abstract-skilllist-that-can-be-edited-in-inspector/948612/14 .
	/// </summary>
	[CustomPropertyDrawer(typeof(Attributes.PolymorphicSelectorAttribute))]
	public sealed class PolymorphicSelectorDrawer : PropertyDrawer
	{
#region Methods Public
		/// <inheritdoc/>
		public override VisualElement CreatePropertyGUI(SerializedProperty Property)
		{
			// Root
			var visual_element = new VisualElement();

			var property_field = new PropertyField();
			property_field.BindProperty(Property);
			property_field.label = " ";

			// Reference type
			if(Property.propertyType == SerializedPropertyType.ManagedReference)
			{
				// Add popup with supported types
				visual_element.Add(
					new TypePopupField(Property,
						GetTypes(fieldInfo,
							Property
						)
					)
				);

			}
			visual_element.Add(property_field);

			return visual_element;
		}
#endregion

#region Methods Private Static
		/// <summary>
		/// Get all the valid types for the given managed reference property.
		/// </summary>
		/// <param name="FieldInfo">Field info.</param>
		/// <param name="Property">Managed reference property.</param>
		/// <returns>The list of the types supported for the given property.</returns>
		private static List<Type?> GetTypes(FieldInfo FieldInfo, SerializedProperty Property)
		{
			// Get the type of the managed reference
			Type? obj_type = Property.managedReferenceValue?.GetType();

			Type field_type = FieldInfo.FieldType;

			// All the valid types for the given object
			var types = new List<Type?>()
			{
				obj_type,
			};

			// Is it array type or List<>
			bool is_collection = field_type.IsArray || field_type.IsGenericType && field_type.GetGenericTypeDefinition() == typeof(List<>);

			Type base_type = field_type;
			// Collection
			if(is_collection)
			{
				base_type = field_type.GetGenericArguments()[0];

				// For collection only add non-abstract types
				if(!base_type.IsAbstract)
					types.Add(base_type);
			}
			// Not a collection
			else
			{
				// Only add if not abstract
				if(!field_type.IsAbstract)
					types.Add(field_type);
			}

			// Add all the derived types
			types.AddRange(TypeCache.GetTypesDerivedFrom(base_type));

			// Add the null also if not already added
			if(obj_type is not null)
				types.Add(null);

			return types;
		}
#endregion
	}

	/// <summary>
	/// Popup field for selecting the concrete type.
	/// </summary>
	internal sealed class TypePopupField : PopupField<Type>
	{
#region Fields
		/// <summary>
		/// Original property.
		/// </summary>
		private readonly SerializedProperty m_Property;
#endregion

#region Methods Private
		/// <summary>
		/// Format the type to string.
		/// </summary>
		/// <param name="Type">Type.</param>
		/// <returns>String of the type name.</returns>
		private static string FormatType(Type? Type)
		{
			return Type is null ? "Null" : ObjectNames.NicifyVariableName(Type.Name);
		}
#endregion

#region Slots
		/// <summary>
		/// Handler for item selection.
		/// </summary>
		/// <param name="ChangeEvent">Event.</param>
		private void OnValueSelected(ChangeEvent<Type> ChangeEvent)
		{
			Type? selected_type = ChangeEvent.newValue;
			if(selected_type is null)
			{
				m_Property.managedReferenceValue = null;
				m_Property.serializedObject.ApplyModifiedProperties();
			}
			else
			{
				ConstructorInfo? constructor = selected_type.GetConstructor(Type.EmptyTypes);
				if(constructor is not null)
				{
					object new_obj = constructor.Invoke(null);
					m_Property.managedReferenceValue = new_obj;
					m_Property.serializedObject.ApplyModifiedProperties();
				}
				else
				{
					Debug.LogWarning(
						string.Format("Selected Type {0} does not have a parameterless constructor. Cannot assign instance of type.",
							selected_type.Name
						)
					);
				}
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Construction.
		/// </summary>
		/// <param name="Property">Property field.</param>
		/// <param name="Types">Types to select from.</param>
		public TypePopupField(SerializedProperty Property, List<Type?> Types)
			: base(Property.displayName,
				Types,
				0,
				FormatType,
				FormatType
			)
		{
			m_Property = Property;

			this.RegisterValueChangedCallback(OnValueSelected);
		}
#endregion
	}
}
