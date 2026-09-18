using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using Object = UnityEngine.Object;

namespace Ja2.Editor
{
	/// <summary>
	/// <see cref="AssetRefMockerManager"/> editor.
	/// </summary>
	[CustomEditor(typeof(AssetRefMockerManager))]
	public sealed class AssetRefMockerManagerEditor : UnityEditor.Editor
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

			// Spacer
			root.Add(
				new VisualElement
				{
					style =
					{
						height = 8
					}
				}
			);

			var load_button = new Button(OnLoadAllAssets);
			load_button.text = "Load all assets";

			root.Add(load_button);

			var clear_button = new Button(OnResetAllAssets);
			clear_button.text = "Reset all assets";

			root.Add(clear_button);

			return root;
		}
#endregion

#region Slots
		/// <summary>
		/// "Load all assets" button handler.
		/// </summary>
		private void OnLoadAllAssets()
		{
			// Get all the mocker components
			foreach(AssetRefMockerBase it in FindObjectsByType<AssetRefMockerBase>(FindObjectsInactive.Include, FindObjectsSortMode.None))
			{
				var asset_list = new List<Object?>();

				var j = 0;
				foreach(AssetRef asset_ref in it.assetRefs)
				{
					// Find the right type
					Type asset_type = it.assetType.Length == 1 ? it.assetType[0] : it.assetType[j];

					Object? asset_loaded = null;

					if(asset_ref.isValid)
					{
						asset_loaded = EditorAssetManager.instance.LoadAsset(asset_ref,
							asset_type
						);
					}

					asset_list.Add(asset_loaded);

					++j;
				}

				it.LoadAssets(
					new AssetMockData(
						asset_list.ToArray()
					)
				);
			}
		}

		/// <summary>
		/// Reset all the assets in the referenced components.
		/// </summary>
		private void OnResetAllAssets()
		{
			// Get all the mocker components
			foreach(AssetRefMockerBase it in FindObjectsByType<AssetRefMockerBase>(FindObjectsInactive.Include, FindObjectsSortMode.None))
				it.ResetAssets();

			// Mark the scene dirty, so it is saved if needed
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
#endregion
	}
}
