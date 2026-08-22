using Aspid.MVVM;

namespace Ja2.UI.ViewModel
{
	[ViewModel]
	public sealed partial class ViewModelMainMenu
	{
#region Fields
		/// <summary>
		/// Command for the start new game menu item.
		/// </summary>
		[Bind]
		private readonly IRelayCommand m_CommandStartNewGame;

		/// <summary>
		/// Command for continue saved game menu item.
		/// </summary>
		[Bind]
		private readonly IRelayCommand m_CommandContinueSavedGame;

		/// <summary>
		/// Command for the preferences menu item.
		/// </summary>
		[Bind]
		private readonly IRelayCommand m_CommandPrefs;

		/// <summary>
		/// Command for the credist menu item.
		/// </summary>
		[Bind]
		private readonly IRelayCommand m_CommandCredits;

		/// <summary>
		/// Command for the quit menu item.
		/// </summary>
		[Bind]
		private readonly IRelayCommand m_CommandQuit;
#endregion

#region Construction
		/// <summary>
		/// Cosntructor.
		/// </summary>
		/// <param name="MainMenuModel">Main menu model instance.</param>
		public ViewModelMainMenu(ModelMainMenu MainMenuModel)
		{
			m_CommandStartNewGame = new RelayCommand(MainMenuModel.StartNewGame);
			m_CommandContinueSavedGame = new RelayCommand(MainMenuModel.ContinueSaveGame);
			m_CommandPrefs = new RelayCommand(MainMenuModel.ShowPreferences);
			m_CommandCredits = new RelayCommand(MainMenuModel.ShowCredits);
			m_CommandQuit = new RelayCommand(MainMenuModel.Quit);
		}
#endregion
	}
}
