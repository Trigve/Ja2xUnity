using System;
using System.Collections.Generic;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Mouse system manager.
	/// </summary>
	public class MouseSystemManager
	{
#region Constants
		/// <summary>
		/// Base ID.
		/// </summary>
		[HistoricName("MSYS_ID_BASE")]
		private const int IdBase = 1;

		/// <summary>
		/// Maimum region ID.
		/// </summary>
		[HistoricName("MSYS_ID_MAX")]
		private const int IdMax = 0xfffffff;

		/// <summary>
		/// System region ID.
		/// </summary>
		[HistoricName("MSYS_ID_SYSTEM")]
		private const int IdSystem = 0;
#endregion

#region Enums
		/// <summary>
		/// Mouse buttons enums.
		/// </summary>
		[Flags]
		public enum MouseButton
		{
			/// <summary>
			/// Default value.
			/// </summary>
			NoButton = 0,

			/// <summary>
			/// Left button.
			/// </summary>
			[HistoricName("MSYS_LEFT_BUTTON")]
			ButtonLeft = 1,

			/// <summary>
			/// Right button.
			/// </summary>
			[HistoricName("MSYS_RIGHT_BUTTON")]
			ButtonRight = 2,

			/// <summary>
			/// Middle button.
			/// </summary>
			[HistoricName("MSYS_MIDDLE_BUTTON")]
			ButtonMiddle = 4,

			/// <summary>
			/// X1 button.
			/// </summary>
			[HistoricName("MSYS_X1_BUTTON")]
			ButtonX1 = 8,

			/// <summary>
			/// X2 button.
			/// </summary>
			[HistoricName("MSYS_X2_BUTTON")]
			ButtonX2 = 16,
		}

		/// <summary>
		/// Mouse action enums.
		/// </summary>
		[Flags]
		public enum MouseAction
		{
			/// <summary>
			/// Default.
			/// </summary>
			[HistoricName("MSYS_NO_ACTION")]
			NoAction = 0,

			/// <summary>
			/// Mouse move.
			/// </summary>
			[HistoricName("MSYS_DO_MOVE")]
			MouseMove = 1,

			/// <summary>
			/// Mouse left button down.
			/// </summary>
			[HistoricName("MSYS_DO_LBUTTON_DWN")]
			MouseButtonLDown = 2,

			/// <summary>
			/// Mouse left button up.
			/// </summary>
			[HistoricName("MSYS_DO_LBUTTON_UP")]
			MouseButtonLUp = 4,

			/// <summary>
			/// Mouse right button down.
			/// </summary>
			[HistoricName("MSYS_DO_RBUTTON_DWN")]
			MouseButtonRDown = 8,

			/// <summary>
			/// Mouse right button up.
			/// </summary>
			[HistoricName("MSYS_DO_RBUTTON_UP")]
			MouseButtonRUp = 16,

			/// <summary>
			/// Mouse left button repeat.
			/// </summary>
			[HistoricName("MSYS_DO_LBUTTON_REPEAT")]
			MouseButtonLRepeat = 32,

			/// <summary>
			/// Mouse right button repeat.
			/// </summary>
			[HistoricName("MSYS_DO_RBUTTON_REPEAT")]
			MouseButtonRRepeat = 64,

			/// <summary>
			/// Mouse middle button down.
			/// </summary>
			[HistoricName("MSYS_DO_MBUTTON_DWN")]
			MouseButtonMDown = 128,

			/// <summary>
			/// Mouse middle button up.
			/// </summary>
			[HistoricName("MSYS_DO_MBUTTON_UP")]
			MouseButtonMUp = 256,

			/// <summary>
			/// Mouse middle button repeat.
			/// </summary>
			[HistoricName("MSYS_DO_MBUTTON_REPEAT")]
			MouseButtonMRepeat = 512,

			/// <summary>
			/// Mouse X1 button down.
			/// </summary>
			[HistoricName("MSYS_DO_X1BUTTON_DWN")]
			MouseButtonX1Down = 1024,

			/// <summary>
			/// Mouse X1 button up.
			/// </summary>
			[HistoricName("MSYS_DO_X1BUTTON_UP")]
			MouseButtonX1Up = 2048,

			/// <summary>
			/// Mouse X1 button repeat.
			/// </summary>
			[HistoricName("MSYS_DO_X1BUTTON_REPEAT")]
			MouseButtonX1Repeat = 4096,

			/// <summary>
			/// Mouse X2 button down.
			/// </summary>
			[HistoricName("MSYS_DO_X2BUTTON_DWN")]
			MouseButtonX2Down = 8192,

			/// <summary>
			/// Mouse X2 button up.
			/// </summary>
			[HistoricName("MSYS_DO_X2BUTTON_UP")]
			MouseButtonX2Up = 16384,

			/// <summary>
			/// Mouse X2 button repeat.
			/// </summary>
			[HistoricName("MSYS_DO_X2BUTTON_REPEAT")]
			MouseButtonx2Repeat = 32768,

			/// <summary>
			/// Mouse wheel up.
			/// </summary>
			[HistoricName("MSYS_DO_WHEEL_UP")]
			MouseButtonWheelUp = 65536,

			/// <summary>
			/// Mouse wheel down.
			/// </summary>
			[HistoricName("MSYS_DO_WHEEL_DOWN")]
			MouseButtonWheelDown = 131072,

			/// <summary>
			/// Mouse all buttons actions.
			/// </summary>
			[HistoricName("MSYS_DO_BUTTONS")]
			MouseAllButtons = (MouseButtonLDown | MouseButtonLUp | MouseButtonLRepeat | MouseButtonRDown | MouseButtonRUp | MouseButtonRRepeat | MouseButtonMDown | MouseButtonMUp | MouseButtonMRepeat | MouseButtonX1Down | MouseButtonX1Up | MouseButtonX1Repeat | MouseButtonX2Down | MouseButtonX2Up | MouseButtonx2Repeat),

			/// <summary>
			/// Mouse all wheel actions.
			/// </summary>
			[HistoricName("MSYS_DO_WHEEL")]
			MouseAllWheels = (MouseButtonWheelUp | MouseButtonWheelDown)
		}
#endregion

#region Fields
		/// <summary>
		/// All the mouse regions.
		/// </summary>
		[HistoricName("MSYS_RegList")]
		private List<MouseRegion> m_MouseRegions = new List<MouseRegion>();

		/// <summary>
		/// Current region ID.
		/// </summary>
		[HistoricName("MSYS_CurrentID")]
		private int m_CurrentId;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("MSYS_ScanForID")]
		private bool m_ScanForId;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("MSYS_CurrentMX")]
		private ushort m_CurrentMx;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("MSYS_CurrentMY")]
		private ushort m_CurrentMy;

		/// <summary>
		/// Current buttons status.
		/// </summary>
		[HistoricName("MSYS_CurrentButtons")]
		private MouseButton m_CurrentButtons;

		/// <summary>
		/// Mouse wheel status.
		/// </summary>
		[HistoricName("MSYS_Wheel")]
		private int m_wheelStatus;

		/// <summary>
		/// Current action.
		/// </summary>
		[HistoricName("MSYS_Action")]
		private MouseAction m_Action;

		/// <summary>
		/// Could use mouse handler hook.
		/// </summary>
		[HistoricName("MSYS_UseMouseHandlerHook")]
		private bool m_UseMouseHandlerHook;

		/// <summary>
		/// Is mouse grabbed.
		/// </summary>
		[HistoricName("MSYS_Mouse_Grabbed")]
		private bool m_IsMouseGrabbed;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("MSYS_GrabRegion")]
		private MouseRegion? m_GrabRegion;

		/// <summary>
		/// Background region.
		/// </summary>
		[HistoricName("MSYS_SystemBaseRegion")]
		private MouseRegion m_SystemBaseRegion;

		/// <summary>
		/// Flag used if some mouse region becomes dirty.
		/// </summary>
		[HistoricName("gfRefreshUpdate")]
		private bool m_RefreshUpdate;
