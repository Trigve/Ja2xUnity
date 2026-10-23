using System;
using System.Threading;

using UnityEngine;
using UnityEngine.Assertions;

using TMPro;

using Cysharp.Threading.Tasks;

namespace Ja2.UI
{
	/// <summary>
	/// Component handling the message dialog UI.
	/// </summary>
	public sealed class MessageDialogComponent : MonoBehaviour, IDialog
	{
#region Enums
		/// <summary>
		/// Button types.
		/// </summary>
		[Flags]
		public enum ButtonType
		{
			ButtonNone = 0,
			ButtonYes = 1 << 0,
			ButtonNo = 1 << 1,
			ButtonOk = 1 << 2,
		}
#endregion

#region Fields Component
		/// <summary>
		/// Yes button.
		/// </summary>
		[Header("Buttons")]
		[SerializeField]
		private Button5State m_ButtonYes = null!;

		/// <summary>
		/// No button.
		/// </summary>
		[SerializeField]
		private Button5State m_ButtonNo = null!;

		/// <summary>
		/// Ok button.
		/// </summary>
		[SerializeField]
		private Button5State m_ButtonOk = null!;
#endregion

#region Fields
		/// <summary>
		/// Text control.
		/// </summary>
		private TMP_Text? m_Text;

		/// <summary>
		/// Task completion source.
		/// </summary>
		private UniTaskCompletionSource<ButtonType>? m_Tcs;
#endregion

#region Properties
		/// <summary>
		/// Text.
		/// </summary>
		public string text
		{
			set => m_Text!.text = value;
		}
#endregion

#region Events
		/// <inheritdoc/>.
		public event EventHandler? eventDialogClose;
#endregion

#region Messages
		public void Awake()
		{
			m_Text = GetComponentInChildren<TMP_Text>();
		}
#endregion

#region Methods Public
		/// <summary>
		/// Show the dialog asynchronously.
		/// </summary>
		/// <param name="Buttons">Buttons to show.</param>
		/// <param name="Token">Cancellation token.</param>
		/// <returns></returns>
		public async UniTask<ButtonType> ShowAsync(ButtonType Buttons, CancellationToken Token)
		{
			// Add buttons
			if((Buttons & ButtonType.ButtonYes) != 0)
			{
				AddButton(ButtonType.ButtonYes,
					m_ButtonYes
				);
			}
			if((Buttons & ButtonType.ButtonNo) != 0)
			{
				AddButton(ButtonType.ButtonNo,
					m_ButtonNo
				);
			}
			if((Buttons & ButtonType.ButtonOk) != 0)
			{
				AddButton(ButtonType.ButtonOk,
					m_ButtonOk
				);
			}

			// Must not exist
			Assert.IsNull(m_Tcs);

			m_Tcs = new UniTaskCompletionSource<ButtonType>();

			// Cancelling support
			await using CancellationTokenRegistration registration = Token.Register(() => m_Tcs.TrySetCanceled(Token));

			ButtonType ret = await m_Tcs.Task;

			eventDialogClose?.Invoke(this,
				EventArgs.Empty
			);

			return ret;
		}
#endregion

#region Methods Private
		/// <summary>
		/// Add the new button.
		/// </summary>
		/// <param name="ButtonType">Button type.</param>
		/// <param name="Button">Button instance.</param>
		private void AddButton(ButtonType ButtonType, Button5State Button)
		{
			// Activate the button GO
			Button.gameObject.SetActive(true);

			// Complete the task on button click
			Button.onClick.AddListener(() => m_Tcs!.TrySetResult(ButtonType));
		}
#endregion
	}
}
