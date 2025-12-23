using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Manager as scriptable object.
	/// </summary>
	public abstract class ScriptableObjectManager<T> : ScriptableObject where T : class
	{
#region Methods Public
		/// <summary>
		/// Do some initialization stuff.
		/// </summary>
		/// <param name="Params">Optional arguments.</param>
		public void Initialize(params object[] Params)
		{
#if UNITY_EDITOR
			Ja2Logger.LogInfo(
				"Initializing SO manager {0} ...",
				typeof(T)
			);
#endif
			DoInitialize(Params);
		}

		/// <summary>
		/// Do some deinitialization stuff.
		/// </summary>
		public void Deinitialize()
		{
#if UNITY_EDITOR
			Ja2Logger.LogInfo("Deinitializing SO manager {0} ...",
				typeof(T)
			);
#endif
			DoDeinitialize();
		}
#endregion

#region Methods Private
		/// <summary>
		/// /// Override in derived class to do some initialization.
		/// </summary>
		/// <param name="Params">Optional arguments.</param>
		protected virtual void DoInitialize(params object[] Params)
		{}

		/// <summary>
		/// Override in derived class to do some deinitialization.
		/// </summary>
		protected virtual void DoDeinitialize()
		{}
#endregion
	}
}
