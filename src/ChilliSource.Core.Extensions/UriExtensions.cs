using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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

        /// <summary>
        /// Gets the domain of the specified System.Uri, i.e. https://www.mysite.com returns mysite.com
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <returns>The base component of the URI.</returns>
        public static string Domain(this Uri uri)
        {
            var host = uri.DnsSafeHost;
            return host.StartsWith("www.") ? host.Substring(4) : host;
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

        /// <summary>
        /// Append a trailing slash to the path if needed
        /// </summary>
        /// <param name="uri">A System.Uri.</param>
        /// <returns>A uri with a trailing slash</returns>
        public static Uri WithTrailingSlash(this Uri uri)
        {
            if (!uri.Base().EndsWith('/'))
            {
                return new Uri(uri.Base() + "/" + uri.Query);
            }
            return uri;
        }
    }
}
