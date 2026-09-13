using UnityEngine;

using AYellowpaper.SerializedCollections;

namespace Ja2
{
	/// <summary>
	/// Database of all cursors.
	/// </summary>
	public sealed class AssetCursorDatabase : AssetBase
	{
#region Fields Components
		/// <summary>
		/// All the cursor map.
		/// </summary>
		[SerializeField]
		private SerializedDictionary<CursorType, CursorObjectData> m_Cursors = new();
#endregion

#region Methods Public
		/// <summary>
		/// Get the cursor by type.
		/// </summary>
		/// <param name="Type">Cursor type.</param>
		/// <returns><see cref="CursorObjectData"/> instance for the given type, if found. Otherwise, exception is thrown.</returns>
		public CursorObjectData GetCursor(CursorType Type)
		{
			return m_Cursors[Type];
		}
#endregion

#region Methods Public Static
#if UNITY_EDITOR
		/// <summary>
		/// Context menu asset creation.
		/// </summary>
		[UnityEditor.MenuItem("Assets/Create/JA2 Asset Cursor DB", false, 1)]
		public static void CreateContext()
		{
			UnityEditor.ProjectWindowUtil.CreateAsset(Create(),
				"Cursor DB.asset"
			);
		}
#endif
#endregion

#region Construction
		/// <summary>
		/// Construction.
		/// </summary>
		/// <returns>New instance.</returns>
		public static AssetCursorDatabase Create()
		{
			var ret = CreateInstance<AssetCursorDatabase>();

			return ret;
		}
#endregion
	}
}
