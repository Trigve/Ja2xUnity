using Aspid.MVVM;

namespace Ja2.UI.ViewModel
{
	/// <summary>
	/// ViewModel for the preference item.
	/// </summary>
	[ViewModel]
	public sealed partial class ViewModelPreferencesItem
	{
#region Fields
		/// <summary>
		/// Value placeholder.
		/// </summary>
		[Bind]
		private bool m_Value;

		/// <summary>
		/// Option description.
		/// </summary>
		[OneTimeBind]
		private readonly string m_Text;

		/// <summary>
		/// Game settings used.
		/// </summary>
		private readonly GameSettings m_GameSettings;

		/// <summary>
		/// Model.
		/// </summary>
		private readonly GameSettingOptionValue m_Item;
#endregion

#region Methods Private
		/// <summary>
		/// Value changed handler.
		/// </summary>
		/// <param name="newValue"></param>
		partial void OnValueChanged(bool newValue)
		{
			// Set new value
			m_GameSettings.SetValue(m_Item.optionData.m_OptionType,
				newValue
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Settings">Game settings used.</param>
		/// <param name="Option">Game settings option.</param>
		internal ViewModelPreferencesItem(GameSettings Settings, GameSettingOptionValue Option)
		{
			m_GameSettings = Settings;
			m_Item = Option;

			// Set the initial values
			m_Value = m_Item.GetValue<bool>();
			m_Text = m_Item.optionData.m_Text;
		}
#endregion
	}
}
