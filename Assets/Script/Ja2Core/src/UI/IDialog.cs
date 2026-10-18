using System;

namespace Ja2.UI
{
	/// <summary>
	/// Interface for the dialogs.
	/// </summary>
	public interface IDialog
	{
#region Events
		/// <summary>
		/// Event raised when the dialog is closed.
		/// </summary>
		public event EventHandler? eventDialogClose;
#endregion
	}
}
