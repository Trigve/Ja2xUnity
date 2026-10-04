using Cysharp.Threading.Tasks;

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

		/// <summary>
		/// Start the batch mode.
		/// </summary>
		public void StartBatchMode();

		/// <summary>
		/// Stop the batch mode.
		/// </summary>
		/// <param name="DoLoadAssets">If true, all the registred asset ref mockers will be loaded. Othewise nothing will happen.</param>
		public void StopBatchMode(bool DoLoadAssets);

		/// <summary>
		/// Load all the assets from the AssetRefs.
		/// </summary>
		public void LoadAssets();

		/// <summary>
		/// Load all the assets from the AssetRefs.
		/// </summary>
		public UniTask LoadAssetsAsync();
#endregion
	}
}
