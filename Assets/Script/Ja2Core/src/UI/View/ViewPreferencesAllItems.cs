using UnityEngine;

using Aspid.MVVM;

namespace Ja2.UI.View
{
	/// <summary>
	/// View for all the preferences items.
	/// </summary>
	[View]
	public sealed partial class ViewPreferencesAllItems : MonoView
	{
#region Fields Component
		/// <summary>
		/// All item's binder.
		/// </summary>
		[SerializeField]
		private MonoBinder[]? m_Items;
#endregion
	}
}
