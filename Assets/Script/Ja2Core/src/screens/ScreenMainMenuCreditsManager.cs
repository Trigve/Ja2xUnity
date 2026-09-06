using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using UnityEngine;

using TMPro;

namespace Ja2
{
	/// <summary>
	/// Screen manager for the main menu's credits
	/// </summary>
	public sealed class ScreenMainMenuCreditsManager : MonoBehaviour
	{
#region Constants
		/// <summary>
		/// Space between the nodes in pixels.
		/// </summary>
		[HistoricName("CRDT_SPACE_BN_NODES")]
		private const ushort NodesSpace = 12;

		/// <summary>
		/// Scroll speed in miliseconds for 1 pixel for the nodes.
		/// </summary>
		[HistoricName("CRDT_NODE_DELAY_AMOUNT")]
		[HistoricName("CRDT_SCROLL_PIXEL_AMOUNT")]
		private const ushort NodeScrollSpeed = 25;

		/// <summary>
		/// Title text predefined color.
		/// </summary>
		[HistoricName("FONT_MCOLOR_RED")]
		private const ushort ColorTitleDefault = 163;

		/// <summary>
		/// Default text predefined color.
		/// </summary>
		[HistoricName("FONT_MCOLOR_DKWHITE")]
		private const ushort ColorScreenDefault = 134;
#endregion

#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

		/// <summary>
		/// Asset ref mocker manager.
		/// </summary>
		[SerializeField]
		private AssetRefMockerManager? m_AssetRefMocker;

		/// <summary>
		/// Data component.
		/// </summary>
		[SerializeField]
		private CreditsDataComponent? m_DataComponent;

		/// <summary>
		/// Prefab for the text component.
		/// </summary>
		[SerializeField]
		private GameObject? m_TextPrefab;

		/// <summary>
		/// Canvas GO.
		/// </summary>
		[SerializeField]
		private Canvas? m_Canvas;

		/// <summary>
		/// Starting position for the credits.
		/// </summary>
		[SerializeField]
		private RectTransform? m_PositionStart;

		/// <summary>
		/// Edning position for the credits.
		/// </summary>
		[SerializeField]
		private RectTransform? m_PositionEnd;

		/// <summary>
		/// Screen that should be run upon exit.
		/// </summary>
		[SerializeField]
		private GameScreen? m_ExitScreen;
#endregion

#region Fields
		/// <summary>
		/// Credits data instance.
		/// </summary>
		private CreditsDataAsset? m_CreditsData;

		/// <summary>
		/// Current node index being processed.
		/// </summary>
		private int m_NodeIndex;

		/// <summary>
		/// Queue of the nodes, that are currently shown.
		/// </summary>
		private Queue<CreditsDataNodeComponent> m_NodesShown = new();

		/// <summary>
		/// Space margin, when new node is displayed.
		/// </summary>
		[HistoricName("guiGapTillReadNextCredit")]
		private ushort m_SpaceForNextNodeRead;

		/// <summary>
		/// Space between the credits nodes.
		/// </summary>
		[HistoricName("guiGapBetweenCreditNodes")]
		private ushort m_SpaceBetweenNodes;

		/// <summary>
		/// Space between the credits sections.
		/// </summary>
		[HistoricName("guiGapBetweenCreditSections")]
		private ushort m_SpaceBetweenSections;

		/// <summary>
		/// Current text color.
		/// </summary>
		private ushort m_CurrentColor;

		/// <summary>
		/// Current title text color.
		/// </summary>
		private ushort m_CurrentColorTitle;

		/// <summary>
		/// Current font.
		/// </summary>
		[HistoricName("guiCreditScreenTitleFont")]
		private AssetFontClass? m_CurrentFont;

		/// <summary>
		/// Current text alignment.
		/// </summary>
		[HistoricName("gubCrdtJustification")]
		private HorizontalAlignmentOptions m_CurrentTextAlignment;

		/// <summary>
		/// Current font size.
		/// </summary>
		private int m_CurrentFontSize;

		/// <summary>
		/// Current scroll speed.
		/// </summary>
		private ushort m_CurrentScrollSpeed;

		/// <summary>
		/// Is the screen paused.
		/// </summary>
		private bool m_IsPaused;
#endregion

#region Messages
		public void Start()
		{
			m_NodesShown = new Queue<CreditsDataNodeComponent>();

			m_AssetRefMocker!.LoadAssets(m_GameState.assetManager);

			// Get the actual credits data after loading the data
			m_CreditsData = m_DataComponent!.m_CreditsData;

			m_NodeIndex = 0;
			m_SpaceForNextNodeRead = NodesSpace;
			m_CurrentScrollSpeed = NodeScrollSpeed;
			m_SpaceBetweenNodes = NodesSpace;
			m_CurrentColor = ColorScreenDefault;
			m_CurrentColorTitle = ColorTitleDefault;
			m_CurrentTextAlignment = HorizontalAlignmentOptions.Center;
			m_IsPaused = false;

			m_GameState.eventUpdate += OnUpdate;
		}

		public void OnDestroy()
		{
			m_GameState.eventUpdate -= OnUpdate;
		}
#endregion

#region Slots
		/// <summary>
		/// Update is called on each frame.
		/// </summary>
		private void OnUpdate()
		{
			// Pause handling
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Pause))
				m_IsPaused = !m_IsPaused;

			// If paused, skip the processing
			if(m_IsPaused)
				return;

