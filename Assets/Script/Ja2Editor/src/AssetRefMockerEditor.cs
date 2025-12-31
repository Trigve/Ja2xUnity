using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ja2.Editor
{
	/// <summary>
	/// <see cref="UI.AssetRefMockerManager"/> editor.
	/// </summary>
	[CustomEditor(typeof(UI.AssetRefMockerManager))]
	public sealed class AssetRefMockerEditor : UnityEditor.Editor
	{
#region Fields
		/// <summary>
		/// Assets.
		/// </summary>
		private SerializedProperty m_AssetMocks = null!;
#endregion

#region Messages
		private void OnEnable()
		{
			m_AssetMocks = serializedObject.FindProperty(nameof(m_AssetMocks));
		}
#endregion

#region Methods Public
		/// <inheritdoc />
		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			DrawDefaultInspector();

			EditorGUILayout.Space();

			if(GUILayout.Button("Gather all assets"))
			{
				// Clear the data
				m_AssetMocks.ClearArray();

				var root_objects = new Queue<GameObject>(
					SceneManager.GetActiveScene().GetRootGameObjects()
				);

				while(root_objects.Count > 0)
				{
					GameObject top_go = root_objects.Dequeue();

					// Get all the children
					foreach(Transform it in top_go.transform)
						root_objects.Enqueue(it.gameObject);

					// Process all the components in the current GO
					foreach(Component it in top_go.GetComponents<Component>())
					{
						if(it is UI.IAssetRefMocker mocker_component)
						{
							Undo.RecordObject(mocker_component.componentsModified,
								"Clear component data "
							);

							var asset_mock = mocker_component.GatherAssets();

							if(asset_mock == null)
							{
								Debug.LogWarningFormat("{0}: Component not set for the '{1}'",
									nameof(AssetRefMockerEditor),
									it.gameObject
								);

								continue;
							}

							var asset_refs = new List<AssetRef>();

							// Load all the asset refs
							foreach(Object? it_asset in asset_mock.Value.m_Assets)
							{
								var asset_ref = new AssetRef();

								// Only if there is some valid asset
								if(it_asset != null)
								{
									var asset_ref_found = EditorAssetManager.instance.GetAssetRefFromAsset(it_asset);

									// \FIXME Asset ref may not be valid when???
									if(asset_ref_found.HasValue)
										asset_ref = asset_ref_found.Value;
								}

								asset_refs.Add(asset_ref);
							}

							// Need to mark it as modified, otherwise, it wouldn't be saved to scene, see
							// https://discussions.unity.com/t/updating-prefab-variable-via-script-doesnt-save-override/727795/5
							PrefabUtility.RecordPrefabInstancePropertyModifications(mocker_component.componentsModified);

							// Add new item
							++m_AssetMocks.arraySize;
							SerializedProperty element_last = m_AssetMocks.GetArrayElementAtIndex(m_AssetMocks.arraySize - 1);
							element_last.boxedValue = new UI.AssetRefMockerInstance(mocker_component,
								asset_refs.ToArray()
							);
						}
					}
				}

				serializedObject.ApplyModifiedProperties();
			}
			if(GUILayout.Button("Load all assets"))
			{
				for(var i = 0; i < m_AssetMocks.arraySize; ++i)
				{
					SerializedProperty asset_mock = m_AssetMocks.GetArrayElementAtIndex(i);

					var mocker_component = (UI.IAssetRefMocker)asset_mock.FindPropertyRelative(
						nameof(UI.AssetRefMockerInstance.m_Component)
					).boxedValue;

					SerializedProperty asset_refs = asset_mock.FindPropertyRelative(
						nameof(UI.AssetRefMockerInstance.m_AssetRefs)
					);

					var asset_list = new List<Object?>();

					for(var j = 0; j < asset_refs.arraySize; ++j)
					{
						Object? asset_loaded = null;

						var asset_ref = (AssetRef)asset_refs.GetArrayElementAtIndex(j).boxedValue;
						if(asset_ref.isValid)
						{
							asset_loaded = EditorAssetManager.instance.LoadAsset(asset_ref,
								mocker_component.assetType[0]
							);
						}

						asset_list.Add(asset_loaded);
					}

					mocker_component.LoadAssets(
						new UI.AssetMockData(
							asset_list.ToArray()
						)
					);
				}
			}
		}
#endregion
	}
}
