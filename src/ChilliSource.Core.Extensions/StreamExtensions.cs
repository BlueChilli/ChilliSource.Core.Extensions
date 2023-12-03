#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.IO;
using System.Threading.Tasks;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Stream extensions.
	/// </summary>
	public static class StreamExtensions
	{
		/// <summary>
		/// Converts the specified stream to a byte array
		/// </summary>
		/// <returns>The byte array.</returns>
		/// <param name="stream">Stream to convert.</param>
		public static byte[] ReadToByteArray(this Stream stream)
		{
			if (stream == null)
			{
				return null;
			}

            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using (MemoryStream ms2 = new MemoryStream())
			{
				stream.CopyTo(ms2);
				return ms2.ToArray();
			}
		}

        /// <summary>
        /// Converts the specified stream to a byte array
        /// </summary>
        /// <returns>The byte array.</returns>
        /// <param name="stream">Stream to convert.</param>
        public async static Task<byte[]> ReadToByteArrayAsync(this Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            if (stream is MemoryStream ms)
            {
                return ms.ToArray();
            }

            using (MemoryStream ms2 = new MemoryStream())
            {
                await stream.CopyToAsync(ms2);
                return ms2.ToArray();
            }
        }
    }
}
