using System.IO;

using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Base class for the SO settings classes.
	/// </summary>
	public abstract class ScriptableObjectSettings<T> : ScriptableObject where T : ScriptableObject
	{
#region Fields Static
		/// <summary>
		/// Static instance.
		/// </summary>
		private static T? m_Instance;
#endregion

#region Properties Static
		/// <summary>
		/// Get current instance.
		/// </summary>
		/// <exception cref="FileNotFoundException"></exception>
		public static T instance
		{
			get
			{
#if UNITY_EDITOR
				// Not loaded yet
				if(m_Instance is null)
				{
					string[] assets_found = UnityEditor.AssetDatabase.FindAssets(
						string.Format("t:{0}",
							typeof(T).Name
						)
					);
					if(assets_found.Length != 0)
					{
						m_Instance = UnityEditor.AssetDatabase.LoadMainAssetAtPath(
							UnityEditor.AssetDatabase.GUIDToAssetPath(assets_found[0])
						) as T;
					}
				}

				// Still not found
				if(m_Instance is null)
				{
					throw new FileNotFoundException(
						string.Format("Cannot find {0} asset.",
							typeof(T).Name
						)
					);
				}
#endif
				return m_Instance;
			}
		}
#endregion
	}
}
