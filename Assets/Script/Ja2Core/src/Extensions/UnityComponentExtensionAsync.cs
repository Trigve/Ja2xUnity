using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine.Video;

namespace Ja2.Extensions.UnityComponentAsync
{
	/// <summary>
	/// Unity component extensions.
	/// </summary>
	public static class UnityComponentExtensionAsync
	{
#region Methods
		/// <summary>
		/// Wait for the playback end.
		/// </summary>
		/// <param name="Self">VideoPlayer instance.</param>
		/// <param name="Token">Cancelation token.</param>
		public static async UniTask PlayAsync(this VideoPlayer Self, CancellationToken Token = default)
		{
			var tcs = new UniTaskCompletionSource();

			Self.loopPointReached += (_) => tcs.TrySetResult();

			// Cancelling support
			await using CancellationTokenRegistration registration = Token.Register(
				() =>
				{
					tcs.TrySetCanceled(Token);
				}
			);

			Self.Play();

			await tcs.Task;
		}

		/// <summary>
		/// Wait for prepare to finish.
		/// </summary>
		/// <param name="Self">VideoPlayer instance.</param>
		/// <param name="Token">Cancelation token.</param>
		/// <returns></returns>
		public static async UniTask PrepareAsync(this VideoPlayer Self, CancellationToken Token = default)
		{
			var tcs = new UniTaskCompletionSource();
			Self.prepareCompleted += (_) => tcs.TrySetResult();

			// Cancelling support
			await using CancellationTokenRegistration registration = Token.Register(
				() =>
				{
					tcs.TrySetCanceled(Token);
				}
			);

			Self.Prepare();

			await tcs.Task;
		}
#endregion
	}
}
