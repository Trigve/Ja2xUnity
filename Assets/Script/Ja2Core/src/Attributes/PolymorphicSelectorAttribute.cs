using System;

using UnityEngine;

namespace Ja2.Attributes
{
	/// <summary>
	/// Helper attribute used for the editor GUI for selecting the right type.
	/// </summary>
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class PolymorphicSelectorAttribute : PropertyAttribute
	{
	}
}
