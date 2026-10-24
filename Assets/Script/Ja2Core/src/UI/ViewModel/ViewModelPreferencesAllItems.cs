using System.Linq;
using System.Threading;

using UnityEngine;

using Aspid.Collections.Observable;
using Aspid.MVVM;

using Cysharp.Threading.Tasks;

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

		/// <summary>
		/// Game settings used.
		/// </summary>
		private readonly GameSettings m_GameSettings;

		/// <summary>
		/// Message dialog prefab.
		/// </summary>
		private readonly GameObject m_MessageDialogPrefab;
#endregion

#region Slots
		/// <summary>
		/// Handle the option change.
		/// </summary>
		/// <param name="Sender">Sender object.</param>
		/// <param name="Event">Event.</param>
		private async UniTask OnOptionChanged(object? Sender, EvenArgsGameOption Event)
		{
			// Only on disabled values
			if(!Event.value)
			{
				// Find if the option could be disabled
				GameSettingOptionGroup? options_problem = m_GameSettings.CheckOptionValue(Event.optionData.m_OptionType);
				if(options_problem is not null)
				{
					// Show the dialog
					DialogHandle dialog = BootstrapManager.instance.dialogManager.CreateMessageDialog(m_MessageDialogPrefab);

					// Generate the text from options that need to be set
					dialog.m_Dialog.text = string.Format("At least one of the {0} option must be set.",
						string.Join(", ",
							options_problem.m_OptionsNeeded.Select(Option => '"' + m_GameSettings[Option].optionData.m_Text + '"')
						)
					);

					await dialog.m_Dialog.ShowAsync(MessageDialogComponent.ButtonType.ButtonOk,
						CancellationToken.None
					);

					// Set the value back to true
					((ViewModelPreferencesItem)Sender!).ForceValue(true);
				}
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Settings">Game settings.</param>
		/// <param name="MessageDialogPrefab">Message dialog prefab.</param>
		internal ViewModelPreferencesAllItems(GameSettings Settings, GameObject MessageDialogPrefab)
		{
			m_Items = new ObservableList<ViewModelPreferencesItem>();
			m_GameSettings = Settings;
			m_MessageDialogPrefab = MessageDialogPrefab;

			// Only items, which should be shown and are supported (bool values for now)
			foreach(ViewModelPreferencesItem it in Settings.options.Where(Item => (Item.optionData.m_Flags & GameSettingOptionData.OptionFlag.ShowInSettings) != 0 && Item.optionData.m_ValueType == GameSettingOptionData.ValueType.Boolean).Select(Item => new ViewModelPreferencesItem(Settings, Item)))
			{
				it.eventOptionChanged += async (Sender, Event) => await OnOptionChanged(Sender, Event);

				m_Items.Add(it);
			}
		}
#endregion
	}
}
