using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Main entry class.
	/// </summary>
	public sealed class GameManager : MonoBehaviour
	{
#region Messages
		/// See unity.
		public void Start()
		{
			RandomManager.Init();
		}
#endregion
	}
}
