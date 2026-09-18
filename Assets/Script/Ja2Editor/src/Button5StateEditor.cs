using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;

namespace Ja2.Editor
{
	/// <summary>
	/// Editor for <see cref="UI.Button5State"/>.
	/// </summary>
	[CustomEditor(typeof(UI.Button5State))]
	public sealed class Button5StateEditor : ButtonEditor
	{
#region Methods Public
		/// <inheritdoc/>
		public override VisualElement CreateInspectorGUI()
		{
			SerializedProperty normal_on = serializedObject.FindProperty(UI.Button5State.PropertyNameNormalOn);
			SerializedProperty hilite_on = serializedObject.FindProperty(UI.Button5State.PropertyNameHiliteOn);
			SerializedProperty normal_off = serializedObject.FindProperty(UI.Button5State.PropertyNameNormalOff);
			SerializedProperty hilite_off = serializedObject.FindProperty(UI.Button5State.PropertyNameHiliteOff);
			SerializedProperty grayed = serializedObject.FindProperty(UI.Button5State.PropertyNameGrayed);
			SerializedProperty disable_style = serializedObject.FindProperty(UI.Button5State.PropertyNameDisableStyle);

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
				new PropertyField(grayed)
			);
			root.Add(
				new PropertyField(disable_style)
			);

			return root;
		}
#endregion
	}
}
