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
		/// Video player component.
		/// </summary>
		[SerializeField]
		private VideoPlayer m_VideoPlayer = null!;

		/// <summary>
		/// See <see cref="videoClips"/>.
		/// </summary>
		[SerializeField]
		private VideoClip?[] m_VideoClips = Array.Empty<VideoClip>();
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

			// As first, load all the needed assets
			await m_MockManager!.LoadAssets(m_GameState.assetManager);

			// Play all the clips
			foreach(VideoClip? it in m_VideoClips)
			{
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
		}
#endregion
	}
}
