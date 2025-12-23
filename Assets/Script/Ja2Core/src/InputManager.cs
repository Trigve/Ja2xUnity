using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Input manager.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Input Manager", fileName = "InputManager")]
	public sealed class InputManager : ScriptableObjectManager<InputManager>
	{
#region Constants
		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("BUTTON_REPEAT_TIMEOUT")]
		private const double ButtonRepeatTimeout = 0.250;

		/// <summary>
		/// Time between two succesive clicks to be interpreted as double click.
		/// </summary>
		[HistoricName("DBL_CLK_TIME")]
		private const double DoubleClickTime = 0.300;

		/// <summary>
		/// Key codes for keyboard.
		/// </summary>
		private static readonly KeyCode[] KeyCodes = Enum.GetValues(typeof(KeyCode))
			.Cast<KeyCode>()
			// Ignore mouse, joystick
			.Where(Key => (int)Key < (int)KeyCode.Mouse0)
			.ToArray()
		;

		/// <summary>
		/// Queue max capacity.
		/// </summary>
		private const int QueueCapacity = 256;
#endregion

#region Fields
		/// <summary>
		/// Table is used to track which of the keys is up or down at any one time. This is used while polling the interface. TRUE = Pressed, FALSE = Not Pressed.
		/// </summary>
		[HistoricName("gfKeyState")]
		private readonly bool[] m_KeyState = new bool[Enum.GetValues(typeof(KeyCode)).Cast<int>().Max()];

		/// <summary>
		/// Event queue.
		/// </summary>
		[HistoricName("gEventQueue")]
		private readonly Queue<InputAtom> m_EventQueue = new Queue<InputAtom>(QueueCapacity);

		/// <summary>
		/// Was input received in the current frame.
		/// </summary>
		[HistoricName("gfSGPInputReceived")]
		private bool m_InputReceived;

		/// <summary>
		/// Is there some UI control which is capturing the input.
		/// </summary>
		[HistoricName("gfCurrentStringInputState")]
		private bool m_IsCurrentStringInputState;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiLeftButtonRepeatTimer")]
		private double m_ButtonLeftRepeatTimer;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiRightButtonRepeatTimer")]
		private double m_ButtonRightRepeatTimer;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiMiddleButtonRepeatTimer")]
		private double m_ButtonMiddleRepeatTimer;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiX1ButtonRepeatTimer")]
		private double m_ButtonX1RepeatTimer;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiX2ButtonRepeatTimer")]
		private double m_ButtonX2RepeatTimer;

		/// <summary>
		/// ???
		/// </summary>
		[HistoricName("guiSingleClickTimer")]
		private double m_SingleClickTimer;
#endregion

#region Properties
		/// <summary>
		/// Current mouse position.
		/// </summary>
		public Vector3 mousePosition { get; private set; }

		/// <summary>
		/// Left mouse button pressed during frame.
		/// </summary>
		[HistoricName("_LeftButtonDown")]
		[HistoricName("gfLeftButtonState")]
		public bool isMouseButtonLeftDown { get; private set; }

		/// <summary>
		/// Right mouse button pressed during frame.
		/// </summary>
		[HistoricName("_RightButtonDown")]
		[HistoricName("gfRightButtonState")]
		public bool isMouseButtonRightDown { get; private set; }

		/// <summary>
		/// Is SHIFT pressed in the current frame.
		/// </summary>
		[HistoricName("gfShiftState")]
		public bool isShiftPressed { get; private set; }

		/// <summary>
		/// Is ALT pressed in the current frame.
		/// </summary>
		[HistoricName("gfAltState")]
		public bool isAltPressed { get; private set; }

		/// <summary>
		/// Is CTRL pressed in the current frame.
		/// </summary>
		[HistoricName("gfCtrlState")]
		public bool isCtrlPressed { get; private set; }
#endregion

