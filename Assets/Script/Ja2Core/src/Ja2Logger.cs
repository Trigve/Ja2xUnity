//#define JA2_VFS_DEBUG

using System.Diagnostics;

using JetBrains.Annotations;

using UnityEngine;

using Debug = UnityEngine.Debug;

namespace Ja2
{
	/// <summary>
	/// Logger.
	/// </summary>
	internal static class Ja2Logger
	{
#region Methods
		/// <summary>
		/// Log info message.
		/// </summary>
		/// <param name="Message"></param>
		/// <param name="Args"></param>
		[StringFormatMethod("Message")]
		internal static void LogInfo(string Message, params object[] Args)
		{
			Debug.LogFormat(LogType.Log,
				LogOption.NoStacktrace,
				null,
				Message,
				Args
			);
		}

		/// <summary>
		/// Log generic warning message.
		/// </summary>
		/// <param name="Message"></param>
		/// <param name="Args"></param>
		[StringFormatMethod("Message")]
		internal static void LogWarning(string Message, params object[] Args)
		{
			Debug.LogWarningFormat(Message,
				Args
			);
		}

		/// <summary>
		/// VFS log.
		/// </summary>
		/// <param name="Message"></param>
		/// <param name="Args"></param>
		[Conditional("JA2_VFS_DEBUG")]
		[StringFormatMethod("Message")]
		internal static void LogVfs(string Message, params object[] Args)
		{
			Debug.LogFormat("VFS: " + Message, Args);
		}

		/// <summary>
        /// Sound log.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Args"></param>
        [Conditional("JA2_VFS_DEBUG")]
        [StringFormatMethod("Message")]
        internal static void LogSound(string Message, params object[] Args)
        {
        	Debug.LogFormat("Sound: " + Message, Args);
        }
#endregion
	}
}