#endregion

#region Methods
		/// <summary>
		/// Initialize the manager.
		/// </summary>
		public void Init()
		{
			m_MouseRegions.Clear();

			m_CurrentId = IdSystem;
			m_ScanForId = false;

			m_CurrentMx = 0;
			m_CurrentMy = 0;
			m_CurrentButtons = MouseButton.NoButton;
			m_wheelStatus = 0;
			m_Action = MouseAction.NoAction;
			m_UseMouseHandlerHook = true;

			m_IsMouseGrabbed = false;
			m_GrabRegion = null;

			// Setup the system's background region
			m_SystemBaseRegion = new MouseRegion(IdSystem,
				MouseRegion.PrioritySystem,
				MouseRegion.RegionFlag.BaseRegion,
				new RectInt(-32767,
					-32767,
					32767 * 2,
					32767 * 2
				),
				new Vector2Int(),
				new Vector2Int(),
				0,
				0,
				0,
				Array.Empty<int>()
			);

			// Add the base region to the list
			m_MouseRegions.Add(m_SystemBaseRegion);
		}

		/// <summary>
		/// Hook to the SGP's mouse handler.
		/// </summary>
		/// <param name="Type"></param>
		/// <param name="Xcoord"></param>
		/// <param name="Ycoord"></param>
		/// <param name="LeftButton"></param>
		/// <param name="RightButton"></param>
		[HistoricName("MSYS_SGP_Mouse_Handler_Hook")]
		public void MouseHandlerHook(InputAction Type, ushort Xcoord, ushort Ycoord, bool LeftButton, bool RightButton)
		{
			// If we're not using the handler stuff, ignore this call
			if(!m_UseMouseHandlerHook)
				return;

			m_Action = MouseAction.NoAction;
			switch(Type)
			{
				case InputAction.ButtonLeftDown:
				case InputAction.ButtonLeftUp:
				case InputAction.ButtonRightDown:
				case InputAction.ButtonRightUp:
				case InputAction.ButtonMiddleUp:
				case InputAction.ButtonMiddleDown:
				case InputAction.ButtonX1Down:
				case InputAction.ButtonX1Up:
				case InputAction.ButtonX2Down:
				case InputAction.ButtonX2Up:

					if(Type == InputAction.ButtonLeftDown)
					{
						m_Action |= MouseAction.MouseButtonLDown;
						m_CurrentButtons |= MouseButton.ButtonLeft;
					}
					else if(Type == InputAction.ButtonLeftUp)
					{
						m_Action |= MouseAction.MouseButtonLUp;
						m_CurrentButtons &= (~MouseButton.ButtonLeft);
					}
					else if(Type == InputAction.ButtonRightDown)
					{
						m_Action |= MouseAction.MouseButtonRDown;
						m_CurrentButtons |= MouseButton.ButtonRight;
					}
					else if(Type == InputAction.ButtonRightUp)
					{
						m_Action |= MouseAction.MouseButtonRUp;
						m_CurrentButtons &= ~MouseButton.ButtonRight;
					}
					else if(Type == InputAction.ButtonMiddleDown)
					{
						m_Action |= MouseAction.MouseButtonMDown;
						m_CurrentButtons |= MouseButton.ButtonMiddle;
					}
					else if(Type == InputAction.ButtonMiddleUp)
					{
						m_Action |= MouseAction.MouseButtonMUp;
						m_CurrentButtons &= ~MouseButton.ButtonMiddle;
					}
					else if(Type == InputAction.ButtonX1Down)
					{
						m_Action |= MouseAction.MouseButtonX1Down;
						m_CurrentButtons |= MouseButton.ButtonX1;
					}
					else if(Type == InputAction.ButtonX1Up)
					{
						m_Action |= MouseAction.MouseButtonX1Up;
						m_CurrentButtons &= ~MouseButton.ButtonX1;
					}
					else if(Type == InputAction.ButtonX2Down)
					{
						m_Action |= MouseAction.MouseButtonX2Down;
						m_CurrentButtons |= MouseButton.ButtonX2;
					}
					else if(Type == InputAction.ButtonX2Up)
					{
						m_Action |= MouseAction.MouseButtonX2Up;
						m_CurrentButtons &= ~MouseButton.ButtonX2;
					}

					if(LeftButton)
						m_CurrentButtons |= MouseButton.ButtonLeft;
					else
						m_CurrentButtons &= ~MouseButton.ButtonLeft;

					if(RightButton)
						m_CurrentButtons |= MouseButton.ButtonRight;
					else
						m_CurrentButtons &= ~MouseButton.ButtonRight;

					if((Xcoord != m_CurrentMx) || (Ycoord != m_CurrentMy))
					{
						m_Action |= MouseAction.MouseMove;
						m_CurrentMx = Xcoord;
						m_CurrentMy = Ycoord;
					}

					break;

				// ATE: Checks here for mouse button repeats.....
				// Call mouse region with new reason
				case InputAction.ButtonLeftRepeat:
				case InputAction.ButtonRightRepeat:
				case InputAction.ButtonMiddleRepeat:
				case InputAction.ButtonX1Repeat:
				case InputAction.ButtonX2Repeat:
					if(Type == InputAction.ButtonLeftRepeat)
						m_Action |= MouseAction.MouseButtonLRepeat;
					else if(Type == InputAction.ButtonRightRepeat)
						m_Action |= MouseAction.MouseButtonRRepeat;
					else if(Type == InputAction.ButtonMiddleRepeat)
						m_Action |= MouseAction.MouseButtonMRepeat;
					else if(Type == InputAction.ButtonX1Repeat)
						m_Action |= MouseAction.MouseButtonX1Repeat;
					else if(Type == InputAction.ButtonX2Repeat)
						m_Action |= MouseAction.MouseButtonx2Repeat;

					if((Xcoord != m_CurrentMx) || (Ycoord != m_CurrentMy))
					{
						m_Action |= MouseAction.MouseMove;
						m_CurrentMx = Xcoord;
						m_CurrentMy = Ycoord;
					}
					break;
				case InputAction.MouseWheelUp:
				case InputAction.MouseWheelDown:
					if(Type == InputAction.MouseWheelUp)
					{
						m_Action |= MouseAction.MouseButtonWheelUp;
						m_wheelStatus++;
					}
					else if(Type == InputAction.MouseWheelDown)
					{
						m_Action |= MouseAction.MouseButtonWheelDown;
						m_wheelStatus--;
					}
					break;
				case InputAction.MousePos:
					if((Xcoord != m_CurrentMx) || (Ycoord != m_CurrentMy) || m_RefreshUpdate)
					{
						m_Action |= MouseAction.MouseMove;
						m_CurrentMx = Xcoord;
						m_CurrentMy = Ycoord;

						m_RefreshUpdate = false;

					}
					break;
				default:
					Ja2Logger.LogWarning("MSYS 2 SGP Mouse Hook got bad type.");
					break;
			}
			// check for moved mouse
			if((Xcoord != m_CurrentMx) || (Ycoord != m_CurrentMy) || m_RefreshUpdate)
			{
				m_Action |= MouseAction.MouseMove;
				m_CurrentMx = Xcoord;
				m_CurrentMy = Ycoord;
			}

			// update if something happened
			if(m_Action != MouseAction.NoAction)
			{
			}
		}
#endregion
	}
}
