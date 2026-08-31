using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// STCI Palette asset.
	/// </summary>
	public sealed class AssetStciPalette : AssetBase
	{
#region Fields Component
		/// <summary>
		/// Color indices.
		/// </summary>
		[SerializeField]
		private Color32[] m_Colors = new Color32[256];
#endregion

#region Properties
		/// <summary>
		/// Number of colors in palette.
		/// </summary>
		public int count => m_Colors.Length;

		/// <summary>
		/// Get the color from the index.
		/// </summary>
		/// <param name="Index">Index to use in the color array.</param>
		public Color32 this[int Index] => m_Colors[Index];
#endregion

#region Construction
		/// <summary>
		/// Create the instance.
		/// </summary>
		/// <param name="Colors">Color indices.</param>
		/// <returns>New Instance.</returns>
		public static AssetStciPalette Create(IEnumerable<Color32> Colors)
		{
			var ret = CreateInstance<AssetStciPalette>();
			ret.name = "Palette";
			ret.m_Colors = Colors.ToArray();

			return ret;
		}
#endregion
	}
}