			// Credit speed down
			if(Input.GetKeyDown(KeyCode.UpArrow))
				m_CurrentScrollSpeed += 5;
			// Credit speed up
			else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				// Couldn't go past the minimum scroll speed
				m_CurrentScrollSpeed = (ushort)Mathf.Max(m_CurrentScrollSpeed - 5, 5);
			}

			// Forced exit or nothing to process
			if(Input.GetKeyDown(KeyCode.Escape) || (m_NodesShown.Count == 0 && m_NodeIndex >= m_CreditsData!.count))
			{
				m_GameState.screenManager.SetPendingScreen(m_ExitScreen!,
					new GameScreenOptions()
					{
						destroyActiveSceen = true
					}
				);

				// Don't run the update anymore
				m_GameState.eventUpdate -= OnUpdate;

				return;
			}

			// No nodes or the last node position exceeded the space between the two nodes
			if(m_NodesShown.Count == 0 || m_NodesShown.Last().SpaceDiffY(m_PositionStart!) > m_SpaceForNextNodeRead)
			{
				var process_next_node = true;

				// Process till there is a text node and processing shouldn't stop
				for(; m_NodeIndex < m_CreditsData!.count && process_next_node; ++m_NodeIndex)
				{
					// Get the next node
					CreditsDataNode credits_node = m_CreditsData![m_NodeIndex];

					var was_section_start = false;
					var was_end_section = false;
					var was_title = false;

					// Set the default values, which could be overriden by the flags
					m_CurrentFont = m_DataComponent!.m_FontNormal;
					m_CurrentFontSize = 12;
					m_CurrentColor = ColorScreenDefault;
					m_CurrentColorTitle = ColorTitleDefault;

					// Process all the flags
					foreach(CreditsDataNodeFlag it in credits_node.flags)
					{
						switch(it.flagType)
						{
						// Gap between the nodes
						case CreditsDataNodeFlag.FlagType.GapNodes:
							m_SpaceBetweenNodes = ushort.Parse(it.data,
								CultureInfo.InvariantCulture
							);
							break;
						// Gap between the sections
						case CreditsDataNodeFlag.FlagType.GapSection:
							m_SpaceBetweenSections = ushort.Parse(it.data,
								CultureInfo.InvariantCulture
							);
							break;
						case CreditsDataNodeFlag.FlagType.ScrollSpeed:
							break;
						// Text alignment
						case CreditsDataNodeFlag.FlagType.FontJustify:
							m_CurrentTextAlignment = ushort.Parse(it.data, CultureInfo.InvariantCulture) switch
							{
								0 => HorizontalAlignmentOptions.Left,
								1 => HorizontalAlignmentOptions.Center,
								2 => HorizontalAlignmentOptions.Right,
								_ => m_CurrentTextAlignment
							};
							break;
						// Title font color
						case CreditsDataNodeFlag.FlagType.FontColorTitle:
							m_CurrentColorTitle = ushort.Parse(it.data,
								CultureInfo.InvariantCulture
							);
							break;
						// Active font color
						case CreditsDataNodeFlag.FlagType.FontColorActive:
							m_CurrentColor = ushort.Parse(it.data,
								CultureInfo.InvariantCulture
							);
							break;
						// Title need post-process
						case CreditsDataNodeFlag.FlagType.Title:
							was_title = true;
							break;
						// Section start/end needs post-process
						case CreditsDataNodeFlag.FlagType.SectionStart:
							was_section_start = true;
							break;
						// Section start/end needs post-process
						case CreditsDataNodeFlag.FlagType.SectionEnd:
							was_end_section = true;
							break;
						// Should never happen
						case CreditsDataNodeFlag.FlagType.None:
						default:
							throw new ArgumentOutOfRangeException();
						}
					}

					// Need to post-process some data explicitly
					if(was_section_start)
						m_SpaceForNextNodeRead = m_SpaceBetweenNodes;
					if(was_end_section)
						m_SpaceForNextNodeRead = m_SpaceBetweenSections;
					if(was_title)
					{
						m_CurrentFont = m_DataComponent!.m_FontTitle;
						m_CurrentFontSize = 14;
						m_CurrentColor = m_CurrentColorTitle;
					}

					// If there is a text also
					if(!string.IsNullOrEmpty(credits_node.text))
					{
						// Create and initialize the node
						var node_component = Instantiate(m_TextPrefab,
							m_Canvas!.transform
						)!.GetComponent<CreditsDataNodeComponent>();

						// Position it to the predefined "starting" position
						node_component.Initialize(credits_node.text,
							m_PositionStart!,
							m_CurrentFont!,
							m_CurrentFontSize,
							m_CurrentColor,
							m_CurrentTextAlignment
						);

						m_NodesShown.Enqueue(node_component);

						// Stop processing the next node
						process_next_node = false;

						continue;
					}
				}
			}

			// Process the shown nodes
			foreach(CreditsDataNodeComponent it in m_NodesShown)
			{
				// Scroll the node up with current speed
				it.ScrollNode((float)1000 / m_CurrentScrollSpeed);
			}

			// If the last node is not visible anymore
			if(m_NodesShown.Peek().SpaceDiffY(m_PositionEnd!) > 1)
			{
				// Remove from the visible nodes
				CreditsDataNodeComponent last_node = m_NodesShown.Dequeue();
				// Destroy the GO
				Destroy(last_node.gameObject);
			}
		}
#endregion
	}
}
