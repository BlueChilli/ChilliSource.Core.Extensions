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
		/// Converts float radiants to degrees
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="radiants">Radiants.</param>
		public static float ToDegrees(this float radiants) => (float)(radiants * 180 / Math.PI);

		/// <summary>
		/// Converts dobule radiants to degrees
		/// </summary>
		/// <returns>The degrees.</returns>
		/// <param name="radiants">Radiants.</param>
		public static double ToDegrees(this double radiants) => radiants * 180 / Math.PI;

		/// <summary>
		/// Converts float degrees to radiants
		/// </summary>
		/// <returns>The radiants.</returns>
		/// <param name="degrees">Degrees.</param>
		public static float ToRadiants(this float degrees) => (float)(degrees / 180 * Math.PI);

		/// <summary>
		/// Converts double degrees to radiants
		/// </summary>
		/// <returns>The radiants.</returns>
		/// <param name="degrees">Degrees.</param>
		public static double ToRadiants(this double degrees) => degrees / 180 * Math.PI;
	}
}
