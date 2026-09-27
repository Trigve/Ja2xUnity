using System.Linq;

using Aspid.Collections.Observable;
using Aspid.MVVM;

namespace Ja2.UI.ViewModel
{
	/// <summary>
	/// ViewModel providing the items to the view.
	/// </summary>
	[ViewModel]
	public sealed partial class ViewModelPreferencesAllItems
	{
#region Fields
		/// <summary>
		/// All the items.
		/// </summary>
		[OneTimeBind]
		private readonly ObservableList<ViewModelPreferencesItem>? m_Items;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Settings">Game settings.</param>
		internal ViewModelPreferencesAllItems(GameSettings Settings)
		{
			// Only items, that should be shown and are supported (bool values for now)
			m_Items = new ObservableList<ViewModelPreferencesItem>(
				Settings.options.Where(Item => (Item.optionData.m_Flags & GameSettingOptionData.OptionFlag.ShowInSettings) != 0 && Item.optionData.m_ValueType == GameSettingOptionData.ValueType.Boolean).Select(Item => new ViewModelPreferencesItem(Settings, Item))
			);
		}
#endregion
	}
}
