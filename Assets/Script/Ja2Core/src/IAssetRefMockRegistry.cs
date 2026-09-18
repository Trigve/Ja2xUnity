namespace Ja2
{
	/// <summary>
	/// Interface for registration of the asset ref mockers.
	/// </summary>
	public interface IAssetRefMockRegistry
	{
#region Methods Public
		/// <summary>
		/// Register new ref mocker to the manager.
		/// </summary>
		/// <param name="MockerComponent">Component to register.</param>
		public void RegisterAssetRefMocker(AssetRefMockerBase MockerComponent);
#endregion
	}
}
