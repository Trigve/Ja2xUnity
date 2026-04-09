using UnityEditor.UIElements;

using UnityEngine.UIElements;

namespace Ja2.Editor
{
	/// <summary>
	/// Base editor class for <see cref="UI.AssetRefMocker{T}"/>
	/// </summary>
	public abstract class AssetRefMockerBaseEditor : UnityEditor.Editor
	{
#region Methods Public
		/// <inheritdoc />
		public override VisualElement CreateInspectorGUI()
		{
			var root = new VisualElement();

			// Draw the default inspector
			InspectorElement.FillDefaultInspector(root,
				serializedObject,
				this
			);

			// "Add to mocker manager" button
			var button = new Button(OnAddTomanager);
			button.text = "Add to manager";

			root.Add(button);

			return root;
		}
#endregion

#region Slots
		/// <summary>
		/// "Add to mocker manager" button handler.
		/// </summary>
		private void OnAddTomanager()
		{
			// Find the manager GO
			var mocker_manager = FindAnyObjectByType<UI.AssetRefMockerManager>();

			mocker_manager.AddRefMocker((UI.IAssetRefMocker)serializedObject.targetObject);
		}
#endregion
	}
}
