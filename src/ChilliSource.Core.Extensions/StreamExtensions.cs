#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.IO;
using MessagePack;
using System.Text;
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

        public static T DeserializeTo<T>(this Stream stream, StreamSerializationOptions options = null)
        {
            options = options ?? new StreamSerializationOptions();

            try
            {
                if (stream == null)
                    return default(T);

                if (options.SkipFormatterForStrings && typeof(T) == typeof(string))
                {
                    using (var streamReader = new StreamReader(stream, Encoding.UTF8, false, 4096, leaveOpen: true))
                    {
                        object value = streamReader.ReadToEnd();
                        return (T)value;
                    }
                }

                return MessagePackSerializer.Deserialize<T>(stream, MessagePack.Resolvers.ContractlessStandardResolverAllowPrivate.Options);
            }
            finally
            {
                if (!options.LeaveOpen && stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        public class StreamSerializationOptions
        {
            public StreamSerializationOptions()
            {
                this.LeaveOpen = false;
            }

            public bool LeaveOpen { get; set; }

            public bool SkipFormatterForStrings { get; set; }
        }
    }
}
