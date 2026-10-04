using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Scene manager active during the scene (not the best name for the class - to avoid conflicts with Unity). Needs
	/// to be run as the "first" item in the scene.
	/// </summary>
	[DefaultExecutionOrder(-10)]
	public abstract class SceneManagerSingleton : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		protected GameState m_GameState = null!;
#endregion

#region Messages
		public void Awake()
		{
			// Set the scene manager
			m_GameState.SetSceneManager(gameObject.scene,
				this
			);

			DoAwake();
		}

		public void OnDestroy()
		{
			DoOnDestroy();

			// Remove the scene manager
			m_GameState.RemoveSceneManager(gameObject.scene);
		}
#endregion

#region Methods Private
		/// <summary>
		/// Override in children for Awake() struff.
		/// </summary>
		protected virtual void DoAwake()
		{}

		/// <summary>
		/// Override in children for OnDestroy() stuff.
		/// </summary>
		protected virtual void DoOnDestroy()
		{}
#endregion
	}
}
