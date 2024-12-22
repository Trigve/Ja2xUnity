namespace Ja2.Editor
{
    /// <summary>
    /// File data from different source.
    /// </summary>
    public readonly struct FileData
    {
#region Properties
        /// <summary>
        /// File origina path.
        /// </summary>
	    public string path { get; }

        /// <summary>
        /// Data for the file.
        /// </summary>
        public byte[] data { get; }
#endregion

#region Construction
	    public FileData(string Path, byte[] Data)
	    {
		    path = Path;
		    data = Data;
	    }
#endregion
    }
}
