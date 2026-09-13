using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Cursor asset.
	/// </summary>
	[Serializable]
	public struct CursorObjectData
	{
#region Fields
		/// <summary>
		/// Is just simple non-animated cursor.
		/// </summary>
		[SerializeField]
		public bool m_IsSimpleCursor;

		/// <summary>
		/// Cursor images.
		/// </summary>
		[SerializeField]
		public CursorImageData[] m_Images;
#endregion
	}

	/// <summary>
	/// Cursor image specification
	/// </summary>
	[Serializable]
	public struct CursorImageData
	{
#region Enums
		/// <summary>
		/// Mode used for the positioning.
		/// </summary>
		public enum PositionMode
		{
			/// <summary>
			/// Position is set explicitly.
			/// </summary>
			PositionExplicit = 0,

			/// <summary>
			/// Position is in the center of the cursor rect.
			/// </summary>
			PositionCenter = 1,

			/// <summary>
			/// Cursor is hidden.
			/// </summary>
			PositionHidden = 2,
		}
#endregion

#region Fields Component
		/// <summary>
		/// Position mode.
		/// </summary>
		[SerializeField]
		public PositionMode m_PositionMode;

		/// <summary>
		/// Cursor file used.
		/// </summary>
		[Attributes.AssetRefType(typeof(AssetCursorFile))]
		[SerializeField]
		public AssetRef m_CursorFile;

		/// <summary>
		/// X position.
		/// </summary>
		[SerializeField]
		public int m_PosX;

		/// <summary>
		/// Y position.
		/// </summary>
		[SerializeField]
		public int m_PosY;

		/// <summary>
		/// Index of the subimage to use.
		/// </summary>
		[SerializeField]
		public int m_SubImageIndex;
#endregion
	}
}
