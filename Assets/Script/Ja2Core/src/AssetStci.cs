using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// STCI asset.
	/// </summary>
	public sealed class AssetStci : AssetBase
	{
#region Fields
		/// <summary>
		/// Original width.
		/// </summary>
		[SerializeField]
		private int m_Width;

		/// <summary>
		/// Original height.
		/// </summary>
		[SerializeField]
		private int m_Height;

		/// <summary>
		/// Sub-image textures.
		/// </summary>
		[SerializeField]
		private Texture2D[] m_Textures = Array.Empty<Texture2D>();

		/// <summary>
		/// All the sub-images as sprites.
		/// </summary>
		[SerializeField]
		private Sprite[] m_Sprites =  Array.Empty<Sprite>();

		/// <summary>
		/// All the sub-image data.
		/// </summary>
		/// <returns></returns>
		[SerializeField]
		private STCISubImageData[] m_SubImages = Array.Empty<STCISubImageData>();

		/// <summary>
		/// Application data.
		/// </summary>
		[SerializeField]
		private byte[]? m_AppData;
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="Name">Name of the asset.</param>
		/// <param name="Width">Asset total width.</param>
		/// <param name="Height">Asset total height.</param>
		/// <param name="Textures">All the textures.</param>
		/// <param name="Sprites">All the sprites.</param>
		/// <param name="SubImages">Sub-image data.</param>
		/// <returns></returns>
		public static AssetStci Create(string Name, int Width, int Height, Texture2D[] Textures, Sprite[] Sprites, STCISubImageData[] SubImages)
		{
			var ret = CreateInstance<AssetStci>();
			ret.name = Name;
			ret.m_Width = Width;
			ret.m_Height = Height;
			ret.m_Textures = Textures;
			ret.m_Sprites = Sprites;
			ret.m_SubImages = SubImages;

			return ret;
		}
#endregion
	}

	/// <summary>
	/// STCI sub-image data.
	/// </summary>
	[Serializable]
	public struct STCISubImageData
	{
#region Fields
		/// <summary>
		/// Index of the sub-image.
		/// </summary>
		public int m_Index;

		/// <summary>
		/// Draw offset (JA2 stores per-tile offsets) in pixels.
		/// </summary>
		public Vector2Int m_Offset;
#endregion
	}

}
