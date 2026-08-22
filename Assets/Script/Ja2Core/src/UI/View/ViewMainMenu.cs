using Aspid.MVVM;

using UnityEngine;

namespace Ja2.UI.View
{
	/// <summary>
	/// Main menu view
	/// </summary>
	[View]
	public sealed partial class ViewMainMenu : MonoView
	{
#region Fields Component
		/// <summary>
		/// Start the new game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandStartNewGame;

		/// <summary>
		/// Continue saved game command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandContinueSavedGame;

		/// <summary>
		/// Show preferences command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandPrefs;

		/// <summary>
		/// Show credits command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandCredits;

		/// <summary>
		/// Quit command.
		/// </summary>
		[RequireBinder(typeof(IRelayCommand))]
		[SerializeField]
		private MonoBinder[]? m_CommandQuit;
#endregion
	}
}
