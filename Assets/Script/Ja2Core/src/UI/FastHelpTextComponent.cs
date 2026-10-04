using UnityEngine;
using UnityEngine.EventSystems;

namespace Ja2.UI
{
	/// <summary>
	/// Fast help text component.
	/// </summary>
	public sealed class FastHelpTextComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
#region Fields Component
		/// <summary>
		/// Delay, after which the fast help popup should be displayed.
		/// </summary>
		[SerializeField]
		private float m_Delay;

		/// <summary>
		/// Text to show.
		/// </summary>
		[SerializeField]
		private string m_Text = string.Empty;
#endregion

#region Fields
		/// <summary>
		/// Mouse entered the control.
		/// </summary>
		private bool m_IsEntered;

		/// <summary>
		/// Is the fast help shown.
		/// </summary>
		private bool m_IsShown;

		/// <summary>
		/// Time elapsed after mouse entered.
		/// </summary>
		private float m_Timer;
#endregion

#region Properties
		/// <summary>
		/// Text to show property.
		/// </summary>
		public string text
		{
			set => m_Text = value;
		}
#endregion

#region Messages
		public void Update()
		{
			// Only if cursor is present
			if(m_IsEntered)
			{
				// Only if isn't shown already
				if(!m_IsShown)
				{
					m_Timer += Time.deltaTime;

					if(m_Timer >= m_Delay)
					{
						m_IsShown = true;

						// Instantiate the fast help
						var fast_help = BootsrapManager.instance.fastHelpTextManager!.GetGameObject(this).GetComponent<FastHelpTextRendererComponent>();
						fast_help.text = m_Text;

						// Set the position
						var fast_help_trans = (RectTransform)fast_help.transform;
						Vector3 position = fast_help_trans.position;
						position.x = transform.position.x + 15;
						position.y = transform.position.y;

						fast_help_trans.position = position;

						fast_help.RefreshText();
					}
				}
			}
		}
#endregion

#region Methods Public
		/// <inheritdoc/>
		public void OnPointerEnter(PointerEventData EventData)
		{
			m_IsEntered = true;
			m_IsShown = false;
			m_Timer = 0;
		}

		/// <inheritdoc/>
		public void OnPointerExit(PointerEventData EventData)
		{
			m_IsEntered = false;

			// Destroy the fast help
			BootsrapManager.instance.fastHelpTextManager!.ReleaseGameObject(this);
		}
#endregion

	}
}
