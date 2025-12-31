using System;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Ja2.UI
{
	/// <summary>
	/// Data for the asset mock.
	/// </summary>
	[Serializable]
	public struct AssetMockData
	{
#region Fields
		/// <summary>
		/// Stored assets.
		/// </summary>
		[SerializeField]
		public Object?[] m_Assets;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		public AssetMockData(Object?[] Assets)
		{
			m_Assets = Assets;
		}
#endregion
	}
}
