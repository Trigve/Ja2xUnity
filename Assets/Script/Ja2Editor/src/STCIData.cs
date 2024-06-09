using System.Collections.Generic;
using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// STCI data.
	/// </summary>
	internal sealed class STCIData
	{
#region Nested classes
		/// <summary>
		/// Sub-image data.
		/// </summary>
		internal struct SubImage
		{
#region Properties
			/// <summary>
			/// Subimage width.
			/// </summary>
			public int width { get; set; }

			/// <summary>
			/// Subimage height.
			/// </summary>
			public int height { get; set; }

			/// <summary>
			/// Subimage offset X.
			/// </summary>
			public int offsetX { get; set; }

			/// <summary>
			/// Subimage offset Y.
			/// </summary>
			public int offsetY { get; set; }

			/// <summary>
			/// Main texture.
			/// </summary>
			public Texture2D texture { get; set; }

			/// <summary>
			/// Alternative texture.
			/// </summary>
			public Texture2D? textureAlt { get; set; }
#endregion
		}
#endregion

#region Fields
		/// <summary>
		/// Image width.
		/// </summary>
		public int m_Width;

		/// <summary>
		/// Image height.
		/// </summary>
		public int m_Height;

		/// <summary>
		/// Subimage data.
		/// </summary>
		public readonly List<SubImage> m_SubImageData = new List<SubImage>();

		/// <summary>
		/// Application data.
		/// </summary>
		public byte[]? m_AppData;
#endregion
	}
}
