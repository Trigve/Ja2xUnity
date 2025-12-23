using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main entry class.
	/// </summary>
	public sealed class GameManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state singleton.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;
#endregion

#region Messages
		/// See unity.
		public void Update()
		{
			Vector3 MousePos = m_GameState.inputManager.mousePosition;

			m_GameState.inputManager.Update();



			// Hook into mouse stuff for MOVEMENT MESSAGES
			m_GameState.mouseSystemManager.MouseHandlerHook(InputAction.MousePos,
				(ushort)MousePos.x,
				(ushort)MousePos.y,
				m_GameState.inputManager.isMouseButtonLeftDown,
				m_GameState.inputManager.isMouseButtonRightDown
			);


			while(
				m_GameState.inputManager.DequeueSpecificEvent(InputAction.ButtonLeftRepeat | InputAction.ButtonRightRepeat | InputAction.ButtonLeftDown | InputAction.ButtonLeftUp | InputAction.ButtonMiddleUp | InputAction.ButtonX1Up | InputAction.ButtonX2Up | InputAction.ButtonRightDown | InputAction.ButtonRightUp | InputAction.ButtonMiddleDown | InputAction.ButtonX1Down | InputAction.ButtonX2Down | InputAction.MouseWheelUp | InputAction.MouseWheelDown,
						out var input_event
				)
			)
			{
				// HOOK INTO MOUSE HOOKS
				m_GameState.mouseSystemManager.MouseHandlerHook(input_event!.Value.eventType,
					(ushort)MousePos.x,
					(ushort)MousePos.y,
					m_GameState.inputManager.isMouseButtonLeftDown,
					m_GameState.inputManager.isMouseButtonRightDown
				);
			}

			{

			}

			{
				{


					{
					}













			}







		}
#endregion
	}
}
