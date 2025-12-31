using System;
using System.Threading;

using UnityEngine;
using UnityEngine.Assertions;

namespace Ja2
{
	/// <summary>
	/// Global game state SO.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Game State")]
	public sealed class GameState : ScriptableObjectSingleton<GameState>
	{
#region Fields Component
		/// <summary>
		/// Mouse system manager.
		/// </summary>
		[SerializeField]
		private MouseSystemManager? m_MouseSystemManager;

		/// <summary>
		/// Random manager.
		/// </summary>
		[SerializeField ]
		private RandomManager? m_RandomManager;

		/// <summary>
		/// Virtual file system manager.
		/// </summary>
		[SerializeField]
		private Vfs.VfsManager? m_VfsManager;

		/// <summary>
		/// Input manager.
		/// </summary>
		[SerializeField]
		private InputManager? m_InputManager;

		/// <summary>
		/// Screen manager.
		/// </summary>
		[SerializeField]
		private ScreenManager? m_ScreenManager;

		/// See <see cref="assetManager"/>.
		[SerializeField]
		private AssetManager? m_AssetManager;
#endregion

#region Fields
		/// <summary>
		/// Global cancellation token.
		/// </summary>
		private CancellationTokenSource? m_CancellationTokenSource;

		/// <summary>
		/// Active camera backing field.
		/// </summary>
		private Camera? m_ActiveCamera;
#endregion

#region Properties
		/// <summary>
		/// Mouse system manager.
		/// </summary>
		public MouseSystemManager mouseSystemManager => m_MouseSystemManager!;

		/// <summary>
		/// VFS manager.
		/// </summary>
		public Vfs.VfsManager vfsManager => m_VfsManager!;

		/// <summary>
		/// Input manager.
		/// </summary>
		public InputManager inputManager => m_InputManager!;

		/// <summary>
		/// Screen manager.
		/// </summary>
		public ScreenManager screenManager => m_ScreenManager!;

		/// <summary>
		/// Asset manager.
		/// </summary>
		public AssetManager assetManager => m_AssetManager!;

		/// <summary>
		/// Get the new cancelation token.
		/// </summary>
		public CancellationToken cancellationToken => m_CancellationTokenSource?.Token ?? CancellationToken.None;

		/// <summary>
		/// Currently active camera.
		/// </summary>
		public Camera? activeCamera
		{
			get => m_ActiveCamera;
			set
			{
				// Deactivate the old camera's GO
				if(m_ActiveCamera != null)
					m_ActiveCamera.gameObject.SetActive(false);

				m_ActiveCamera = value;
			}
		}
#endregion

#region Events
		/// <summary>
		/// Event called during initialization.
		/// </summary>
		public event Action? eventStart;
#endregion

#region Methods Public
		/// <summary>
		/// Update the game state.
		/// </summary>
		protected override void DoUpdate()
		{
			m_InputManager!.Update();
			m_ScreenManager!.UpdateManager();
		}
#endregion

#region Methods Private
		/// <inheritdoc />
		protected override void DoInitialize()
		{
			Ja2Logger.LogInfo("Initializing game state ...");

			Assert.IsNotNull(m_MouseSystemManager);
			Assert.IsNotNull(m_RandomManager);
			Assert.IsNotNull(m_VfsManager);
			Assert.IsNotNull(m_InputManager);
			Assert.IsNotNull(m_ScreenManager);
			Assert.IsNotNull(m_AssetManager);

			m_CancellationTokenSource = new CancellationTokenSource();

			m_MouseSystemManager!.Initialize();
			m_RandomManager!.Initialize();
			m_VfsManager!.Initialize();
			m_InputManager!.Initialize();
			m_AssetManager!.Initialize();
			m_ScreenManager!.Initialize(cancellationToken);

			eventStart?.Invoke();
		}

		/// <inheritdoc />
		protected override void DoDeinitialize()
		{
			m_CancellationTokenSource?.Cancel();

			m_MouseSystemManager!.Deinitialize();
			m_RandomManager!.Deinitialize();
			m_VfsManager!.Deinitialize();
			m_InputManager!.Deinitialize();
			m_ScreenManager!.Deinitialize();
			m_AssetManager!.Deinitialize();

			m_CancellationTokenSource?.Dispose();
			m_CancellationTokenSource = null;

			eventStart = null;
			m_ActiveCamera = null;
		}
#endregion
	}

}
