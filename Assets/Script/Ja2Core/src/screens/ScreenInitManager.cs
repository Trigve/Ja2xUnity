using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Init screen game manager.
	/// </summary>
	[HistoricName("INIT_SCREEN")]
	public sealed class ScreenInitManager : MonoBehaviour
	{
#region Fields Component
		/// <summary>
		/// Game state.
		/// </summary>
		[SerializeField]
		private GameState m_GameState = null!;
#endregion

#region Messages
		public void Start()
		{
		}
#endregion

	}
}
