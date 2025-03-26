using System;

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
}
