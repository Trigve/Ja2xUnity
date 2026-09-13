using System.Collections.Generic;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Cursor runtime object.
	/// </summary>
	internal sealed class CursorObject
	{
#region Fields
		/// <summary>
		/// Cursor data.
		/// </summary>
		private readonly CursorObjectData m_CursorData;

		/// <summary>
		/// All the images.
		/// </summary>
		private readonly List<CursorImage> m_Images;
#endregion

#region Methods Public
		/// <summary>
		/// Show the cursor.
		/// </summary>
		public void ShowCursor()
		{
			// Simple curosr
			if(m_CursorData.m_IsSimpleCursor)
			{
				// Get the first one
				CursorImage cursor_image = m_Images[0];

				Cursor.SetCursor(cursor_image.texture,
					cursor_image.offset,
					// Need to use software so the cursor isn't enlarged
					CursorMode.ForceSoftware
				);
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="CursorData">Cursor data.</param>
		/// <param name="Images">Cursor images.</param>
		public CursorObject(CursorObjectData CursorData, List<CursorImage> Images)
		{
			m_CursorData = CursorData;
			m_Images = Images;
		}
#endregion
	}

	/// <summary>
	/// Cursor image.
	/// </summary>
	internal sealed class CursorImage
	{
#region Fields
		/// <summary>
		/// Cursor image data.
		/// </summary>
		private readonly CursorImageData m_ImageData;

		/// <summary>
		/// Cursor file asset.
		/// </summary>
		private readonly AssetCursorFile m_AssetCursorFile;
#endregion

#region Properties
		/// <summary>
		/// Get the texture for the given cursor image.
		/// </summary>
		public Texture2D texture => m_AssetCursorFile[m_ImageData.m_SubImageIndex];

		/// <summary>
		/// Get the cursor hotspot offset.
		/// </summary>
		public Vector2 offset => new Vector2(m_ImageData.m_PosX, m_ImageData.m_PosY);
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="ImageData">Image data.</param>
		/// <param name="Asset">Cursor file asset.</param>
		public CursorImage(CursorImageData ImageData, AssetCursorFile Asset)
		{
			m_ImageData = ImageData;
			m_AssetCursorFile = Asset;
		}
#endregion
	}
}
