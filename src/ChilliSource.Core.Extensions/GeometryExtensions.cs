#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
namespace ChilliSource.Core
{
	public static partial class GeometryExtensions
	{
		public static float ToDegrees(this float radiants) => (float)(radiants * 180 / Math.PI);

		public static double ToDegrees(this double radiants) => radiants * 180 / Math.PI;

		public static float ToRadiants(this float degrees) => (float)(degrees / 180 * Math.PI);

		public static double ToRadiants(this double degrees) => degrees / 180 * Math.PI;
	}
}
