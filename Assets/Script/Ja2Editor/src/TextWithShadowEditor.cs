using UnityEditor;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// Editor for the <see cref="Ja2.UI.TextWithShadow"/>
	/// </summary>
	[CustomEditor(typeof(UI.TextWithShadow))]
	public sealed class TextWithShadowEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
	{
#region Fields
		/// <summary>
		/// Use shadow property.
		/// </summary>
		private SerializedProperty? m_ShadowUseProperty;

		/// <summary>
		/// Shadow color property.
		/// </summary>
		private SerializedProperty? m_ShadowColorIndexProperty;

		/// <summary>
		/// Font asset property.
		/// </summary>
		private SerializedProperty? m_FontAssetProperty;
#endregion

#region Messages
		/// <inheritdoc/>
		protected override void OnEnable()
		{
			base.OnEnable();

			// Load the properties
			m_ShadowUseProperty = serializedObject.FindProperty("m_UseShadow");
			m_ShadowColorIndexProperty = serializedObject.FindProperty("m_ShadowColorIndex");
			m_FontAssetProperty = serializedObject.FindProperty("m_FontAsset");
		}

		/// <inheritdoc/>
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			EditorGUILayout.Space();

			// Shadow properties
			EditorGUILayout.LabelField("Shadow",
				EditorStyles.boldLabel
			);

			// Font asset
			EditorGUILayout.PropertyField(m_FontAssetProperty);

			// Shadow enabled
			EditorGUILayout.PropertyField(m_ShadowUseProperty);

			// Shadow color
			if(m_ShadowUseProperty!.boolValue)
			{
				EditorGUILayout.IntSlider(m_ShadowColorIndexProperty,
					0,
					255
				);
			}

			serializedObject.ApplyModifiedProperties();

			// Apply changes if needed
			if(GUI.changed)
				((UI.TextWithShadow)target).ApplyChanges();
		}
#endregion
	}
}
