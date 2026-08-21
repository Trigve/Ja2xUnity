using NUnit.Framework;

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
		/// Instance of the singleton, only used in editor mode (see below).
		/// </summary>
		private static ScriptableObjectRuntime? m_Instance;
#endregion

#region Methods Private Static
#if UNITY_EDITOR
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeOnLoad()
		{
			Assert.IsNotNull(m_Instance);
			m_Instance!.OnEditorPlayModeEnable();
		}
#endif

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void InitializeSceneLoad()
		{
			Assert.IsNotNull(m_Instance);
			m_Instance!.OnSceneLoaded();
		}
#endregion

#region Methods Private
		protected virtual void OnEnable()
		{
			m_Instance = this;
		}

		/// <summary>
		/// This is emulation of OnEnable() in editor, so it behave same in editor and in the standalone player.
		/// </summary>
		protected virtual void OnEditorPlayModeEnable()
		{}

		/// <summary>
		/// Called when scene is loaded. Operations that needs the scene present should be done here.
		/// </summary>
		protected virtual void OnSceneLoaded()
		{}
#endregion
	}
}
