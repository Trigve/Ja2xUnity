using System;
using System.Collections.Generic;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// The class responsible for the credit's data.
	/// </summary>
	public sealed class CreditsDataAsset : AssetBase
	{
#region Fields Component
		/// <summary>
		/// All the nodes.
		/// </summary>
		[SerializeField]
		private List<CreditsDataNode> m_Nodes  = new();
#endregion

#region Properties
		/// <summary>
		/// Number of nodes.
		/// </summary>
		public int count => m_Nodes.Count;

		/// <summary>
		/// Indexer.
		/// </summary>
		/// <param name="Index">Index to get the value from.</param>
		public CreditsDataNode this[int Index] => m_Nodes[Index];
#endregion

#region Methods Public
		/// <summary>
		/// Add new node.
		/// </summary>
		/// <param name="Node">Node instance.</param>
		public void AddNode(CreditsDataNode Node)
		{
			m_Nodes.Add(Node);
		}
#endregion

#region Construction
		/// <summary>
		/// Create the instance.
		/// </summary>
		/// <returns>New instance.</returns>
		public static CreditsDataAsset Create()
		{
			var ret = CreateInstance<CreditsDataAsset>();

			return ret;
		}
#endregion
	}

	/// <summary>
	/// Node for the credits.
	/// </summary>
	[Serializable]
	public sealed class CreditsDataNode
	{
#region Fields
		/// <summary>
		/// Flags for the current node.
		/// </summary>
		[SerializeField]
		private List<CreditsDataNodeFlag> m_Flags = new ();

		/// <summary>
		/// Text.
		/// </summary>
		[SerializeField]
		private string m_Text;
#endregion

#region Properties
		/// <summary>
		/// Text.
		/// </summary>
		public string text => m_Text;

		/// <summary>
		/// Enumerate all the flags.
		/// </summary>
		public IEnumerable<CreditsDataNodeFlag> flags => m_Flags;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Text">Text to show.</param>
		/// <param name="Flags">Flags set from (and onwards) the given node.</param>
		public CreditsDataNode(string Text, IEnumerable<CreditsDataNodeFlag>? Flags = null)
		{
			if(Flags is not null)
				m_Flags = new List<CreditsDataNodeFlag>(Flags);

			m_Text = Text;
		}
#endregion
	}

	/// <summary>
	/// Flag with data for the <see cref="CreditsDataNode"/>.
	/// </summary>
	[Serializable]
	public sealed class CreditsDataNodeFlag
	{
#region Enums
		/// <summary>
		/// Flags type.
		/// </summary>
		[Flags]
		public enum FlagType
		{
			None = 0,

			/// <summary>
			/// Gap between the nodes.
			/// </summary>
			[HistoricName("CRDT_DELAY_BN_STRINGS_CODE")]
			GapNodes = 1 << 0,

			/// <summary>
			/// Gap between the sections.
			/// </summary>
			[HistoricName("CRDT_DELAY_BN_SECTIONS_CODE")]
			GapSection = 1 << 1,

			/// <summary>
			/// Scroll speed.
			/// </summary>
			[HistoricName("CRDT_SCROLL_SPEED")]
			ScrollSpeed =  1 << 2,

			/// <summary>
			/// Font justification.
			/// </summary>
			[HistoricName("CRDT_FONT_JUSTIFICATION")]
			FontJustify = 1 << 3,

			/// <summary>
			/// Title font color.
			/// </summary>
			[HistoricName("CRDT_TITLE_FONT_COLOR")]
			FontColorTitle = 1 << 4,

			/// <summary>
			/// Active text font color.
			/// </summary>
			[HistoricName("CRDT_ACTIVE_FONT_COLOR")]
			FontColorActive = 1 << 5,

			/// <summary>
			/// Title.
			/// </summary>
			[HistoricName("CRDT_TITLE")]
			Title = 1 << 6,

			/// <summary>
			/// Start of the section.
			/// </summary>
			[HistoricName("CRDT_START_OF_SECTION")]
			SectionStart =  1 << 7,

			/// <summary>
			/// End of the section.
			/// </summary>
			[HistoricName("CRDT_END_OF_SECTION")]
			SectionEnd =  1 << 8,
		}
#endregion

#region Fields
		/// <summary>
		/// See <see cref="flagType"/>.
		/// </summary>
		[SerializeField]
		private FlagType m_FlagType;

		/// <summary>
		/// See <see cref="data"/>.
		/// </summary>
		[SerializeField]
		private string m_Data;
#endregion

#region Properties
		/// <summary>
		/// Type of the flag.
		/// </summary>
		public FlagType flagType => m_FlagType;

		/// <summary>
		/// Data for the flags. Stored as string for simple implementation.
		/// </summary>
		public string data => m_Data;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Type">Flag type.</param>
		/// <param name="Data">Flag's data.</param>
		public CreditsDataNodeFlag(FlagType Type, string Data)
		{
			m_FlagType = Type;
			m_Data = Data;
		}
#endregion
	}
}
