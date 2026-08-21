using System.Collections.Generic;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Base class for runtime ScriptableObjects.
	/// </summary>
	public abstract class ScriptableObjectRuntime : ScriptableObject
	{
#region Fields Static
		/// <summary>
		/// All the loaded instances.
		/// </summary>
		private static readonly List<ScriptableObjectRuntime> m_Instances = new List<ScriptableObjectRuntime>();
#endregion

#region Methods Static Private
		/// <summary>
		/// Initialize all the <see cref="ScriptableObjectRuntime"/> object.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad()
		{
			m_Instances.Clear();

			// Force load of all SO needed and explicitly instantiate it to avoid race condition
			// when using OnEnable()
			foreach(ScriptableObjectRuntime it in Resources.LoadAll<ScriptableObjectRuntime>("State"))
			{
				m_Instances.Add(it);
#if UNITY_EDITOR
				it!.OnEditorPlayModeEnable();
#endif
			}

		}

		/// <summary>
		/// Run the initialization code after scene has been loaded.
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeSceneLoad()
		{
			// Process all loaded instances
			foreach(ScriptableObjectRuntime it in m_Instances)
				it.OnSceneLoaded();
		}
#endregion

#region Messages
		protected virtual void OnEnable()
		{
		}
#endregion

#region Methods Private
#if UNITY_EDITOR
		/// <summary>
		/// This is emulation of OnEnable() in editor, so it behave same in editor and in the standalone player.
		/// </summary>
		protected virtual void OnEditorPlayModeEnable()
		{}
#endif

		/// <summary>
		/// Called when scene is loaded. Operations that needs the scene present should be done here.
		/// </summary>
		protected virtual void OnSceneLoaded()
		{}
#endregion
	}
}
