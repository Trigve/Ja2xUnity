using UnityEngine;

namespace Ja2
{
	/// <summary>
	/// Helper component for mananing the assets during editor/playmode as <see cref="AssetRef"/>. During edit time,
	/// only "mocks" are saved, not actual assets. Then, during the runtime, the real assets are loaded from the
	/// mocks.
	/// </summary>
	public abstract class AssetRefMocker<T> : AssetRefMockerBase where T : Component
	{
#region Fields Component
		/// <summary>
		/// Component.
		/// </summary>
		[SerializeField]
		protected T? m_Component;
#endregion

#region Properties
		/// <summary>
		/// Typed component.
		/// </summary>
		protected override Component? component => m_Component;
#endregion

#region Methods Private
		/// <inheritdoc/>
		protected override void DoAwake()
		{
			// Try to find component, if not assigned already
			if(m_Component == null)
				m_Component = GetComponent<T>();
		}
#endregion
	}
}
