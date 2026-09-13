using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Asset for the cursor file.
	/// </summary>
	public sealed class AssetCursorFile : AssetBase
	{
#region Fields Component
		/// <summary>
		/// Is cursor animated.
		/// </summary>
		[SerializeField]
		private bool m_IsAnimated;

		/// <summary>
		/// All the cursor textures.
		/// </summary>
		[SerializeField]
		private Texture2D[] m_Textures = Array.Empty<Texture2D>();

		/// <summary>
		/// All the cursor sprites.
		/// </summary>
		[SerializeField]
		private Sprite[] m_Sprites =  Array.Empty<Sprite>();
#endregion

#region Properties
		/// <summary>
		/// Get the texture from the given index.
		/// </summary>
		/// <param name="Index">Index of the texture to get.</param>
		public Texture2D this[int Index] => m_Textures[Index];
#endregion

#region Construction
		/// <summary>
		/// Create the asset.
		/// </summary>
		/// <param name="IsAnimated">Is cursor animated.</param>
		/// <param name="Textures">Textures associated with cursor.</param>
		/// <param name="Sprites">Sprites associated with cursor.</param>
		/// <returns>New instance.</returns>
		public static AssetCursorFile Create(bool IsAnimated, Texture2D[] Textures, Sprite[] Sprites)
		{
			var ret = CreateInstance<AssetCursorFile>();

			ret.m_IsAnimated = IsAnimated;
			ret.m_Textures = Textures;
			ret.m_Sprites = Sprites;

			return ret;
		}
#endregion
	}
}
