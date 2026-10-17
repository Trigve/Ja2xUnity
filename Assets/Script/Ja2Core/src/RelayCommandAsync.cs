using System;
using System.Threading;

using Aspid.MVVM;

using Cysharp.Threading.Tasks;

namespace Ja2
{
	/// <summary>
	/// Asynchronous relay command.
	/// </summary>
	public sealed class RelayCommandAsync : IRelayCommand, IDisposable
	{
#region Fields
		/// <summary>
		/// Function to execute.
		/// </summary>
		private readonly Func<CancellationToken, UniTask> m_Func;

		/// <summary>
		/// "Can execute" function.
		/// </summary>
		private readonly Func<bool>? m_CanExecuteFunc;

		/// <summary>
		/// Cancellation token source.
		/// </summary>
		private CancellationTokenSource? m_CancellationTokenSource;

		/// <summary>
		/// Is the command executing?
		/// </summary>
		private bool m_IsExecuting;
#endregion

#region Events
		/// <summary>
		/// "Can execute" flag changed.
		/// </summary>
		public event Action<IRelayCommand>? CanExecuteChanged;
#endregion

#region Methods Public
		/// <inheritdoc/>
		public bool CanExecute()
		{
			// If the function is being executed, cannot execute once more. The task needs to finish
			return !m_IsExecuting && (m_CanExecuteFunc?.Invoke() ?? true);
		}

		/// <inheritdoc/>
		public void Execute()
		{
			m_IsExecuting = true;
			NotifyCanExecuteChanged();

			ExecuteAsync().Forget();
		}

		/// <inheritdoc/>
		public void NotifyCanExecuteChanged()
		{
			CanExecuteChanged?.Invoke(this);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			m_CancellationTokenSource?.Cancel();
			// Disable notifications, as nothing could be done next
			CanExecuteChanged = null;
		}
#endregion

#region Methods Private
		/// <summary>
		/// Asynchronously executes the command.
		/// </summary>
		/// <returns>Task.</returns>
		private async UniTaskVoid ExecuteAsync()
		{
			m_CancellationTokenSource = new CancellationTokenSource();

			try
			{
				await m_Func(m_CancellationTokenSource.Token);
			}
			catch (OperationCanceledException) when (m_CancellationTokenSource.IsCancellationRequested)
			{
				// Canceled on purpose, not an error.
			}
			finally
			{
				m_CancellationTokenSource.Dispose();
				m_CancellationTokenSource = null;

				// Notify that execution has finished
				m_IsExecuting = false;
				NotifyCanExecuteChanged();
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Func">User function must take <see cref="CancellationToken"/> as argument.</param>
		/// <param name="CanExecuteFunc">Can execute function.</param>
		public RelayCommandAsync(Func<CancellationToken, UniTask> Func, Func<bool>? CanExecuteFunc = null)
		{
			m_Func = Func;
			m_CanExecuteFunc = CanExecuteFunc;
			m_IsExecuting = false;
		}
#endregion
	}
}
