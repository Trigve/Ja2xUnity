using System;

namespace Ja2
{
	/// <summary>
	/// Event arguments used with game options.
	/// </summary>
	public sealed class EvenArgsGameOption : EventArgs
	{
#region Properties
		/// <summary>
		/// Game setting's option data.
		/// </summary>
		public GameSettingOptionData optionData { get; }

		/// <summary>
		/// Value of the given option.
		/// </summary>
		public bool value { get; }
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="OptionData">See <see cref="optionData"/>.</param>
		/// <param name="Value">See <see cref="value"/>.</param>
		public EvenArgsGameOption(GameSettingOptionData OptionData, bool Value)
		{
			optionData = OptionData;
			value = Value;
		}
#endregion
	}
}