#region Methods
		/// <summary>
		/// Update the input states.
		/// </summary>
		public void Update()
		{
			m_InputReceived = Input.anyKeyDown;

			// Mouse position
			mousePosition = Input.mousePosition;

			// Check for keyboard
			foreach(KeyCode key_code in KeyCodes)
			{
				if(Input.GetKeyDown(key_code))
					KeyDown(key_code);
				else if(Input.GetKeyUp(key_code))
					KeyUp(key_code);
			}

			// Mouse stuff
			isMouseButtonLeftDown = Input.GetMouseButtonDown(0);
			isMouseButtonRightDown = Input.GetMouseButtonDown(1);
		}

		/// <summary>
		/// Deque the event, if event mask match the one in the front of the queue.
		/// </summary>
		/// <param name="Event"></param>
		/// <param name="uiMaskFlags"></param>
		/// <returns></returns>
		public bool DequeueSpecificEvent(InputAction uiMaskFlags, out InputAtom? Event)
		{
			var result = false;
			Event = null;

			// Is there an event to dequeue
			if(m_EventQueue.Count > 0)
			{
				InputAtom event_peek = m_EventQueue.Peek();

				// Check if it has the masks
				if(event_peek.eventType.HasFlag(uiMaskFlags))
					result = DequeueEvent(out Event);
			}

			return result;
		}

		/// <summary>
		/// Deque the event from the queue.
		/// </summary>
		/// <param name="Event"></param>
		/// <returns></returns>
		[HistoricName("InternalDequeueEvent")]
		public bool DequeueEvent(out InputAtom? Event)
		{
			var ret = false;
			Event = null;

			// Is there an event to dequeue
			if(m_EventQueue.Count > 0)
			{
				// We have an event, so we dequeue it
				Event = m_EventQueue.Dequeue();

				ret = true;
			}

			return ret;
		}

		/// <summary>
		/// Key down implementation.
		/// </summary>
		private void KeyDown(KeyCode Key)
		{
			// SHIFT key is PRESSED
			if(Key is KeyCode.LeftShift or KeyCode.RightShift)
			{
				isShiftPressed = true;
				m_KeyState[(int)Key] = true;
			}
			else
			{
				// CTRL key is PRESSED
				if(Key is KeyCode.LeftControl or KeyCode.RightControl)
				{
					isCtrlPressed = true;
					m_KeyState[(int)Key] = true;
				}
				else
				{
					// ALT key is pressed
					if(Key is KeyCode.LeftAlt or KeyCode.RightAlt)
					{
						isAltPressed = true;
						m_KeyState[(int)Key] = true;
					}
					else
					{
						if(Key == KeyCode.Print)
						{
							//PrintScreen();
							// DB Done in the KeyUp function
							// this used to be keyed to SCRL_LOCK
							// which I believe Luis gave the wrong value
						}
						else
						{
							// No special keys have been pressed
							// Call KeyChange() and pass TRUE to indicate key has been PRESSED and not RELEASED
							KeyChange(Key,
								true
							);
						}
					}
				}
			}
		}

		/// <summary>
		/// Key up implementation.
		/// </summary>
		private void KeyUp(KeyCode Key)
		{
			// Are we RELEASING one of SHIFT, ALT or CTRL ???
			if(Key is KeyCode.LeftShift or KeyCode.RightShift)
			{
				// SHIFT key is RELEASED
				isShiftPressed = false;
				m_KeyState[(int)Key] = false;
			}
			else
			{
				// CTRL key is RELEASED
				if(Key is KeyCode.LeftControl or KeyCode.RightControl)
				{
					isCtrlPressed = false;
					m_KeyState[(int)Key] = false;
				}
				else
				{
					// ALT key is RELEASED
					if(Key is KeyCode.LeftAlt or KeyCode.RightAlt)
					{
						isAltPressed = false;
						m_KeyState[(int)Key] = false;
					}
					else
					{
						if(Key == KeyCode.Print)
						{
						}
						else
						{
							// No special keys have been pressed
							// Call KeyChange() and pass FALSE to indicate key has been PRESSED and not RELEASED
							KeyChange(Key,
								false
							);
						}
					}
				}
			}
		}

		/// <summary>
		/// Handle the key change.
		/// </summary>
		/// <param name="Key">Key enum.</param>
		/// <param name="IsPressed">True, if the key was pressed in the current frame. Otherwise, it was released.</param>
		private void KeyChange(KeyCode Key, bool IsPressed)
		{
			ushort ubChar;

			Vector3 uiTmpLParam = Input.mousePosition;

			// Key has been PRESSED
			if(IsPressed)
			{
				// If the key is currently not pressed
				if(!m_KeyState[(int)Key])
				{
					// Well the key has just been pressed, therefore we queue up and event and update the gsKeyState
					// There is no string input going on right now, so we queue up the event
					if(!m_IsCurrentStringInputState)
					{
						m_KeyState[(int)Key] = true;

						QueueEvent(InputAction.KeyDown,
							Key,
							uiTmpLParam
						);
					}
					// There is a current input string which will capture this event
					else
					{
					}
				}
				else
				{
					// Well the key gets repeated
					if(!m_IsCurrentStringInputState)
					{
						// There is no string input going on right now, so we queue up the event
						QueueEvent(InputAction.KeyRepeat,
							Key,
							uiTmpLParam
						);
					}
					else
					{
					}
				}
			}
			// Key has been RELEASED
			else
			{
				// Find out if the key is already pressed and if so, queue an event and update the gfKeyState array
				if(m_KeyState[(int)Key])
				{
					// Well the key has just been pressed, therefore we queue up and event and update the gsKeyState
					m_KeyState[(int)Key] = false;

					QueueEvent(InputAction.KeyUp,
						Key,
						uiTmpLParam
					);
				}
				//else if the alt tab key was pressed
//x				else if(ubChar == TAB && gfAltState)
//x				{
//x					// therefore minimize the application
//x					ShowWindow(ghWindow, SW_MINIMIZE);
//x					m_KeyState[ALT] = FALSE;
//x					gfAltState = FALSE;
//x				}
			}
		}

		/// <summary>
		/// Queue the input event.
		/// </summary>
		/// <param name="InputEvent"></param>
		/// <param name="Param1"></param>
		/// <param name="Param2"></param>
		private void QueueEvent(InputAction InputEvent, object Param1, object Param2)
		{
			InternalQueueEvent(InputEvent,
				Param1,
				Param2
			);
		}

		/// <summary>
		/// Implementation of the event queueing.
		/// </summary>
		/// <param name="InputEvent"></param>
		/// <param name="Param1"></param>
		/// <param name="Param2"></param>
		private void InternalQueueEvent(InputAction InputEvent, object Param1, object Param2)
		{
			// Limit reached, no more queue space
			if(m_EventQueue.Count >= QueueCapacity)
				return;

			double uiTimer = Time.realtimeSinceStartupAsDouble;
			InputKeyState usKeyState = (isShiftPressed ? InputKeyState.Shift : InputKeyState.None) | (isCtrlPressed ? InputKeyState.Control : InputKeyState.None)  | (isAltPressed ? InputKeyState.Alt : InputKeyState.None);

			if(InputEvent == InputAction.ButtonLeftDown)
				m_ButtonLeftRepeatTimer = uiTimer + ButtonRepeatTimeout;

			if(InputEvent == InputAction.ButtonRightDown)
				m_ButtonRightRepeatTimer = uiTimer + ButtonRepeatTimeout;
			if(InputEvent == InputAction.ButtonMiddleDown)
				m_ButtonMiddleRepeatTimer = uiTimer + ButtonRepeatTimeout;
			if(InputEvent == InputAction.ButtonX1Down)
				m_ButtonX1RepeatTimer = uiTimer + ButtonRepeatTimeout;
			if(InputEvent == InputAction.ButtonX2Down)
				m_ButtonX2RepeatTimer = uiTimer + ButtonRepeatTimeout;

			if(InputEvent == InputAction.ButtonLeftUp)
				m_ButtonLeftRepeatTimer = 0;
			if(InputEvent == InputAction.ButtonRightUp)
				m_ButtonRightRepeatTimer = 0;
			if(InputEvent == InputAction.ButtonMiddleUp)
				m_ButtonMiddleRepeatTimer = 0;
			if(InputEvent == InputAction.ButtonX1Up)
				m_ButtonX1RepeatTimer = 0;
			if(InputEvent == InputAction.ButtonX2Up)
				m_ButtonX2RepeatTimer = 0;


			if(InputEvent == InputAction.ButtonLeftUp)
			{
				// Do we have a double click
				if((uiTimer - m_SingleClickTimer) < DoubleClickTime)
				{
					m_SingleClickTimer = 0;

					// Add a button up first
					m_EventQueue.Enqueue(
						new InputAtom(uiTimer,
							0,
							InputAction.ButtonLeftUp,
							Param1,
							Param2
						)
					);

					// Now do double-click
					m_EventQueue.Enqueue(
						new InputAtom(uiTimer,
							0,
							InputAction.ButtonDoubleClick,
							Param1,
							Param2
						)
					);

					return;
				}

				// Save time
				m_SingleClickTimer = uiTimer;
			}

			// We can queue up the event, so we do it
			m_EventQueue.Enqueue(
				new InputAtom(uiTimer,
					usKeyState,
					InputEvent,
					Param1,
					Param2
				)
			);
		}
#endregion

#region Construction
		/// <inheritdoc />
		protected override void DoInitialize(params object[] Params)
		{
			Ja2Logger.LogInfo("Initializing Input Manager ...");

			// Initialize the Event Queue
			m_EventQueue.Clear();


			// Initialize other variables
			isShiftPressed = false;
			isAltPressed = false;
			isCtrlPressed = false;

			// Initialize variables pertaining to DOUBLE CLIK stuff

			// Initialize variables pertaining to the button states
			isMouseButtonLeftDown = false;
			isMouseButtonRightDown = false;

			m_ButtonLeftRepeatTimer = 0;
			m_ButtonRightRepeatTimer = 0;


			// Initialize the string input mechanism
			m_IsCurrentStringInputState = false;
		}
#endregion
	}
}
