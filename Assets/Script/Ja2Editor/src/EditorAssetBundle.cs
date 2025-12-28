using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// Asset bundle editor window.
	/// </summary>
	public sealed class EditorAssetBundle : EditorWindow
	{
#region Constants
		/// <summary>
		/// Build target name/value tuples.
		/// </summary>
		private static readonly (string name, BuildTarget value)[] TargetValues =
		{
			("Windows (64-bit)", BuildTarget.StandaloneWindows64),
			("Linux (64-bit)", BuildTarget.StandaloneLinux64),
			("macOS", BuildTarget.StandaloneOSX)
		};

		/// <summary>
		/// Compression option name/value tuples.
		/// </summary>
		private static readonly (string name, BuildAssetBundleOptions value)[] CompressionValues =
		{
			("No Compression", BuildAssetBundleOptions.UncompressedAssetBundle),
			("LZ4 Compression", BuildAssetBundleOptions.ChunkBasedCompression),
		};
#endregion

#region Fields
		/// <summary>
		/// Selected target index.
		/// </summary>
		///
		private int m_SelectedTargetIndex;

		/// <summary>
		/// Selected compression index.
		/// </summary>
		private int m_SelectedCompressionIndex = 1;

		/// <summary>
		/// Output path.
		/// </summary>
		private string m_OutputPath = string.Empty;

		/// <summary>
		/// Is debug build.
		/// </summary>
		private bool m_IsDebugBuild;

		/// <summary>
		/// List of bundles to use.
		/// </summary>
		private readonly List<AssetBundleInput> m_AssetBundleInput = new();
#endregion

#region properties
		/// <summary>
		/// Selected target.
		/// </summary>
		private BuildTarget selectedTarget => TargetValues[m_SelectedTargetIndex].value;

		/// <summary>
		/// Selected compression.
		/// </summary>
		private BuildAssetBundleOptions selectedCompression => CompressionValues[m_SelectedCompressionIndex].value;
#endregion

#region Methods Static
		[MenuItem("JA2/Build Asset Bundle")]
		public static void ShowWindow()
		{
			GetWindow<EditorAssetBundle>("Asset Bundle Builder");
		}
#endregion

#region Methods
		/// <summary>
		/// Build the asset bundle.
		/// </summary>
		private void BuildAssetBundles()
		{
			// Path for the temporary asset
			const string manifest_path = "Assets/" + AssetBundleInfo.FileName;

			// Project path
			string project_path = Directory.GetParent(Application.dataPath)!.FullName;

			// Directories to include
			List<AssetBundleDef> dir_to_search = new();

			// Only enabled
			foreach(AssetBundleInput it in m_AssetBundleInput.Where(Input => Input.isEnabled))
			{
				dir_to_search.Add(
					new AssetBundleDef()
					{
						directory = it.directory,
						assetBundleDesc = it.assetBundleDesc,
						recursive = true
					}
				);
			}

			foreach(AssetBundleDef bundle_info in dir_to_search)
			{
				var asset_mappings = new List<string>();
				var asset_real_paths = new List<string>();
				var asset_guids = new List<string>();

				string it_path = bundle_info.directory;

				var search_options = SearchOption.TopDirectoryOnly;
				if(bundle_info.recursive)
					search_options = SearchOption.AllDirectories;

				// Process files with specific extension only
				foreach(string file in Directory.EnumerateFiles(it_path, "*", search_options))
				{
					// Ignore budle descriptor
					if(AssetDatabase.LoadMainAssetAtPath(file) is AssetBundleDesc)
						continue;

					// Ignore all .meta files
					if(Path.GetExtension(file).ToLower() == ".meta")
						continue;

					// Addressable path
					asset_mappings.Add(
						UtilsPath.NormalizePath(
							Path.GetRelativePath(it_path,
								file
							)
						)
					);

					// "Real" path
					string asset_real_path = Path.GetRelativePath(project_path,
						file
					);
					asset_real_paths.Add(
						asset_real_path
					);

					// GUID
					asset_guids.Add(
						AssetDatabase.AssetPathToGUID(asset_real_path)
					);
				}

				// Create the manifest
				var bundle_manifest = AssetBundleInfo.Create(bundle_info.assetBundleDesc!.bundleId,
					asset_mappings.ToArray(),
					asset_guids.ToArray()
				);

				// Store to the manifest as asset
				AssetDatabase.CreateAsset(bundle_manifest,
					manifest_path
				);

				// Now add the manifest to the bundles
				asset_mappings.Add(AssetBundleInfo.FileName);
				asset_real_paths.Add(manifest_path);

				// Create asset bundle info
				var bundle_build = new AssetBundleBuild
				{
					assetBundleName = bundle_info.assetBundleDesc.fileName,
					addressableNames = asset_mappings.ToArray(),
					assetNames = asset_real_paths.ToArray()
				};

				BuildAssetBundleOptions options = selectedCompression;

				// For performance reason
				options |= BuildAssetBundleOptions.DisableLoadAssetByFileName | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;

				if(!m_IsDebugBuild)
					options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;

				Debug.LogFormat("Building Asset Bundles for {0}",
					selectedTarget
				);
				Debug.LogFormat("Selected files: {0}",
					string.Join(", ", asset_mappings)
				);
				Debug.LogFormat("Compression: {0}",
					CompressionValues[m_SelectedCompressionIndex]
				);
				Debug.LogFormat("Output: {0}",
					m_OutputPath
				);

				BuildPipeline.BuildAssetBundles(m_OutputPath,
					new[]
					{
						bundle_build
					},
					options,
					selectedTarget
				);

				// Remove the manifest
				AssetDatabase.DeleteAsset(manifest_path);
			}
		}
#endregion

#region Messages
		private void OnEnable()
		{
			// Load the config.
			var he_cfg = SettingsDev.instance;
			if(he_cfg != null)
			{
				// Set the default value
				m_OutputPath = he_cfg.bundleExportDir;

				// Find all the bundle descriptors
				foreach(AssetBundleDesc it in Resources.FindObjectsOfTypeAll<AssetBundleDesc>())
				{
					// Find the path, where is the descriptor located
					m_AssetBundleInput.Add(
						new AssetBundleInput(
							Path.GetDirectoryName(
								AssetDatabase.GetAssetPath(it)
							)!,
							it
						)
					);
				}
			}
		}

		private void OnGUI()
		{
			// All values are valid
			var are_values_valid = true;

			EditorGUILayout.Space();

			m_IsDebugBuild = GUILayout.Toggle(m_IsDebugBuild,
				"Debug build"
			);

			EditorGUILayout.Space();

			// Directory Selection
			GUILayout.Label("Select bundles to Include:",
				EditorStyles.boldLabel
			);

			foreach(AssetBundleInput it in m_AssetBundleInput)
			{
				using var _1 = new EditorGUI.IndentLevelScope(1);

				// Bundle info
				{
					using var _2 = new EditorGUILayout.HorizontalScope();

					// Enable/disable bundle
					it.isEnabled = EditorGUILayout.Toggle(it.assetBundleDesc.bundleName,
						it.isEnabled,
						GUILayout.ExpandWidth(false)
					);

					// Read only file name
					{
						using var _ = new EditorGUI.DisabledScope(true);

						EditorGUILayout.TextField("Bundle file name",
							it.directory
						);
					}
				}
			}

			EditorGUILayout.Space();

			// Build Target Selection
			GUILayout.Label("Target Platform:",
				EditorStyles.boldLabel
			);
			m_SelectedTargetIndex = EditorGUILayout.Popup(m_SelectedTargetIndex,
				TargetValues.Select(Value => Value.name).ToArray()
			);

			EditorGUILayout.Space();

			// In debug mode, no compression at all
			if(m_IsDebugBuild)
			{
				m_SelectedCompressionIndex = Array.FindIndex(CompressionValues,
					Value => Value.value == BuildAssetBundleOptions.UncompressedAssetBundle
				);
			}

			{
				using var _ = new EditorGUI.DisabledScope(m_IsDebugBuild);

				// Compression Selection
				GUILayout.Label("Compression:",
					EditorStyles.boldLabel
				);
				m_SelectedCompressionIndex = EditorGUILayout.Popup(m_SelectedCompressionIndex,
					CompressionValues.Select(Value => Value.name).ToArray()
				);
			}

			EditorGUILayout.Space();

			// Output Path
			GUILayout.Label("Output Path:",
				EditorStyles.boldLabel
			);
			EditorGUILayout.BeginHorizontal();
			m_OutputPath = EditorGUILayout.TextField(m_OutputPath);


			if(GUILayout.Button("Browse", GUILayout.Width(60)))
			{
				string path = EditorUtility.SaveFolderPanel("Choose Output Folder",
					m_OutputPath,
					""
				);
				if(!string.IsNullOrEmpty(path))
					m_OutputPath = path;
			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(20);

			if(string.IsNullOrEmpty(m_OutputPath))
			{
				are_values_valid = false;

				EditorGUILayout.HelpBox("\nOutput path must be provided!\n",
					MessageType.Error,
					true
				);
			}

			{
				using var _ = new EditorGUI.DisabledScope(!are_values_valid);
				// Build Button
				if(GUILayout.Button("Build Asset Bundles", GUILayout.Height(40)))
					BuildAssetBundles();
			}
		}
#endregion
	}

	/// <summary>
	/// Helper class for asset bundle creation.
	/// </summary>
	internal sealed class AssetBundleDef
	{
#region Properties
		/// <summary>
		/// Directory to use.
		/// </summary>
		public string directory { get; set; } = string.Empty;

		/// <summary>
		/// Asset bundle descriptor.
		/// </summary>
		public AssetBundleDesc? assetBundleDesc { get; set; }

		/// <summary>
		/// Should use recursive travel inside subdirectories.
		/// </summary>
		public bool recursive { get; set; }
#endregion
	}

	/// <summary>
	/// Input data for generating the asset bundles.
	/// </summary>
	internal sealed class AssetBundleInput
	{
#region Properties
		/// <summary>
		/// Source directory of the bundle.
		/// </summary>
		public string directory { get; set; }

		/// <summary>
		/// Asset bundle descriptor associated.
		/// </summary>
		public AssetBundleDesc assetBundleDesc { get; set; }

		/// <summary>
		/// Is the bundle enabled.
		/// </summary>
		public bool isEnabled { get; set; } = true;
#endregion

#region Construction
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="Directory">Directory of the asset bundle desc.</param>
		/// <param name="BundleDesc">Asset bundle descriptor.</param>
		public AssetBundleInput(string Directory, AssetBundleDesc BundleDesc)
		{
			directory = Directory;
			assetBundleDesc = BundleDesc;
		}
#endregion
	}
}
