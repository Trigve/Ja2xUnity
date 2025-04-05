using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Input action type.
	/// </summary>
	[Flags]
	public enum InputAction
	{
		/// <summary>
		/// Key down.
		/// </summary>
		[HistoricName("KEY_DOWN")]
		KeyDown = 0x0001,

		/// <summary>
		/// Key up.
		/// </summary>
		[HistoricName("KEY_UP")]
		KeyUp = 0x0002,

		/// <summary>
		/// Key repeat.
		/// </summary>
		[HistoricName("KEY_REPEAT")]
		KeyRepeat = 0x0004,

		/// <summary>
		/// Mouse left button down.
		/// </summary>
		[HistoricName("LEFT_BUTTON_DOWN")]
		ButtonLeftDown = 0x0008,

		/// <summary>
		/// Mouse left button up.
		/// </summary>
		[HistoricName("LEFT_BUTTON_UP")]
		ButtonLeftUp = 0x0010,

		/// <summary>
		/// Mouse left button double click.
		/// </summary>
		[HistoricName("LEFT_BUTTON_DBL_CLK")]
		ButtonDoubleClick = 0x0020,

		/// <summary>
		/// Mouse left button repeat.
		/// </summary>
		[HistoricName("LEFT_BUTTON_REPEAT")]
		ButtonLeftRepeat = 0x0040,

		/// <summary>
		/// Mouse right button down.
		/// </summary>
		[HistoricName("RIGHT_BUTTON_DOWN")]
		ButtonRightDown = 0x0080,

		/// <summary>
		/// Mouse right button up.
		/// </summary>
		[HistoricName("RIGHT_BUTTON_UP")]
		ButtonRightUp = 0x0100,

		/// <summary>
		/// Mouse right button repeat.
		/// </summary>
		[HistoricName("RIGHT_BUTTON_REPEAT")]
		ButtonRightRepeat = 0x0200,

		/// <summary>
		/// Mouse position.
		/// </summary>
		[HistoricName("MOUSE_POS")]
		MousePos = 0x0400,

		/// <summary>
		/// Mouse wheel.
		/// </summary>
		[HistoricName("MOUSE_WHEEL")]
		MouseWheel = 0x0800,

		/// <summary>
		/// Mouse wheel up.
		/// </summary>
		[HistoricName("MOUSE_WHEEL_UP")]
		MouseWheelUp = 0x0800,

		/// <summary>
		/// Mouse wheel down.
		/// </summary>
		[HistoricName("MOUSE_WHEEL_DOWN")]
		MouseWheelDown = 0x1000,

		/// <summary>
		/// Mouse middle/wheel button down.
		/// </summary>
		[HistoricName("MIDDLE_BUTTON_DOWN")]
		ButtonMiddleDown = 0x2000,

		/// <summary>
		/// Mouse middle/wheel button up.
		/// </summary>
		[HistoricName("MIDDLE_BUTTON_UP")]
		ButtonMiddleUp = 0x4000,

		/// <summary>
		/// Mouse middle/wheel button repeat.
		/// </summary>
		[HistoricName("MIDDLE_BUTTON_REPEAT")]
		ButtonMiddleRepeat = 0x8000,

		/// <summary>
		/// X1 butto down.
		/// </summary>
		[HistoricName("X1_BUTTON_DOWN")]
		ButtonX1Down = 0x8010,

		/// <summary>
		/// X1 button up.
		/// </summary>
		[HistoricName("X1_BUTTON_UP")]
		ButtonX1Up = 0x8020,

		/// <summary>
		/// X1 button repeat.
		/// </summary>
		[HistoricName("X1_BUTTON_REPEAT")]
		ButtonX1Repeat = 0x8030,

		/// <summary>
		/// X2 button down.
		/// </summary>
		[HistoricName("X2_BUTTON_DOWN")]
		ButtonX2Down = 0x8040,

		/// <summary>
		/// X2 button up.
		/// </summary>
		[HistoricName("X2_BUTTON_UP")]
		ButtonX2Up = 0x8050,

		/// <summary>
		/// X2 button repeat.
		/// </summary>
		[HistoricName("X2_BUTTON_REPEAT")]
		ButtonX2Repeat = 0x8060,
	}

	/// <summary>
	/// Special input keys.
	/// </summary>
	public enum InputKeySpecial
	{
		[HistoricName("BACKSPACE")]
		Backspace = KeyCode.Backspace,

		[HistoricName("TAB")]
		Tab = KeyCode.Tab,

		[HistoricName("ENTER")]
		Enter = KeyCode.Return,

		[HistoricName("PAUSE")]
		Pause = KeyCode.Pause,

		[HistoricName("ESC")]
		Escape = KeyCode.Escape,

		[HistoricName("SPACE")]
		Space = KeyCode.Space,

		[HistoricName("COMMA")]
		Comma = KeyCode.Comma,

		[HistoricName("DEL")]
		Delete = KeyCode.Delete,

		[HistoricName("UPARROW")]
		UpArrow = KeyCode.UpArrow,

		[HistoricName("DNARROW")]
		DownArrow = KeyCode.DownArrow,

		[HistoricName("RIGHTARROW")]
		RightArrow = KeyCode.RightArrow,

		[HistoricName("LEFTARROW")]
		LeftArrow = KeyCode.LeftArrow,

		[HistoricName("INSERT")]
		Insert = KeyCode.Insert,

		[HistoricName("HOME")]
		Home = KeyCode.Home,

		[HistoricName("END")]
		End = KeyCode.End,

		[HistoricName("PGUP")]
		PageUp = KeyCode.PageUp,

		[HistoricName("PGDN")]
		PageDown = KeyCode.PageDown,

		F1 = KeyCode.F1,

		F2 = KeyCode.F2,

		F3 = KeyCode.F3,

		F4 = KeyCode.F4,

		F5 = KeyCode.F5,

		F6 = KeyCode.F6,

		F7 = KeyCode.F7,

		F8 = KeyCode.F8,

		F9 = KeyCode.F9,

		F10 = KeyCode.F10,

		F11 = KeyCode.F11,

		F12 = KeyCode.F12,

		[HistoricName("NUM_LOCK")]
		NumLock = KeyCode.Numlock,

		[HistoricName("CAPS")]
		CapsLock = KeyCode.CapsLock,

		[HistoricName("SCRL_LOCK")]
		ScrollLock = KeyCode.ScrollLock,

		[HistoricName("SHIFT")]
		ShiftR = KeyCode.RightShift,

		[HistoricName("SHIFT")]
		ShiftL = KeyCode.LeftShift,

		[HistoricName("CTRL")]
		ControlR = KeyCode.RightControl,

		[HistoricName("CTRL")]
		ControlL = KeyCode.LeftControl,

		[HistoricName("ALT")]
		AltR = KeyCode.RightAlt,

		[HistoricName("ALT")]
		AltL = KeyCode.LeftAlt,

		[HistoricName("SNAPSHOT")]
		PrintScreen = KeyCode.Print,
//x		FULLSTOP = 190,
	}

	/// <summary>
	/// Key state.
	/// </summary>
	[Flags]
	public enum InputKeyState
	{
		None = 0,
		/// "CONTROL" was pressed down.
		Control = 1,
		/// "ALT" was pressed down.
		Alt = 1 << 1,
		/// "SHIFT" was pressed down.
		Shift = 1 << 2,
	}
	/// <summary>
	/// Input event definition.
	/// </summary>
	public readonly struct InputAtom
	{
#region Properties
		/// <summary>
		/// Event time stmap.
		/// </summary>
		[HistoricName("uiTimeStamp")]
		public double timeStamp { get; }

		/// <summary>
		/// Key state.
		/// </summary>
		[HistoricName("usKeyState")]
		public InputKeyState keyState { get; }

		/// <summary>
		/// Event type.
		/// </summary>
		[HistoricName("usEvent")]
		public InputAction eventType { get; }

		/// <summary>
		/// Custom parameter 1.
		/// </summary>
		[HistoricName("usParam")]
		public object param1 { get; }

		/// <summary>
		/// Custom parameter 2.
		/// </summary>
		[HistoricName("uiParam")]
		public object param2 { get; }
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="TimeStamp">See <see cref="timeStamp"/></param>
		/// <param name="KeyState"></param>
		/// <param name="EventType"></param>
		/// <param name="Param1"></param>
		/// <param name="Param2"></param>
		public InputAtom(double TimeStamp, InputKeyState KeyState, InputAction EventType, object Param1, object Param2)
		{
			timeStamp = TimeStamp;
			keyState = KeyState;
			eventType = EventType;
			param1 = Param1;
			param2 = Param2;
		}
#endregion
	}
}
