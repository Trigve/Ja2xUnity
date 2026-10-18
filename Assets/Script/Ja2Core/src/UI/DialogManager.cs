using System;
using System.Collections.Generic;

using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Dialog manager.
	/// </summary>
	public sealed class DialogManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Root canvas for the dialogs.
		/// </summary>
		[SerializeField]
		private Canvas m_Canvas = null!;
#endregion

#region Fields
		/// <summary>
		/// All the visible dialogs.
		/// </summary>
		private readonly Dictionary<IDialog, DialogHandle> m_Dialogs = new();
#endregion

#region Methods Public
		/// <summary>
		/// Create the dialog.
		/// </summary>
		/// <param name="DialogPrefab">Dialog prefab.</param>
		/// <returns></returns>
		public DialogHandle CreateMessageDialog(GameObject DialogPrefab)
		{
			// Instantiate the dialog
			GameObject dialog_go = Instantiate(DialogPrefab,
				m_Canvas.transform
			);

			var dialog = dialog_go.GetComponentInChildren<MessageDialogComponent>();

			var ret = new DialogHandle(dialog_go,
				dialog
			);

			// Listen for the dialog close event
			dialog.eventDialogClose += OnMessageDialogDestroy;

			m_Dialogs[dialog] = ret;

			return ret;
		}
#endregion

#region Slots
		/// <summary>
		/// Handle dialog closure.
		/// </summary>
		/// <param name="Sender">Sender object.</param>
		/// <param name="Event">Evenet.</param>
		private void OnMessageDialogDestroy(object Sender, EventArgs Event)
		{
			var dialog = (IDialog)Sender;

			// Destroy the main GO and remove from the map
			Destroy(m_Dialogs[dialog].m_GameObject);
			m_Dialogs.Remove(dialog);
		}
#endregion
	}
}
