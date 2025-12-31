using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Video;

using Ja2.Extensions.UnityComponentAsync;

using UnityEngine.Assertions;

namespace Ja2
{
	/// <summary>
	/// Intro screen game manager.
	/// </summary>
	[HistoricName("INTRO_SCREEN")]
	public sealed class ScreenIntroManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

		/// <summary>
		/// Mock manager.
		/// </summary>
		[SerializeField]
		private UI.AssetRefMockerManager? m_MockManager;

		/// <summary>
		/// Camera used.
		/// </summary>
		[SerializeField]
		private Camera? m_Camera;

		/// <summary>
		/// Video player component.
		/// </summary>
		[SerializeField]
		private VideoPlayer m_VideoPlayer = null!;

		/// <summary>
		/// See <see cref="videoClips"/>.
		/// </summary>
		[SerializeField]
		private VideoClip?[] m_VideoClips = Array.Empty<VideoClip>();

		/// <summary>
		/// Next screen to run.
		/// </summary>
		[SerializeField]
		private GameScreen? m_NextScreen;
#endregion

#region Properties
		/// <summary>
		/// All the video clips to play.
		/// </summary>
		public VideoClip?[] videoClips => m_VideoClips;
#endregion

#region Messages
		public async UniTaskVoid Start()
		{
			var cts = new CancellationTokenSource();

			// Set the active camera
			m_GameState.activeCamera = m_Camera;

			// As first, load all the needed assets
			await m_MockManager!.LoadAssets(m_GameState.assetManager);

			// Play all the clips
			foreach(VideoClip? it in m_VideoClips)
			{
				if(cts.IsCancellationRequested)
					break;

				Assert.IsNotNull(it);

				// Load the clip
				m_VideoPlayer.clip = it;

				await m_VideoPlayer.PrepareAsync(cts.Token);

				UniTask task = m_VideoPlayer.PlayAsync(cts.Token);
				while(task.Status == UniTaskStatus.Pending)
				{
					// Wait to be able to check for any input
					await UniTask.Yield();

					if(m_GameState.inputManager.inputReceived)
					{
						m_VideoPlayer.Stop();

						cts.Cancel();
						break;
					}
				}
			}

			if(m_NextScreen != null)
			{
				m_GameState.screenManager.SetPendingScreen(m_NextScreen,
					new GameScreenOptions()
					{
						destroyActiveSceen = true
					}
				);
			}
		}
#endregion
	}
}
