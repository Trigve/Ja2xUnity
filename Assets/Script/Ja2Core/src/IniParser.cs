using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

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

#region Fields Static
		/// <summary>
		/// Case-insensitive comparer.
		/// </summary>
		private static readonly IEqualityComparer<string> StringComparer = new CaseInsensitiveStringComparer();
#endregion

#region Fields
		/// <summary>
		/// Sections.
		/// </summary>
		private readonly Dictionary<string, Section> m_MapProps = new(StringComparer);
#endregion

#region Methods Public
		/// <summary>
		/// Get the section's key string value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public string? GetStringProperty(string Section, string Key, string? DefaultValue = null)
		{
			string? ret = DefaultValue;

			// Find section and key
			if(m_MapProps.TryGetValue(Section, out Section? section) && section.TryGetValue(Key, out string value))
				ret = value;

			return ret;
		}

		/// <summary>
		/// Set the string property in the given section and given key.
		/// </summary>
		/// <param name="Section">Section, in which the key/value will be stored.</param>
		/// <param name="Key">Key for the value.</param>
		/// <param name="Value">Value to store.</param>
		public void SetProperty(string Section, string Key, string Value)
		{
			GetOrCreateSection(Section).SetValue(Key,
				Value
			);
		}

		/// <summary>
		/// Get the section's key long value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public long? GetIntProperty(string Section, string Key, long? DefaultValue = null)
		{
			long? ret = DefaultValue;

			if(ValueForKey(Section, Key, out string value_str) && long.TryParse(value_str, NumberStyles.Any, CultureInfo.InvariantCulture, out long value))
				ret = value;

			return ret;
		}

		/// <summary>
		/// Set the int property in the given section and given key.
		/// </summary>
		/// <param name="Section">Section, in which the key/value will be stored.</param>
		/// <param name="Key">Key for the value.</param>
		/// <param name="Value">Value to store.</param>
		public void SetProperty(string Section, string Key, long Value)
		{
			GetOrCreateSection(Section).SetValue(Key,
				Value.ToString(CultureInfo.InvariantCulture)
			);
		}

		/// <summary>
		/// Get the section's key float value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public double? GetFloatProperty(string Section, string Key, double? DefaultValue = null)
		{
			double? ret = DefaultValue;

			if(ValueForKey(Section, Key, out string value_str) && double.TryParse(value_str, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
				ret = value;

			return ret;
		}

		/// <summary>
		/// Set the float property in the given section and given key.
		/// </summary>
		/// <param name="Section">Section, in which the key/value will be stored.</param>
		/// <param name="Key">Key for the value.</param>
		/// <param name="Value">Value to store.</param>
		public void SetProperty(string Section, string Key, float Value)
		{
			GetOrCreateSection(Section).SetValue(Key,
				Value.ToString(CultureInfo.InvariantCulture)
			);
		}

		/// <summary>
		/// Get the section's key bool value.
		/// </summary>
		/// <param name="Section">Section to search in.</param>
		/// <param name="Key">Key.</param>
		/// <param name="DefaultValue">Default value, if section or key is not found.</param>
		/// <returns>Value for the given key in the given section if found. Otherwise <paramref name="DefaultValue"/>.</returns>
		public bool? GetBoolProperty(string Section, string Key, bool? DefaultValue = null)
		{
			bool? ret = DefaultValue;

			if(ValueForKey(Section, Key, out string value_str))
				ret = value_str.ToLower(CultureInfo.InvariantCulture) == "true";

			return ret;
		}

		/// <summary>
		/// Set the bool property in the given section and given key.
		/// </summary>
		/// <param name="Section">Section, in which the key/value will be stored.</param>
		/// <param name="Key">Key for the value.</param>
		/// <param name="Value">Value to store.</param>
		public void SetProperty(string Section, string Key, bool Value)
		{
			GetOrCreateSection(Section).SetValue(Key,
				Value ? "true" : "false"
			);
		}

		/// <summary>
		/// Write the content to the stream writeer.
		/// </summary>
		/// <param name="Writer">Stream writer instance.</param>
		public void Write(StreamWriter Writer)
		{
			foreach(var it in m_MapProps)
			{
				Writer.WriteLine("[{0}]",
					it.Key.Trim()
				);
				foreach(var it_sections in it.Value.values)
				{
					Writer.WriteLine("{0} = {1}",
						it_sections.Key,
						it_sections.Value
					);
				}

				Writer.WriteLine(string.Empty);
			}
		}
#endregion

#region Methods Private
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
			return (m_MapProps.TryGetValue(Section.Trim(), out Section? section) && section.TryGetValue(Key.Trim(), out Value));
		}

		/// <summary>
		/// Get or create the section.
		/// </summary>
		/// <param name="SectionName">Section name.</param>
		private Section GetOrCreateSection(string SectionName)
		{
			if(!m_MapProps.TryGetValue(SectionName, out Section? section_found))
			{
				section_found = new Section();
				m_MapProps[SectionName] = section_found;
			}

			return section_found;
		}
#endregion

#region Methods Static
		/// <summary>
		/// Extract the section from the string.
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
				Section = Input[..idx];

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
		/// Default constructor.
		/// </summary>
		public IniParser()
		{}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Reader">Stream reader instance to read from.</param>
		public IniParser(StreamReader Reader)
		{
			ReadOnlySpan<char> current_section = default;

			var line_counter = 0;

			while(Reader.Peek() >= 0)
			{
				string line = Reader.ReadLine()!;
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
								m_MapProps[current_section.ToString()] = new Section();
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
								if(m_MapProps.TryGetValue(current_section.ToString(), out Section section))
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
#region Fields Static
		/// <summary>
		/// Case-insensitive comparer.
		/// </summary>
		private static readonly IEqualityComparer<string> StringComparer = new CaseInsensitiveStringComparer();
#endregion

#region Fields
		/// <summary>
		/// Dictionary of the section values.
		/// </summary>
		private readonly Dictionary<string, string> m_MapValues = new(StringComparer);
#endregion

#region Properties
		/// <summary>
		/// All values in the section.
		/// </summary>
		public IEnumerable<KeyValuePair<string, string>> values => m_MapValues;
#endregion

#region Methods
		/// <summary>
		/// Set the value for the given key.
		/// </summary>
		/// <param name="Key">Key, for which value is set.</param>
		/// <param name="Value">Value.</param>
		public void SetValue(string Key, string Value)
		{
			m_MapValues[Key] = Value;
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
			if(m_MapValues.TryGetValue(Key, out string? old_value) && old_value.Length != 0)
				value_new = old_value + ", ";

			value_new += Value;

			m_MapValues[Key] = value_new;
		}

		/// <summary>
		/// Try to get the value for the given key.
		/// </summary>
		/// <param name="Key">Key.</param>
		/// <param name="Value">Value.</param>
		/// <returns>True, if key was found. Otherwise, false.</returns>
		public bool TryGetValue(string Key, out string Value)
		{
			return m_MapValues.TryGetValue(Key,
				out Value
			);
		}
#endregion
	}

	/// <summary>
	/// Case-insensitive comparer used for the keys.
	/// </summary>
	internal class CaseInsensitiveStringComparer : IEqualityComparer<string>
	{
#region Methods Public
		/// <inheritdoc/>
		public bool Equals(string x, string y)
		{
			return string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
		}

		/// <inheritdoc/>
		public int GetHashCode(string obj)
		{
			return obj.ToLowerInvariant().GetHashCode();
		}
#endregion
	}
}
