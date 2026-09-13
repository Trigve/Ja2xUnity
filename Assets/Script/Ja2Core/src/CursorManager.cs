using System;
using System.Collections.Generic;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Mouse cursor manager.
	/// </summary>
	[CreateAssetMenu(menuName = "JA2/Create Cursor Manager", fileName = "CursorManager")]
	public sealed class CursorManager : ScriptableObjectManager<CursorManager>
	{
#region Fields Component
		/// <summary>
		/// Asset manager.
		/// </summary>
		[SerializeField]
		private AssetManager? m_AssetManager;

		/// <summary>
		/// Cursor database.
		/// </summary>
		[SerializeField]
		private AssetCursorDatabase? m_CursorDatabase;
#endregion

#region Fields
		/// <summary>
		/// All the loaded cursors.
		/// </summary>
		private Dictionary<CursorType, CursorObject>? m_CursorsLoaded;

		/// <summary>
		/// Currently active cursor.
		/// </summary>
		private CursorObject? m_CursorActive;
#endregion

#region Methods Public
		/// <summary>
		/// Change the currently active cursor.
		/// </summary>
		/// <param name="CursorType">New cursor type.</param>
		public void ChangeCursor(CursorType CursorType)
		{
			m_CursorActive = LoadCursor(CursorType);
			m_CursorActive.ShowCursor();
		}

		/// <summary>
		/// Show cursor
		/// </summary>
		public void ShowCursor()
		{
			Cursor.visible = true;
		}

		/// <summary>
		/// Hide cursor.
		/// </summary>
		public void HideCursor()
		{
			Cursor.visible = false;
		}
#endregion

#region Methods Private
		/// <inheritdoc/>.
		protected override void DoInitialize(params object[] Params)
		{
			m_CursorsLoaded = new Dictionary<CursorType, CursorObject>();

#if DEBUG
			// Load all cursors
			foreach(CursorType it in (CursorType[])Enum.GetValues(typeof(CursorType)))
				LoadCursor(it);
#endif
		}

		/// <inheritdoc/>.
		protected override void DoDeinitialize()
		{
			m_CursorsLoaded = null;
		}

		/// <summary>
		/// Load the cursor by the given type.
		/// </summary>
		/// <param name="Type">Type of the cursor.</param>
		/// <returns><see cref="CursorObject"/> instance if found.</returns>
		private CursorObject LoadCursor(CursorType Type)
		{
			// Find if it is loaded already
			if(!m_CursorsLoaded!.TryGetValue(Type, out CursorObject cursor))
			{
				CursorObjectData cursor_data = m_CursorDatabase!.GetCursor(Type);

				// Process all images
				var cursor_images = new List<CursorImage>();
				foreach(CursorImageData it in cursor_data.m_Images)
				{
					cursor_images.Add(
						new CursorImage(it,
							m_AssetManager!.LoadAsset<AssetCursorFile>(it.m_CursorFile)!
						)
					);
				}

				cursor = new CursorObject(cursor_data,
					cursor_images
				);

				m_CursorsLoaded[Type] = cursor;
			}

			return cursor;
		}
#endregion
	}
}
