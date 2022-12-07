using System;
using System.Linq;

namespace ChilliSource.Core.Extensions
{
    /// <summary>
    /// Extension methods for System.Uri.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Gets the host component of the specified System.Uri, without the "www.". prefix
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <returns>The host component of the URI.</returns>
        public static string AsFriendlyName(this Uri uri)
        {
            return uri.Host.TrimStart("www.");
        }

        /// <summary>
        /// Gets the root of the specified System.Uri, i.e. everything except the path and the query string
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <returns>The root component of the URI.</returns>
        public static string Root(this Uri uri)
        {
            return uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped);
        }

        /// <summary>
        /// Gets the base of the specified System.Uri, i.e. everything except the query string
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <returns>The base component of the URI.</returns>
        public static string Base(this Uri uri)
        {
            return uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
        }

        //https://stackoverflow.com/questions/372865/path-combine-for-urls
        /// <summary>
        /// Append paths to a url without having to worry about slashes
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <param name="paths">Paths to append</param>
        /// <returns>A new uri with paths appended</returns>
        public static Uri Append(this Uri uri, params string[] paths)
        {
            var newPath = paths.Aggregate(uri.Base(), (current, path) => string.Format("{0}/{1}", current.TrimEnd('/'), path.TrimStart('/')));
            return new Uri(newPath + uri.Query);
        }
    }
}
