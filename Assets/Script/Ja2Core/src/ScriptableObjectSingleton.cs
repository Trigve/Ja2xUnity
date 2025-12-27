using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Ja2
{
	/// <summary>
	/// Base class for the scriptable object (SO) singletons.
	///
	/// SO singleton is defined as an asset and is used as dependency (as in DI)
	/// in other SO/GameObjects. Losly based on  https://unity.com/how-to/architect-game-code-scriptable-objects?ref=manuel-rauber.com#architect-events
	/// and https://github.com/SimonNordon4/unity-architecture-patterns
	/// </summary>
	/// <typeparam name="T">Derived class type.</typeparam>
	public abstract class ScriptableObjectSingleton<T> : ScriptableObjectRuntime where T : class
	{
#region Fields
		/// <summary>
		/// Is in play mode.
		/// </summary>
		private bool m_IsPlayMode;

		/// <summary>
		/// Is player loop active.
		/// </summary>
		private bool m_IsActiveLoop;
#endregion

#region Messages
		protected override void OnEnable()
		{
			base.OnEnable();

			if(Application.isPlaying)
				m_IsPlayMode = true;

#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
			// Only initialize in the playmode
			if(m_IsPlayMode)
				Initialize();
		}

		private void OnDisable()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif

			// Only in play mode
			if(m_IsPlayMode)
				Deinitialize();
		}
#endregion


#region Methods Private
		/// <inheritdoc />.
		protected override void OnEditorPlayModeEnable()
		{
			m_IsPlayMode = true;
			Initialize();
		}

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

		/// <summary>
		/// Override to implement Update() functionality
		/// </summary>
		protected virtual void DoUpdate()
		{
		}

		/// <summary>
		/// Do some initialization stuff.
		/// </summary>
		private void Initialize()
		{
			Ja2Logger.LogInfo("{0} initializing ...",
				typeof(T)
			);
			Assert.IsFalse(m_IsActiveLoop);

			if(!m_IsActiveLoop)
			{
				PlayerLoopSystem player_loop = PlayerLoop.GetCurrentPlayerLoop();

				// Create our player loop system
				var update_system = new PlayerLoopSystem
				{
					type = typeof(ScriptableObjectSingleton<T>),
					updateDelegate = OnUpdate
				};

				// Create a new list to populate with subsystems, including the custom system
				List<PlayerLoopSystem> new_sub_system_list = new();

				//Iterate through the subsystems in the existing loop we passed in and add them to the new list
				if(player_loop.subSystemList != null)
				{
					foreach(PlayerLoopSystem it in player_loop.subSystemList)
					{
						new_sub_system_list.Add(it);
						// If the previously added subsystem is of the type to add after, add the custom system
						if(it.type == typeof(Update))
							new_sub_system_list.Add(update_system);
					}
				}

				player_loop.subSystemList = new_sub_system_list.ToArray();

				// Set the new syste,
				PlayerLoop.SetPlayerLoop(player_loop);

				m_IsActiveLoop = true;
			}

			DoInitialize();

#if UNITY_EDITOR
			Ja2Logger.LogInfo("{0} initialized {1}",
				typeof(T),
				(uint)GetInstanceID()
			);
#endif
		}

		/// <summary>
		/// Do some deinitialization stuff.
		/// </summary>
		private void Deinitialize()
		{
			if(m_IsActiveLoop)
			{
				PlayerLoopSystem player_loop = PlayerLoop.GetCurrentPlayerLoop();

				player_loop.subSystemList = Array.FindAll(player_loop.subSystemList,
					System => System.type != typeof(ScriptableObjectSingleton<T>)
				);
				PlayerLoop.SetPlayerLoop(player_loop);

				m_IsActiveLoop = false;
			}

			DoDeinitialize();

#if UNITY_EDITOR
			Ja2Logger.LogInfo("{0} deinitialized {1}",
				typeof(T),
				(uint)GetInstanceID()
			);
#endif
		}

#if UNITY_EDITOR
		private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange StateChange)
		{
			if(StateChange == UnityEditor.PlayModeStateChange.ExitingPlayMode)
			{
				Deinitialize();
				m_IsPlayMode = false;
			}

		}
#endif
#endregion

#region Slots
		/// <summary>
		/// Handle update loop.
		/// </summary>
		private void OnUpdate()
		{
			// Only in playmode
			Assert.IsTrue(m_IsPlayMode);

			DoUpdate();
		}
#endregion
	}
}
