using System;

namespace Ja2.Vfs
{
	/// <summary>
	/// File attributes.
	/// </summary>
	internal readonly struct FileAttributes
	{
#region Enums
		/// <summary>
		/// File attribute,
		/// </summary>
		[Flags]
		public enum Attribute
		{
			AttribInvalid = 0,
			AttribArchive = 1,
			AttribDirectory	= 1 << 1,
			AttribHidden = 1 << 2,
			AttribNormal = 1 << 3,
			AttribReadonly = 1 << 4,
			AttribSystem = 1 << 5,
			AttribTemporary = 1 << 6,
			AttribCompressed = 1 << 7,
			AttribOffline = 1 << 8,
		};

		/// <summary>
		/// Location when the file resides
		/// </summary>
		/// [Flags]
		public enum LocationType
		{
			LocNone = 0,
			LocLibrary = 1,
			LocDir = 1 << 1,
			LocRoDir = 1 << 2,
		};
#endregion

#region Properties
		/// Attributes
		public Attribute attribs { get; }

		/// Location type.
		public LocationType location { get; }
#endregion

#region Contruction
		public FileAttributes(Attribute Attribs, LocationType Location)
		{
			attribs = Attribs;
			location = Location;
		}
#endregion
	}
}
