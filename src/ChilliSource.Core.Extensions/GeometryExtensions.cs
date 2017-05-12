#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Geometry extensions.
	/// </summary>
	public static partial class GeometryExtensions
	{
		/// <summary>
		/// Converts float radians to degrees
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="radians">Radians.</param>
		public static float ToDegrees(this float radians) => (float)(radians * 180 / Math.PI);

		/// <summary>
		/// Converts dobule radians to degrees
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="radiants">Radians.</param>
		public static double ToDegrees(this double radians) => radians * 180 / Math.PI;

		/// <summary>
		/// Converts float degrees to radians
		/// </summary>
		/// <returns>The radians.</returns>
		/// <param name="degrees">Degrees.</param>
		public static float ToRadians(this float degrees) => (float)(degrees / 180 * Math.PI);

		/// <summary>
		/// Converts double degrees to radians
		/// </summary>
		/// <returns>The radiants.</returns>
		/// <param name="degrees">Degrees.</param>
		public static double ToRadians(this double degrees) => degrees / 180 * Math.PI;
	}
}
