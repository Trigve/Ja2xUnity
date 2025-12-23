using System;

namespace Ja2
{
	/// <summary>
	/// Game screen data.
	/// </summary>
	internal struct GameScreenData : IEquatable<GameScreenData>
	{
#region Properties
		/// <summary>
		/// Game screen instance.
		/// </summary>
		public GameScreen gameScreen { get; private set; }

		/// <summary>
		/// Screen options.
		/// </summary>
		public GameScreenOptions options { get; private set; }
#endregion

#region Methods Public
		/// <inheritdoc />
		public bool Equals(GameScreenData Other)
		{
			return gameScreen.Equals(Other.gameScreen);
		}

		/// <inheritdoc />
		public override bool Equals(object? Obj)
		{
			return Obj is GameScreenData other && Equals(other);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return gameScreen.GetHashCode();
		}
#endregion

#region Operators
		/// <summary>
		/// Equality operator.
		/// </summary>
		/// <param name="Left">Left side instance.</param>
		/// <param name="Right">Right side instance.</param>
		/// <returns>True, if both sides are equal. Otherwise, false.</returns>
		public static bool operator ==(GameScreenData Left, GameScreenData Right)
		{
			return Left.Equals(Right);
		}

		/// <summary>
		/// Non-equality operator.
		/// </summary>
		/// <param name="Left">Left side instance.</param>
		/// <param name="Right">Right side instance.</param>
		/// <returns>True, if both sides are non-equal. Otherwise, false.</returns>
		public static bool operator !=(GameScreenData Left, GameScreenData Right)
		{
			return !Left.Equals(Right);
		}
#endregion

#region Construction
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="GameScreen">Game screen instance.</param>
		/// <param name="Options">Game screen options.</param>
		public GameScreenData(GameScreen GameScreen, GameScreenOptions Options)
		{
			gameScreen = GameScreen;
			options = Options;
		}
#endregion
	}
}
