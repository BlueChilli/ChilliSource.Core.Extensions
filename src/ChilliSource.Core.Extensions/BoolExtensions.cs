#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;

namespace ChilliSource.Core.Extensions
{
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts the specified Boolean value to the equivalent 32-bit signed integer.
        /// </summary>
        /// <param name="value">The Boolean value to convert</param>
        /// <returns>The number 1 if value is true; otherwise, 0.</returns>
        public static int ToInt(this bool value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Negates !the specified Boolean value.
        /// </summary>
        public static bool Toggle(this bool value)
        {
            return !value;
        }
    }
}
