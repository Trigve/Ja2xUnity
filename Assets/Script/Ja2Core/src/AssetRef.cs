using System;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Asset reference.
	/// </summary>
	[Serializable]
	public struct AssetRef
	{
#region Fields
		/// <summary>
		/// Bundle.
		/// </summary>
		[SerializeField]
		private string m_Bundle;

		/// <summary>
		/// Asset path.
		/// </summary>
		[SerializeField]
		private string m_AssetPath;
#endregion

#region Properties
		/// <summary>
		/// Bundle.
		/// </summary>
		public string bundle => m_Bundle;

		/// <summary>
		/// Full bundle name (with extension).
		/// </summary>
		public string bundleFull => m_Bundle + ".bundle";

		/// <summary>
		/// Asset path.
		/// </summary>
		public string assetPath => m_AssetPath;

		/// <summary>
		/// As combined path.
		/// </summary>
		public string combinedPath => m_Bundle + ":" + m_AssetPath;
#endregion

#region Methods Static
		/// <summary>
		/// Parse from string.
		/// </summary>
		/// <param name="AssetPathCombined">Combined string from which to parse.</param>
		/// <returns></returns>
		public static AssetRef Parse(string AssetPathCombined)
		{
			string bundle, path;

			int idx = AssetPathCombined.IndexOf(':');
			if(idx != -1)
			{
				bundle = AssetPathCombined[..idx];
				path = AssetPathCombined[(idx + 1)..];
			}
			else
				bundle = path = string.Empty;

			return new AssetRef(path,
				bundle
			);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="AssetPath">Asset Path</param>
		/// <param name="Bundle">Bundle</param>
		public AssetRef(string AssetPath, string Bundle = Constants.StringEmpty)
		{
			m_Bundle = Bundle;
			m_AssetPath = AssetPath;
		}
#endregion
	}
}
