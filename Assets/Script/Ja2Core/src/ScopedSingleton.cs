using UnityEngine.Assertions;

namespace Ja2
{
	/// <summary>
	/// Scoped singleton.
	/// </summary>
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoad]
#endif
	internal class ScopedSingleton<T> where T : class, new()
	{
#region Fields Static
		/// <summary>
		/// Instance.
		/// </summary>
		private static T? m_Instance;
#endregion

#region Properties Static
		/// <summary>
		/// Instance.
		/// </summary>
		public static T instance => m_Instance!;
#endregion

#region Slots Static
#if UNITY_EDITOR
		/// <summary>
		/// Called, when the editor playmode has changed.
		/// </summary>
		/// <param name="State"></param>
		private static void OnPlayModeChanged(UnityEditor.PlayModeStateChange State)
		{
			if(State == UnityEditor.PlayModeStateChange.ExitingPlayMode)
			{
				// Reset instance
				m_Instance = null;
			}
		}
#endif
#endregion

#region Construction
		/// <summary>
		/// Static constructor.
		/// </summary>
		static ScopedSingleton()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
		}

		/// <summary>
		/// Initialization.
		/// </summary>
		public static void Init()
		{
			// Should be reseted already
			Assert.IsNull(m_Instance);

			m_Instance = new T();
		}
#endregion
	}

}
