using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ja2.Editor
{
	/// <summary>
	/// Utils for .edt (encrypted data) file.
	/// </summary>
	public static class EdtUtils
	{
#region Methods Static Public
		/// <summary>
		/// Decrypt the data.
		/// </summary>
		/// <param name="Data">Data string.</param>
		public static string Decrypt(string Data)
		{
			return string.Concat(
				Data
					.TakeWhile(Value => Value != '\0')
					.Select(Value => Value > 33 ? (char)(Value - 1) : Value)
			);
		}

		/// <summary>
		/// Parse the credits from the data.
		/// </summary>
		/// <param name="Data">Input bytes.</param>
		/// <returns>Instance of the <see cref="CreditsDataAsset"/>.</returns>
		public static CreditsDataAsset ParseCredits(byte[] Data)
		{
			// Line size in the .edt file.
			const int edt_line_size = 80 * 2;
			// Code start symbol
			const char code_start = '@';
			// Code end symbol
			const char code_end = ';';
			// Gap between nodes
			const char code_gap_nodes = 'D';
			// Gap between section
			const char code_gap_sections = 'B';
			// Scroll speed
			const char code_scroll_speed = 'S';
			// Justification
			const char code_justify = 'J';
			// Title font color
			const char code_color_title = 'C';
			// Active font color
			const char code_color_active = 'R';
			// Title
			const char code_title = 'T';
			// Section start
			const char code_section_start = '{';
			// Section end
			const char code_section_end = '}';

			var ret = CreditsDataAsset.Create();

			// Prepare the stream for reading
			using var stream = new MemoryStream(Data);

			// Line buffer
			var buffer = new byte[edt_line_size];

			// Read all the lines
			while(stream.Read(buffer) > 0)
			{
				// Decode to UTF-16 and decrypt
				string line_str = Decrypt(
					Encoding.Unicode.GetString(buffer)
				);

				// Normal text only
				if(line_str[0] != code_start)
				{
					ret.AddNode(
						new CreditsDataNode(line_str)
					);
				}
				else
				{
					// Index where code section ends
					int end_of_codes = line_str.IndexOf(code_end);

					// Retrieve all the codes from the string (ignoring the code marker)
					string all_codes = line_str[1..end_of_codes];
					// Retrieve the text
					string text = line_str[(end_of_codes + 1)..];

					var node_flags = new List<CreditsDataNodeFlag>();

					// Process all the codes
					foreach(string it in all_codes.Split(','))
					{
						char ctrl_code = it[0];
						string ctr_data =  it[1..];

						switch(ctrl_code)
						{
						case code_gap_nodes:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.GapNodes,
									ctr_data
								)
							);
							break;
						case code_gap_sections:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.GapSection,
									ctr_data
								)
							);
							break;
						case code_scroll_speed:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.ScrollSpeed,
									ctr_data
								)
							);
							break;
						case code_justify:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.FontJustify,
									ctr_data
								)
							);
							break;
						case code_color_title:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.FontColorTitle,
									ctr_data
								)
							);
							break;
						case code_color_active:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.FontColorActive,
									ctr_data
								)
							);
							break;
						case code_title:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.Title,
									string.Empty
								)
							);
							break;
						case code_section_start:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.SectionStart,
									string.Empty
								)
							);
							break;
						case code_section_end:
							node_flags.Add(
								new CreditsDataNodeFlag(CreditsDataNodeFlag.FlagType.SectionEnd,
									string.Empty
								)
							);
							break;
						}
					}

					ret.AddNode(
						new CreditsDataNode(text,
							node_flags
						)
					);
				}
			}

			return ret;
		}
#endregion
	}
}
