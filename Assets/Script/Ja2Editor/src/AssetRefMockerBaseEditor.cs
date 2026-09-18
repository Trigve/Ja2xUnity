using System;
using System.Collections.Generic;

using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

using Object = UnityEngine.Object;

namespace Ja2.Editor
{
	/// <summary>
	/// Base editor class for <see cref="AssetRefMockerBase"/>
	/// </summary>
	[CustomEditor(typeof(AssetRefMockerBase), true)]
	public abstract class AssetRefMockerBaseEditor : UnityEditor.Editor
	{
#region Fields
		/// <summary>
		/// Asset refs.
		/// </summary>
		private SerializedProperty m_AssetRefs = null!;
#endregion

#region Messages
		private void OnEnable()
		{
			m_AssetRefs = serializedObject.FindProperty(AssetRefMockerBase.PropertyNameAssetRefs);
		}
#endregion

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

			var gather_button = new Button(OnGatherAssets);
			gather_button.text = "Gather assets";
			root.Add(gather_button);

			var load_button = new Button(OnLoadAssets);
			load_button.text = "Load assets";
			root.Add(load_button);

			var clear_button = new Button(OnResetAssets);
			clear_button.text = "Reset assets";
			root.Add(clear_button);

			return root;
		}
#endregion

#region Slots
		/// <summary>
		/// Gather all assets of the given component.
		/// </summary>
		private void OnGatherAssets()
		{
			serializedObject.Update();

			var mocker_component = (AssetRefMockerBase)serializedObject.targetObject;

			Undo.RecordObject(mocker_component.componentsModified,
				"Gather component data "
			);
			// Need to mark it as modified, otherwise, it wouldn't be saved to scene, see
			// https://discussions.unity.com/t/updating-prefab-variable-via-script-doesnt-save-override/727795/5
			PrefabUtility.RecordPrefabInstancePropertyModifications(mocker_component.componentsModified);

			mocker_component.GatherAssets();
		}

		/// <summary>
		/// Load all assets from the mocker.
		/// </summary>
		private void OnLoadAssets()
		{
			var mocker_component = (AssetRefMockerBase)serializedObject.targetObject;

			var asset_list = new List<Object?>();

			// Loop through all the asset refs
			for(var i = 0; i < m_AssetRefs.arraySize; ++i)
			{
				var asset_ref = (AssetRef)m_AssetRefs.GetArrayElementAtIndex(i).boxedValue;

				// Find the right type
				Type asset_type = mocker_component.assetType.Length == 1 ? mocker_component.assetType[0] : mocker_component.assetType[i];

				Object? asset_loaded = null;

				if(asset_ref.isValid)
				{
					asset_loaded = EditorAssetManager.instance.LoadAsset(asset_ref,
						asset_type
					);
				}

				asset_list.Add(asset_loaded);
			}

			mocker_component.LoadAssets(
				new AssetMockData(
					asset_list.ToArray()
				)
			);
		}

		/// <summary>
		/// Reset the assets in the referenced component.
		/// </summary>
		private void OnResetAssets()
		{
			var mocker_component = (AssetRefMockerBase)serializedObject.targetObject;

			Undo.RecordObject(mocker_component.componentsModified,
				"Reset assets"
			);
			PrefabUtility.RecordPrefabInstancePropertyModifications(mocker_component.componentsModified);

			mocker_component.ResetAssets();

			EditorUtility.SetDirty(mocker_component.componentsModified);
		}
#endregion
	}
}
