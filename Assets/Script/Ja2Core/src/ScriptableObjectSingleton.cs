using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Base class for the scriptable object (SO) singletons.
	///
	/// SO singleton is defined as an asset and is used as dependency (as in DI)
	/// in other SO/GameObjects. Losly based on  https://unity.com/how-to/architect-game-code-scriptable-objects?ref=manuel-rauber.com#architect-events
	/// </summary>
	/// <typeparam name="T">Derived class type.</typeparam>
#if UNITY_EDITOR
#endif
	public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : class
	{
#region Methods Public
		/// <summary>
		/// Do some initialization stuff.
		/// </summary>
		public void Initialize()
		{
#if UNITY_EDITOR
			Ja2Logger.LogInfo("{0} initialized {1}",
				typeof(T),
				(uint)GetInstanceID()
			);
#endif
			DoInitialize();
		}

		/// <summary>
		/// Do some deinitialization stuff.
		/// </summary>
		public void Deinitialize()
		{
			DoDeinitialize();

#if UNITY_EDITOR
			Ja2Logger.LogInfo("{0} deinitialized {1}",
				typeof(T),
				(uint)GetInstanceID()
			);
#endif
		}
#endregion

#region Methods Private
		/// <summary>
		/// Override in derived class to do some initialization.
		/// </summary>
		protected virtual void DoInitialize()
		{
		}

		/// <summary>
		/// Override in derived class to do some deinitialization.
		/// </summary>
		protected virtual void DoDeinitialize()
		{
		}
#endregion
	}
}
