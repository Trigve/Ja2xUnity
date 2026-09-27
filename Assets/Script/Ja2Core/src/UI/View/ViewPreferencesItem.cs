using UnityEngine;

using Aspid.MVVM;

namespace Ja2.UI.View
{
	/// <summary>
	/// View for the one item.
	/// </summary>
	[View]
	public sealed partial class ViewPreferencesItem : MonoView
	{
#region Fields Component
		/// <summary>
		/// Item's value.
		/// </summary>
		[RequireBinder(typeof(bool))]
		[SerializeField]
		private MonoBinder[]? m_Value;

		/// <summary>
		/// Item description to show.
		/// </summary>
		[RequireBinder(typeof(string))]
		[SerializeField]
		private MonoBinder[]? m_Text;
#endregion
	}
}
