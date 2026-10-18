using UnityEngine;

namespace Ja2.UI
{
	/// <summary>
	/// Struct encapsulating a dialog GO and managing component.
	/// </summary>
	public readonly struct DialogHandle
	{
#region Fields
		/// <summary>
		/// Game object representing the dialog.
		/// </summary>
		public readonly GameObject m_GameObject;

		/// <summary>
		/// Managing component of the dialog.
		/// </summary>
		public readonly MessageDialogComponent m_Dialog;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="GameObject">Game object representing the dialog.</param>
		/// <param name="Dialog">Managing component of the dialog.</param>
		public DialogHandle(GameObject GameObject, MessageDialogComponent Dialog)
		{
			m_Dialog = Dialog;
			m_GameObject = GameObject;
		}
#endregion
	}
}
