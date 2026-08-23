namespace Ja2
{
	/// <summary>
	/// Main menu model interface.
	/// </summary>
	public interface IModelMainMenu
	{
#region Methods Public
		/// <summary>
		/// Start new game.
		/// </summary>
		public void StartNewGame();

		/// <summary>
		/// Continue the saved game
		/// </summary>
		public void ContinueSaveGame();

		/// <summary>
		/// Show the preferences window.
		/// </summary>
		public void ShowPreferences();

		/// <summary>
		/// Show the credits.
		/// </summary>
		public void ShowCredits();

		/// <summary>
		/// Quit the game.
		/// </summary>
		public void Quit();
#endregion
	}
}
