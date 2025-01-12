using System;
using System.Collections.Generic;
using System.Globalization;

namespace Ja2
{
	/// <summary>
	/// .ini file parser.
	/// </summary>
	internal sealed class IniParser
	{
#region Constants
		/// <summary>
		/// Valid tokens for value operations.
		/// </summary>
		private const string ValueOpTokens = "+=";

		/// <summary>
		/// Addition token.
		/// </summary>
		private const char ValueAddToken = '+';

		/// <summary>
		/// Value assignment token.
		/// </summary>
		private const char ValueAssignToken = '=';
#endregion

#region Enums
		/// <summary>
		/// Value operation type.
		/// </summary>
		private enum ValueOperation
		{
			Error,
			Set,
			Add,
		};
#endregion

#region Fields
		/// <summary>
		/// Sections.
		/// </summary>
		private readonly Dictionary<string, Section> m_mapProps = new Dictionary<string, Section>();
#endregion

#region Methods
		/// <summary>
		/// Get the section's key string value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public string getStringProperty(string Section, string Key, string DefaultValue = "")
		{
			string ret = DefaultValue;

			// Find section and key
			if(m_mapProps.TryGetValue(Section, out Section? section) && section.TryGetValue(Key, out string value))
				ret = value;

			return ret;
		}

		/// <summary>
		/// Get the section's key long value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public long getIntProperty(string Section, string Key, long DefaultValue)
		{
			long ret = DefaultValue;

			if(ValueForKey(Section,Key, out string value) && long.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out ret))
			{}

			return ret;
		}

		/// <summary>
		/// Get the section's key float value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public double getFloatProperty(string Section, string Key, double DefaultValue)
		{
			double ret = DefaultValue;

			if(ValueForKey(Section, Key, out string value) && double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out ret))
			{}

			return ret;
		}

		/// <summary>
		/// Get the value for the given section and the key.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="Value">Value found. Empty, if not found.</param>
		/// <returns>True if the key was found in the given section.</returns>
		private bool ValueForKey(string Section, string Key, out string Value)
		{
			Value = string.Empty;

			// \TODO Is it really needed to trim?
			return (m_mapProps.TryGetValue(Section.Trim(), out Section? section) && section.TryGetValue(Key.Trim(), out Value));
		}
#endregion

#region Methods Static
		/// <summary>
		/// Extract the section fromt he string.
		/// </summary>
		/// <param name="Input">Input string.</param>
		/// <param name="Section">Section name.</param>
		/// <returns>True, if successfull. Otherwise, false.</returns>
		private static bool ExtractSection(in ReadOnlySpan<char> Input, out ReadOnlySpan<char> Section)
		{
			var ret = false;
			Section = default;

			// Find the ending ']'
			int idx = Input.IndexOf(']');

			// Found something valid
			if(idx != -1 && idx > 0)
			{
				Section = Input[..(idx - 1)];

				ret = true;
			}

			return ret;
		}

		/// <summary>
		/// Extract the key and value pair.
		/// </summary>
		/// <param name="Input">Input string.</param>
		/// <param name="Key">Key.</param>
		/// <param name="Value">Value</param>
		/// <returns><see cref="ValueOperation.Error"/> if key/value couldn't be extracted. Otherwise, assignment or addition operation.</returns>
		private static ValueOperation ExtractKeyValue(in ReadOnlySpan<char> Input, out ReadOnlySpan<char> Key, out ReadOnlySpan<char> Value)
		{
			var ret = ValueOperation.Error;
			Key = default;
			Value = default;

			int idx = Input.IndexOfAny(ValueOpTokens);
			// No valid token found
			if(idx == -1)
			{
				Ja2Logger.LogWarning("WARNING : could not extract key-value pair '{0}'",
					Input.ToString()
				);
			}
			else
			{
				// Default value operation is to set value
				ret = ValueOperation.Set;

				// Key
				Key = Input[..(idx - 1)].Trim();

				// Value
				if(Input[idx] == ValueAddToken)
				{
					if((idx + 1) < Input.Length && (Input[idx + 1] == ValueAssignToken))
					{
						++idx;
						ret = ValueOperation.Add;
					}
				}

				Value = Input[(idx + 1)..Input.Length].Trim();
			}

			return ret;
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="File">Input file.</param>
		public IniParser(Vfs.IFileReadable File)
		{
			ReadOnlySpan<char> current_section = default;

			var line_counter = 0;

			var rl = new Vfs.LineReader(File);

			while(rl.ReadLine(out string line))
			{
				++line_counter;

				var line_span = line.AsSpan();

				// Very simple parsing : key = value
				if(line_span.Length > 0)
				{
					// Remove leading white spaces
					line_span = line_span.TrimStart(" \t");

					// only white space characters
					if(line_span.Length == 0)
						continue;

					switch(line_span[0])
					{
					// Comments
					case '!':
					case ';':
					case '#':
						break;
					// New section
					case '[':
						{
							// Try to extract section, use 1 char off becuase of '['
							if(ExtractSection(line_span[1..], out current_section))
								m_mapProps[current_section.ToString()] = new Section();
							else
							{
								Ja2Logger.LogVfs("Could not extract section name: '{0}', line: {1}",
									line,
									line_counter
								);
							}
						}
						break;
					// Key/pair values
					default:
						{
							ValueOperation op = ExtractKeyValue(line_span,
								out var sKey,
								out var sValue
							);

							if(op != ValueOperation.Error)
							{
								// Try to find section
								if(m_mapProps.TryGetValue(current_section.ToString(), out Section section))
								{
									if(op == ValueOperation.Set)
									{
										section.SetValue(sKey.ToString(),
											sValue.ToString()
										);
									}
									else if(op == ValueOperation.Add)
									{
										section.AddValue(sKey.ToString(),
											sValue.ToString()
										);
									}
								}
								else
								{
									Ja2Logger.LogWarning("Could not find section [{0}] in container, line: {1}",
										current_section.ToString(),
										line_counter
									);
								}
							}
						}
						break;
					}
				}
			}
		}
#endregion
	}

	/// <summary>
	/// Section of the .ini file.
	/// </summary>
	internal sealed class Section
	{
#region Fields
		/// <summary>
		/// Dictionary of the section values.
		/// </summary>
		private readonly Dictionary<string, string> m_MapProps = new Dictionary<string, string>();
#endregion

#region Methods
		/// <summary>
		/// Set the value for the given key.
		/// </summary>
		/// <param name="Key">Key, for which value is set.</param>
		/// <param name="Value">Value.</param>
		public void SetValue(string Key, string Value)
		{
			m_MapProps[Key] = Value;
		}

		/// <summary>
		/// Add value to existing value, if exists.
		/// </summary>
		/// <param name="Key">Key.</param>
		/// <param name="Value">Value to add.</param>
		/// <returns></returns>
		public void AddValue(string Key, string Value)
		{
			var value_new = string.Empty;

			// If key exist already and isn't empty, add separator
			if(m_MapProps.TryGetValue(Key, out string? old_value) && old_value.Length != 0)
				value_new = old_value + ", ";

			value_new += Value;

			m_MapProps[Key] = value_new;
		}

		/// <summary>
		/// Try to get the value for the given key.
		/// </summary>
		/// <param name="Key">Key.</param>
		/// <param name="Value">Value.</param>
		/// <returns>True, if key was found. Otherwise, false.</returns>
		public bool TryGetValue(string Key, out string Value)
		{
			return m_MapProps.TryGetValue(Key,
				out Value
			);
		}
#endregion
	}
}
