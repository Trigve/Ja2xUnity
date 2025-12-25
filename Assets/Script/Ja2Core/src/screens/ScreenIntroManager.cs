using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Video;

using Ja2.Extensions.UnityComponentAsync;

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
		/// Video player component.
		/// </summary>
		[SerializeField]
		private VideoPlayer m_VideoPlayer = null!;

		/// <summary>
		/// Splash video asset.
		/// </summary>
		[SerializeField]
		private AssetRef m_VideoSplash;
#endregion

#region Messages
		public async UniTaskVoid Start()
		{
			var cts = new CancellationTokenSource();

			// Load the clip
			m_VideoPlayer.clip = await m_GameState.assetManager.LoadAssetAsync<VideoClip>(m_VideoSplash)!.AttachExternalCancellation<VideoClip>(cts.Token);

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

/*
			if( gfIntroScreenEntry )
			{
				EnterIntroScreen();
				gfIntroScreenEntry = FALSE;
				gfIntroScreenExit = FALSE;

				InvalidateRegion( 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT );
			}


			GetIntroScreenUserInput();

			HandleIntroScreen();

			ExecuteBaseDirtyRectQueue();
			EndFrameBufferRender();


			if( gfIntroScreenExit )
			{
				ExitIntroScreen();
				gfIntroScreenExit = FALSE;
				gfIntroScreenEntry = TRUE;
			}

			return( guiIntroExitScreen );
*/
		}
#endregion
	}
}
