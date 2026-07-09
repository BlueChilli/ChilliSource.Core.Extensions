#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO.Compression;

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

                return JsonDecompressToObject<T>(stream);
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

        /// <summary>
        /// Compresses an object to a GZip stream after serializing it to JSON.
        /// </summary>
        /// <param name="obj">The object to compress.</param>
        /// <returns>A memory stream containing the compressed JSON data.</returns>
        public static MemoryStream JsonCompressToStream(object obj)
        {
            var jsonString = JsonSerializer.Serialize(obj);

            var compressedStream = new MemoryStream();

            using (var zipStream = new GZipStream(compressedStream, CompressionLevel.Fastest, leaveOpen: true))
            using (var writer = new StreamWriter(zipStream, Encoding.UTF8))
            {
                writer.Write(jsonString);
            }

            compressedStream.Position = 0;

            return compressedStream;
        }

        /// <summary>
        /// Decompresses a GZip stream and deserializes the JSON data to an object of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="compressedStream">The stream containing the compressed JSON data.</param>
        /// <returns>An object of type T deserialized from the JSON data.</returns>
        public static T JsonDecompressToObject<T>(this Stream compressedStream)
        {
            if (compressedStream.CanSeek)
            {
                compressedStream.Position = 0;
            }

            using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var reader = new StreamReader(zipStream, Encoding.UTF8);
            var jsonString = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }

}

