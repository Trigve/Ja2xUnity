using Aspid.MVVM;
using Aspid.MVVM.StarterKit;

namespace Ja2.UI.Binder
{
	/// <summary>
	/// Binder for the <see cref="FastHelpTextComponent"/>.
	/// </summary>
	public sealed class FastHelpTextComponentBinder : ComponentMonoBinder<FastHelpTextComponent>, IBinder<string>
	{
#region Methods Public
		/// <inheritdoc/>
		public void SetValue(string? Value)
		{
			if(Value is not null)
				CachedComponent.text = Value;
		}
#endregion
	}
}
