using System.Collections.Generic;
using UnityEngine;

namespace Ja2
{
    /// <summary>
    /// Texture asset info.
    /// </summary>
    public sealed class AssetTexture : AssetBase
    {
#region Fields
        /// <summary>
        /// Original name of the texture.
        /// </summary>
        public string m_NameOrig = string.Empty;

        /// <summary>
        /// Original width.
        /// </summary>
        public int m_Width;

        /// <summary>
        /// Original height.
        /// </summary>
        public int m_Height;

        /// <summary>
        /// Sprite assets.
        /// </summary>
	    public List<Sprite> m_Sprites = new List<Sprite>();
#endregion

#region Construction
	    /// <summary>
	    /// Constructor.
	    /// </summary>
	    /// <returns></returns>
	    public static AssetTexture Create()
	    {
		    var ret = CreateInstance<AssetTexture>();

		    return ret;
	    }
#endregion
    }
}
