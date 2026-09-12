using System.Reflection;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using Object = UnityEngine.Object;

namespace Ja2.Editor
{
	/// <summary>
	/// Drawer for the <see cref="AssetRef"/>.
	/// </summary>
	[CustomPropertyDrawer(typeof(AssetRef))]
	public sealed class AssetRefDrawer : PropertyDrawer
	{
#region Methods Public
		/// <inheritdoc/>
		public override void OnGUI(Rect Position, SerializedProperty Property, GUIContent Label)
		{
			// The OnGUI() is implemented because SerializedDictionary<> doesn't support UI Toolkit based editor GUI

			using(new EditorGUI.PropertyScope(Position, Label, Property))
			{
				// Bundle path/ID
				SerializedProperty bundle_property = Property.FindPropertyRelative(AssetRef.PropertyNameBundle);
				// Asset path
				SerializedProperty asset_path_property = Property.FindPropertyRelative(AssetRef.PropertyNameAssetPath);

				// Bundle
				var bundle_rect = new Rect(Position.x,
					Position.y,
					Position.width,
					EditorGUIUtility.singleLineHeight
				);
				EditorGUI.PropertyField(bundle_rect,
					bundle_property,
					new GUIContent("Bundle")
				);

				// Asset path
				var asset_path_rect = new Rect(Position.x,
					bundle_rect.yMax + EditorGUIUtility.standardVerticalSpacing,
					Position.width,
					EditorGUIUtility.singleLineHeight
				);
				EditorGUI.PropertyField(asset_path_rect,
					asset_path_property,
					new GUIContent("Asset Path")
				);

				// Object selector
				var object_rect = new Rect(Position.x, asset_path_rect.yMax + EditorGUIUtility.standardVerticalSpacing, Position.width, EditorGUIUtility.singleLineHeight);

				EditorGUI.BeginChangeCheck();
				Object new_asset = EditorGUI.ObjectField(object_rect,
					"Asset",
					null,
					fieldInfo.GetCustomAttribute<Attributes.AssetRefTypeAttribute>()?.type ?? typeof(Object),
					false
				);
				if(EditorGUI.EndChangeCheck() && new_asset != null)
				{
					var asset_ref_found = EditorAssetManager.instance.GetAssetRefFromAsset(new_asset);

					// \FIXME Asset ref may not be valid when???
					if(asset_ref_found.HasValue)
					{
						bundle_property.stringValue = asset_ref_found.Value.bundle;
						asset_path_property.stringValue = asset_ref_found.Value.assetPath;

						Property.serializedObject.ApplyModifiedProperties();
					}
				}
			}
		}

		/// <inheritdoc/>
		public override VisualElement CreatePropertyGUI(SerializedProperty Property)
		{
			// Create property container element.
			var root = new VisualElement();

			// Bundle path/ID
			SerializedProperty bundle_property = Property.FindPropertyRelative(AssetRef.PropertyNameBundle);
			var bundle_property_field = new TextField("Bundle");
			bundle_property_field.BindProperty(bundle_property);
			root.Add(bundle_property_field);

			// Asset path
			SerializedProperty asset_path_property = Property.FindPropertyRelative(AssetRef.PropertyNameAssetPath);
			var asset_path_property_field = new TextField("Asset Path");
			asset_path_property_field.BindProperty(asset_path_property);
			root.Add(asset_path_property_field);

			var obj_field = new ObjectField("Asset");
			// Find the allowed type
			obj_field.objectType = fieldInfo.GetCustomAttribute<Attributes.AssetRefTypeAttribute>()?.type ?? typeof(Object);

			// Object changed
			obj_field.RegisterValueChangedCallback(
				Event =>
				{
					// If not resetting
					if(Event.newValue != null)
					{
						var asset_ref_found = EditorAssetManager.instance.GetAssetRefFromAsset(Event.newValue);

						// \FIXME Asset ref may not be valid when???
						if(asset_ref_found.HasValue)
						{
							bundle_property.stringValue = asset_ref_found.Value.bundle;
							asset_path_property.stringValue = asset_ref_found.Value.assetPath;

							// Reset the object field
							obj_field.SetValueWithoutNotify(null);

							Property.serializedObject.ApplyModifiedProperties();
						}
					}
					// Reset
					else
					{
						bundle_property.stringValue = asset_path_property.stringValue = string.Empty;

						Property.serializedObject.ApplyModifiedProperties();
					}
				}
			);

			root.Add(obj_field);

			return root;
		}

		/// <inheritdoc/>
		public override float GetPropertyHeight(SerializedProperty Property, GUIContent Label)
		{
			float line_height = EditorGUIUtility.singleLineHeight;
			float spacing = EditorGUIUtility.standardVerticalSpacing;

			// Bundle row + Asset Path row + Asset object-picker row
			return (line_height * 3f) + (spacing * 2f);
		}

#endregion
	}
}
