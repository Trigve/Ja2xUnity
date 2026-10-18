using System;

using UnityEngine;

namespace Ja2.Editor
{
	/// <summary>
	/// Data for the 9-slice sprite.
	/// </summary>
	[Serializable]
	public struct Sprite9SliceData
	{
#region Fields
		/// <summary>
		/// Top left image index.
		/// </summary>
		[SerializeField]
		public int m_TopLeftIndex;

		/// <summary>
		/// Top image index.
		/// </summary>
		[SerializeField]
		public int m_TopIndex;

		/// <summary>
		/// Top right image index.
		/// </summary>
		[SerializeField]
		public int m_TopRightIndex;

		/// <summary>
		/// Left image index.
		/// </summary>
		[SerializeField]
		public int m_LeftIndex;

		/// <summary>
		/// Right image index.
		/// </summary>
		[SerializeField]
		public int m_RightIndex;

		/// <summary>
		/// Bottom left image index.
		/// </summary>
		[SerializeField]
		public int m_BottomLeftIndex;

		/// <summary>
		/// Bottom image index.
		/// </summary>
		[SerializeField]
		public int m_BottomIndex;

		/// <summary>
		/// Bottom right image index.
		/// </summary>
		[SerializeField]
		public int m_BottomRightIndex;
#endregion

#region Properties
		/// <summary>
		/// Get the index of the image from the position.
		/// </summary>
		/// <param name="Index">Index of the position.</param>
		/// <exception cref="IndexOutOfRangeException">Thrown, if <paramref name="Index"/> is out of range.</exception>
		public int this[int Index]
		{
			get
			{

				return Index switch
				{
					0 => m_TopLeftIndex,
					1 => m_TopIndex,
					2 => m_TopRightIndex,
					3 => m_LeftIndex,
					4 => m_RightIndex,
					5 => m_BottomLeftIndex,
					6 => m_BottomIndex,
					7 => m_BottomRightIndex,
					_ => throw new IndexOutOfRangeException()
				};
			}
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="TopLeftIndex"></param>
		/// <param name="TopIndex"></param>
		/// <param name="TopRightIndex"></param>
		/// <param name="LeftIndex"></param>
		/// <param name="RightIndex"></param>
		/// <param name="BottomLeftIndex"></param>
		/// <param name="BottomIndex"></param>
		/// <param name="BottomRightIndex"></param>
		public Sprite9SliceData(int TopLeftIndex = 0, int TopIndex = 1, int TopRightIndex = 2, int LeftIndex = 3, int RightIndex = 4, int BottomLeftIndex = 5, int BottomIndex = 6, int BottomRightIndex = 7)
		{
			m_TopLeftIndex = TopLeftIndex;
			m_TopIndex = TopIndex;
			m_TopRightIndex = TopRightIndex;
			m_LeftIndex = LeftIndex;
			m_RightIndex = RightIndex;
			m_BottomLeftIndex = BottomLeftIndex;
			m_BottomIndex = BottomIndex;
			m_BottomRightIndex = BottomRightIndex;
		}
#endregion
	}
}
