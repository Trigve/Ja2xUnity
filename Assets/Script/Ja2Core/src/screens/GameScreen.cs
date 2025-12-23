using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Base class for the screen implementations.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Screen", fileName = "Screen")]
	public class GameScreen : AssetBase
	{
#region Fields Component
		/// <summary>
		/// Scene to use.
		/// </summary>
		[SerializeField]
		private string m_Scene;
#endregion
	}
}
