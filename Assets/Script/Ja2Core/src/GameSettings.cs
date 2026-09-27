using System;
using System.Collections.Generic;

namespace Ja2
{
	/// <summary>
	/// Runtime game settings.
	/// </summary>
	internal sealed class GameSettings
	{
#region Fields
		/// <summary>
		/// All the options with values.
		/// </summary>
		private readonly Dictionary<GameSettingOptionData.OptionType, GameSettingOptionValue> m_Options;
#endregion

#region Properties
		/// <summary>
		/// Get all the options.
		/// </summary>
		public IEnumerable<GameSettingOptionValue> options => m_Options.Values;
#endregion

#region Methods Public
		/// <summary>
		/// Get the value for the given option.
		/// </summary>
		/// <param name="Option">Option.</param>
		/// <typeparam name="T">Returned type.</typeparam>
		/// <returns>Option value.</returns>
		public T GetValue<T>(GameSettingOptionData.OptionType Option) where T : struct
		{
			return m_Options[Option].GetValue<T>();
		}

		/// <summary>
		/// Set the value for the option.
		/// </summary>
		/// <param name="Option">Option.</param>
		/// <param name="Value">Value to set.</param>
		/// <typeparam name="T">Type of the value.</typeparam>
		public void SetValue<T>(GameSettingOptionData.OptionType Option, in T Value) where T : struct
		{
			GameSettingOptionValue option_value = m_Options[Option];

			// Set the value
			option_value.SetValue(Value);
			// Store the value back
			m_Options[Option] = option_value;
		}

		/// <summary>
		/// Save the settings to the INI.
		/// </summary>
		/// <param name="Ini"></param>
		public void Save(IniFile Ini)
		{
			foreach(GameSettingOptionValue it in options)
			{
				string section_name = it.optionData.m_SectionName;
				string key = it.optionData.m_Names[0];

				switch(it.optionData.m_ValueType)
				{

				case GameSettingOptionData.ValueType.String:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<string>()
					);
					break;
				case GameSettingOptionData.ValueType.Integer:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<int>()
					);
					break;
				case GameSettingOptionData.ValueType.Short:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<short>()
					);
					break;
				case GameSettingOptionData.ValueType.Long:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<long>()
					);
					break;
				case GameSettingOptionData.ValueType.Float:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<float>()
					);
					break;
				case GameSettingOptionData.ValueType.Double:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<double>()
					);
					break;
				case GameSettingOptionData.ValueType.Boolean:
					Ini.SetProperty(section_name,
						key,
						it.GetValue<bool>()
					);
					break;
				}
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="SettingsData">Game settings options.</param>
		public GameSettings(AssetGameSettingsData SettingsData)
		{
			m_Options = new Dictionary<GameSettingOptionData.OptionType, GameSettingOptionValue>();

			// Generate the options
			foreach(GameSettingOptionData it in SettingsData.options)
			{
				m_Options[it.m_OptionType] = new GameSettingOptionValue(it);
			}
		}

		/// <summary>
		/// Constructor with using INI file for initialization.
		/// </summary>
		/// <param name="SettingsData">Game settings option.</param>
		/// <param name="Ini">Ini file for initialization</param>
		public GameSettings(AssetGameSettingsData SettingsData, IniFile Ini)
		{
			m_Options = new Dictionary<GameSettingOptionData.OptionType, GameSettingOptionValue>();

			// Load from the INI file
			foreach(GameSettingOptionData it in SettingsData.options)
			{
				object? value = null;

				// Use all names for finding the value
				foreach(string key in it.m_Names)
				{
					// Get the value based on the type
					value = it.m_ValueType switch
					{
						GameSettingOptionData.ValueType.String => Ini.GetStringProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Long => Ini.GetIntProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Integer => (int?)Ini.GetIntProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Short => (short?)Ini.GetIntProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Double => Ini.GetFloatProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Float => (float?)Ini.GetFloatProperty(it.m_SectionName, key),
						GameSettingOptionData.ValueType.Boolean => Ini.GetBoolProperty(it.m_SectionName, key),
						_ => value
					};

					// Found the key
					if(value is not null)
						break;
				}

				m_Options[it.m_OptionType] = new GameSettingOptionValue(it,
					value
				);
			}
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Copy">Instance, from which the copy is made.</param>
		public GameSettings(GameSettings Copy)
		{
			m_Options = new Dictionary<GameSettingOptionData.OptionType, GameSettingOptionValue>(Copy.m_Options);
		}
#endregion
	}

	/// <summary>
	/// Game settings option with value.
	/// </summary>
	internal struct GameSettingOptionValue
	{
#region Fields
		/// <summary>
		/// Value for the option.
		/// </summary>
		private object m_Value;
#endregion

#region Properties
		/// <summary>
		/// Option for which data are defined.
		/// </summary>
		public GameSettingOptionData optionData { get; }
#endregion

#region Methods Public
		/// <summary>
		/// Get the value for the option.
		/// </summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <returns>Value for the given option.</returns>
		public T GetValue<T>()
		{
			return (T)m_Value;
		}

		/// <summary>
		/// Set the value for the option.
		/// </summary>
		/// <param name="Value">Value.</param>
		/// <typeparam name="T">Value type.</typeparam>
		public void SetValue<T>(in T Value)
		{
			// Valid combination of types and "real" types
			if(!(
				(optionData.m_ValueType == GameSettingOptionData.ValueType.String && typeof(T) == typeof(string)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Long && typeof(T) == typeof(long)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Integer && typeof(T) == typeof(int)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Short && typeof(T) == typeof(short)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Boolean && typeof(T) == typeof(bool)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Double && typeof(T) == typeof(double)) ||
				(optionData.m_ValueType == GameSettingOptionData.ValueType.Float && typeof(T) == typeof(float))
				) || Value is null
			)
			{
				throw new ArgumentException(nameof(Value),
					string.Format("Wrong option value type'{0}' for option '{1}'",
						typeof(T).Name,
						optionData.m_Names[0]
					)
				);
			}

			m_Value = Value;
		}
#endregion

#region Methods Private
		/// <summary>
		/// Read the value from the string.
		/// </summary>
		/// <param name="Option">Option.</param>
		/// <param name="Value">String value.</param>
		private static object ValueFromString(GameSettingOptionData Option, string Value)
		{
			return Option.m_ValueType switch
			{
				GameSettingOptionData.ValueType.String => Value,
				GameSettingOptionData.ValueType.Integer => int.Parse(Value, System.Globalization.CultureInfo.InvariantCulture),
				GameSettingOptionData.ValueType.Long => long.Parse(Value, System.Globalization.CultureInfo.InvariantCulture),
				GameSettingOptionData.ValueType.Short => short.Parse(Value, System.Globalization.CultureInfo.InvariantCulture),
				GameSettingOptionData.ValueType.Double => double.Parse(Value, System.Globalization.CultureInfo.InvariantCulture),
				GameSettingOptionData.ValueType.Float => float.Parse(Value, System.Globalization.CultureInfo.InvariantCulture),
				GameSettingOptionData.ValueType.Boolean => bool.Parse(Value),
				_ => throw new ArgumentOutOfRangeException(nameof(Value), "Unknown data type for setting option")
			};
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="OptionData">Option for which value could be set.</param>
		/// <param name="Value">Initial value for the option.</param>
		public GameSettingOptionValue(GameSettingOptionData OptionData, object? Value = null)
		{
			optionData = OptionData;

			// Default value
			if(Value is null)
			{
				m_Value = ValueFromString(optionData,
					optionData.m_DefaultValue
				);
			}
			else
				m_Value = Value;
		}
#endregion
	}
}
