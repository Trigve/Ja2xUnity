using System;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Ja2
{
	/// <summary>
	/// Random manager.
	/// </summary>
	public static class RandomManager
	{
#region Constants
		/// IMPORTANT: Changing this define will invalidate the JA2 save.
		private const int MaxPregeneratedNums = 256;

		/// Maximum value that can be returned by the rand function.
		private const int RAND_MAX = 0x7fff;
#endregion

#region Fields
		/// Pre-generated random numbers.
		private static readonly int[] guiPreRandomNums = new int[MaxPregeneratedNums];

		/// Last generated random vaue;
		private static int m_Rnd;

		/// Random numbers generated so far.
		private static uint m_Cnt;

		/// Pre-generated random number index.
		private static uint m_PreRandomIndex;
#endregion

#region Methods Static
		/// <summary>
		/// Initialize the random manager.
		/// </summary>
		public static void Init()
		{
			Debug.Log("Initializing Random");

			// Pregenerate all the random numbers.
			for(var i = 0; i < MaxPregeneratedNums; ++i)
				guiPreRandomNums[i] = GetRndNum(int.MaxValue);

			m_PreRandomIndex = 0;
		}

		/// <summary>
		/// Generate the random number.
		/// </summary>
		/// <param name="MaxNum">Maximum number to generate.</param>
		/// <returns></returns>
		private static int GetRndNum(int MaxNum)
		{
			// \TODO handle network server/client

			var ret = 0;

			if(MaxNum != 0)
			{

				if((m_Cnt++ % RAND_MAX) == 0)
				{
					// Get cursor location
					Vector3 pt = Input.mousePosition;
					Random.InitState(MaxNum ^ m_Rnd ^ (int)pt.x ^ (int)pt.y ^ Environment.TickCount);
				}

				m_Rnd = Random.Range(0, int.MaxValue);
				m_Rnd <<= 11;
				m_Rnd ^= Random.Range(0, int.MaxValue);
				m_Rnd <<= 7;
				m_Rnd ^= Random.Range(0, int.MaxValue);

				ret = (m_Rnd % MaxNum);
			}

			return ret;
		}
#endregion
	}
}
