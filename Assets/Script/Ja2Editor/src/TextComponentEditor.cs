using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine.UIElements;

namespace Ja2.Editor
{
	/// <summary>
	/// Editor for the <see cref="Ja2.UI.TextComponent"/>
	/// </summary>
	[CustomEditor(typeof(UI.TextComponent))]
	public sealed class TextComponentEditor : UnityEditor.Editor
	{
#region Fields
		/// <summary>
		/// Text component property.
		/// </summary>
		private SerializedProperty? m_TextComponent;

		/// <summary>
		/// Font asset property.
		/// </summary>
		private SerializedProperty? m_FontAssetProperty;

		/// <summary>
		/// Use shadow property.
		/// </summary>
		private SerializedProperty? m_ShadowUseProperty;

		/// <summary>
		/// Shadow color property.
		/// </summary>
		private SerializedProperty? m_ShadowColorIndexProperty;
#endregion

#region Messages
		private void OnEnable()
		{
			// Load all the properties
			m_TextComponent = serializedObject.FindProperty(UI.TextComponent.TextComponentFieldName);
			m_FontAssetProperty = serializedObject.FindProperty(UI.TextComponent.FontAssetFieldName);
			m_ShadowUseProperty = serializedObject.FindProperty(UI.TextComponent.UseShadowFieldName);
			m_ShadowColorIndexProperty = serializedObject.FindProperty(UI.TextComponent.ShadowColorIndexFieldName);
		}

		/// <inheritdoc/>
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new();

			// Text component
			PropertyField text_component_field = new(m_TextComponent);
			text_component_field.Bind(serializedObject);

			root.Add(text_component_field);

			// Font asset
			PropertyField font_asset_field = new(m_FontAssetProperty);
			font_asset_field.Bind(serializedObject);
			// Callback for the font change
			font_asset_field.TrackPropertyValue(m_FontAssetProperty,
				_ =>
				{
					serializedObject.ApplyModifiedProperties();
					((UI.TextComponent)serializedObject.targetObject).EditorFontChanged();
				}
			);

			root.Add(font_asset_field);

			// Shadow enabled
			PropertyField shadow_use_field = new(m_ShadowUseProperty);
			shadow_use_field.Bind(serializedObject);

			root.Add(shadow_use_field);

			// Shadow color, only visible while shadow is enabled
			SliderInt shadow_color_field = new("Shadow Color Index",
				0,
				255
			)
			{
				bindingPath = m_ShadowColorIndexProperty!.propertyPath
			};
			shadow_color_field.Bind(serializedObject);
			shadow_color_field.style.display = m_ShadowUseProperty!.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
			shadow_color_field.TrackPropertyValue(m_ShadowColorIndexProperty,
				_ => OnOtherPropertyChanged()
			);

			root.Add(shadow_color_field);

			// Handle shadow color field visibility
			shadow_use_field.RegisterValueChangeCallback(Event =>
				{
					shadow_color_field.style.display = Event.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
					OnOtherPropertyChanged();
				}
			);

			return root;
		}
#endregion

#region Slots
		/// <summary>
		/// Called, when other properties have changed.
		/// </summary>
		private void OnOtherPropertyChanged()
		{
			serializedObject.ApplyModifiedProperties();
			((UI.TextComponent)serializedObject.targetObject).EditorChangedOther();
		}
#endregion
	}
}
