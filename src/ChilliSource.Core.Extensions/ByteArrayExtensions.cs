#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;
using System.Text;

namespace ChilliSource.Core.Extensions
{
    /// <summary>
    /// Byte array extensions.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Builds a hexadecimal string representing the provided <paramref name="bytes" />
        /// </summary>
        /// <returns>The hex string.</returns>
        /// <param name="bytes">Byte data</param>
        public static string ToHexString(this byte[] bytes)
        {

            return bytes == null ? null : string.Concat(bytes.Select(x => x.ToString("X2")).ToArray());

        }

        /// <summary>
        /// Return byte array of encoded string back to string
        /// </summary>
        /// <param name="bytes">Encoded string</param>
        /// <param name="encoding">Encoding of original string. Typically UTF8</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

    }
}
