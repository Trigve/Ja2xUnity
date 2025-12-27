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
#if UNITY_EDITOR
		/// <summary>
		/// Instance of the singleton, only used in editor mode (see below).
		/// </summary>
		private static ScriptableObjectRuntime? m_Instance;
#endif
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
#endregion

#region Methods Private
		protected virtual void OnEnable()
		{
#if UNITY_EDITOR
			m_Instance = this;
#endif
		}

		/// <summary>
		/// This is emulation of OnEnable() in editor, so it behave same in editor and in the standalone player.
		/// </summary>
		protected virtual void OnEditorPlayModeEnable()
		{}
#endregion
	}
}
