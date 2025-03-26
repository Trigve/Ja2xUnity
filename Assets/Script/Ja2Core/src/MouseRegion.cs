using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Mouse region.
	/// </summary>
	public class MouseRegion
	{
#region Constants
		/// <summary>
		/// Lowest priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_LOWEST")]
		public const short PriorityLowest = 0;

		/// <summary>
		/// Low priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_LOW")]
		public const short PriorityLow = 15;

		/// <summary>
		/// Base priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_BASE")]
		public const short PriorityBase = 31;

		/// <summary>
		/// Normal priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_NORMAL")]
		public const short PriorityNormal = 31;

		/// <summary>
		/// High priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_HIGH")]
		public const short PriorityHigh = 63;

		/// <summary>
		/// Highest priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_HIGHEST")]
		public const short PriorityHighest = 127;

		/// <summary>
		/// System priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_SYSTEM")]
		public const short PrioritySystem = -1;

		/// <summary>
		/// Auto priority.
		/// </summary>
		[HistoricName("MSYS_PRIORITY_AUTO")]
		public const short PriorityAuto = -1;
#endregion

#region Enums
		/// <summary>
		/// Mouse region flags.
		/// </summary>
		[Flags]
		public enum RegionFlag
		{
			/// <summary>
			/// No flags.
			/// </summary>
			[HistoricName("MSYS_NO_FLAGS")]
			NoFlags = 0x00000000,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_MOUSE_IN_AREA")]
			MouseInArea = 0x00000001,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_SET_CURSOR")]
			SetCursor = 0x00000002,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_MOVE_CALLBACK")]
			MoveCallback = 0x00000004,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_BUTTON_CALLBACK")]
			ButtonCallback = 0x00000008,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_REGION_EXISTS")]
			RegionExists = 0x00000010,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_SYSTEM_INIT")]
			SystemInit = 0x00000020,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_REGION_ENABLED")]
			RegionEnabled = 0x00000040,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_FASTHELP")]
			FastHelp = 0x00000080,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_GOT_BACKGROUND")]
			GotBackground = 0x00000100,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_HAS_BACKRECT")]
			HasBracket = 0x00000200,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_FASTHELP_RESET")]
			FastHelpReset = 0x00000400,

			/// <summary>
			/// ???.
			/// </summary>
			[HistoricName("MSYS_ALLOW_DISABLED_FASTHELP")]
			FastHelpDisabled = 0x00000800,

			/// <summary>
			/// Base region flags.
			/// </summary>
			[HistoricName("BASE_REGION_FLAGS")]
			BaseRegion = (RegionEnabled | SetCursor)
		}
#endregion

#region Properties
		/// <summary>
		/// Region's ID number, set by mouse system.
		/// </summary>
		public ushort idNumber { get; }

		/// <summary>
		/// Region's Priority, set by system and/or caller.
		/// </summary>
		public short priorityLevel { get; }

		/// <summary>
		/// Region's state flags.
		/// </summary>
		public RegionFlag uiFlags { get; }

		/// <summary>
		/// Screen area affected by this region (screen coordinates).
		/// </summary>
		public RectInt region { get; }

		/// <summary>
		/// Mouse's Coordinates in absolute screen coordinates
		/// </summary>
		public Vector2Int mousePos { get; }

		/// <summary>
		/// Mouse's Coordinates relative to the Top-Left corner of the region.
		/// </summary>
		public Vector2Int relativeXPos { get; }

		/// <summary>
		/// Current state of the mouse buttons
		/// </summary>
		public ushort buttonState { get; }

		/// <summary>
		/// // Wheel state +/- number of wheel units.
		/// </summary>
		public ushort wheelState { get; }

		/// <summary>
		/// Cursor to use when mouse in this region (see flags).
		/// </summary>
		public ushort cursor { get; }

		/// <summary>
		/// User Data, can be set to anything.
		/// </summary>
		public int[] userData { get; }
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		public MouseRegion(ushort IdNumber, short PriorityLevel, RegionFlag UiFlags, RectInt Region, Vector2Int MousePos, Vector2Int RelativeXPos, ushort ButtonState, ushort WheelState, ushort Cursor, int[] UserData)
		{
			idNumber = IdNumber;
			priorityLevel = PriorityLevel;
			uiFlags = UiFlags;
			region = Region;
			mousePos = MousePos;
			relativeXPos = RelativeXPos;
			buttonState = ButtonState;
			wheelState = WheelState;
			cursor = Cursor;
			userData = UserData;
		}
#endregion
	}
}
