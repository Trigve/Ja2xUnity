using UnityEngine;
using UnityEngine.EventSystems;

using TMPro;

using Ja2.Attributes;

namespace Ja2.UI
{
	/// <summary>
	/// Fast help text component.
	/// </summary>
	public sealed class FastHelpTextComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;

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

		/// <summary>
		/// Basic font asset ref.
		/// </summary>
		[Header("Font specs")]
		[AssetRefType(typeof(TMP_FontAsset))]
		[SerializeField]
		private AssetRef m_FontBasicAssetRef;

		/// <summary>
		/// Bold font asset ref.
		/// </summary>
		[SerializeField]
		[AssetRefType(typeof(TMP_FontAsset))]
		private AssetRef m_FontBoldAssetRef;

		/// <summary>
		/// Color used for the hiliting.
		/// </summary>
		[SerializeField]
		private Color32 m_ColorHilite;
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

		/// <summary>
		/// Basic font asset.
		/// </summary>
		private TMP_FontAsset? m_FontBasic;
#endregion

#region Properties
		/// <summary>
		/// Text to show property.
		/// </summary>
		public string text
		{
			// ReSharper disable once ArrangeAccessorOwnerBody
			set
			{
				// Process the formatting
				//
				// <b>...</b> - Bold font with color
				m_Text = value.Replace("<b>",
					string.Format("<font=\"@{0}\"><color=#{1}>",
						m_FontBoldAssetRef.combinedPath,
						ColorUtility.ToHtmlStringRGBA(m_ColorHilite)
					)
				)
				.Replace("</b>",
					"</color></font>"
				);
			}
		}
#endregion

#region Messages
		public void Awake()
		{
			// Load the basic font that will be used
			m_FontBasic = m_GameState.assetManager.LoadAsset<TMP_FontAsset>(m_FontBasicAssetRef);
		}

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
						var fast_help = BootsrapManager.instance.fastHelpTextManager!.GetGameObject(this,
							m_FontBasic!
						).GetComponent<FastHelpTextRendererComponent>();
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
