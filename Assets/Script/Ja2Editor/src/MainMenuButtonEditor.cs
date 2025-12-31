using UnityEditor;
using UnityEditor.UI;

namespace Ja2.Editor
{
	/// <summary>
	/// Editor for <see cref="UI.MainMenuButton"/>.
	/// </summary>
	[CustomEditor(typeof(UI.MainMenuButton))]
	public sealed class MainMenuButtonEditor : ButtonEditor
	{
#region Fields
		/// <summary>
		/// Image control.
		/// </summary>
		private SerializedProperty? m_Image;

		/// <summary>
		/// Normal sprite.
		/// </summary>
		private SerializedProperty? m_Normal;

		/// <summary>
		/// Highlited sprite.
		/// </summary>
		private SerializedProperty? m_Highlighted;

		/// <summary>
		/// Pressed sprite.
		/// </summary>
		private SerializedProperty? m_Pressed;

		/// <summary>
		/// Disabled sprite.
		/// </summary>
		private SerializedProperty? m_Disabled;
#endregion

#region Messages
		protected override void OnEnable()
		{
			base.OnEnable();

			m_Image = serializedObject.FindProperty("m_TargetGraphic");
			m_Normal = serializedObject.FindProperty(nameof(m_Normal));
			m_Highlighted = serializedObject.FindProperty(nameof(m_Highlighted));
			m_Pressed = serializedObject.FindProperty(nameof(m_Pressed));
			m_Disabled = serializedObject.FindProperty(nameof(m_Disabled));
		}
#endregion

#region Methods Public
		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Sprite",
				EditorStyles.boldLabel
			);

			EditorGUILayout.PropertyField(m_Image);
			EditorGUILayout.PropertyField(m_Normal);
			EditorGUILayout.PropertyField(m_Highlighted);
			EditorGUILayout.PropertyField(m_Pressed);
			EditorGUILayout.PropertyField(m_Disabled);

			serializedObject.ApplyModifiedProperties();
		}
#endregion
	}
}
