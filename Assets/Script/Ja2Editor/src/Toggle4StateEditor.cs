using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;

namespace Ja2.Editor
{
	/// <summary>
	/// Editor for <see cref="UI.Toggle4State"/>.
	/// </summary>
	[CustomEditor(typeof(UI.Toggle4State))]
	public sealed class Toggle4StateEditor : ToggleEditor
	{
#region Methods Public
		/// <inheritdoc/>
		public override VisualElement CreateInspectorGUI()
		{
			SerializedProperty normal_on = serializedObject.FindProperty(UI.Toggle4State.PropertyNameNormalOn);
			SerializedProperty hilite_on = serializedObject.FindProperty(UI.Toggle4State.PropertyNameHiliteOn);
			SerializedProperty normal_off = serializedObject.FindProperty(UI.Toggle4State.PropertyNameNormalOff);
			SerializedProperty hilite_off = serializedObject.FindProperty(UI.Toggle4State.PropertyNameHiliteOff);
			SerializedProperty image = serializedObject.FindProperty(UI.Toggle4State.PropertyNameImage);

			VisualElement root = new();

			// Preserve the original Button inspector
			root.Add(new IMGUIContainer(() => OnInspectorGUI()));

			root.Add(
				new ToolbarSpacer()
			);

			// Label divider
			root.Add(
				new Label("Sprite")
				{
					style =
					{
						unityFontStyleAndWeight = FontStyle.Bold
					}
				}
			);

			root.Add(
				new PropertyField(normal_on)
			);
			root.Add(
				new PropertyField(hilite_on)
			);
			root.Add(
				new PropertyField(normal_off)
			);
			root.Add(
				new PropertyField(hilite_off)
			);
			root.Add(
				new PropertyField(image)
			);

			return root;
		}
#endregion
	}
}
