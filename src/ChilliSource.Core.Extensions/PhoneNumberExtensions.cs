#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChilliSource.Core.Extensions
{
	/// <summary>
	/// Phone number extensions.
	/// </summary>
	public static class PhoneNumberExtensions
	{
		/// <summary>
		/// Determines if the provided phone number is a valid Australian mobile number.
		/// </summary>
		/// <returns><c>true</c>, if valid australian mobile number was ised, <c>false</c> otherwise.</returns>
		/// <param name="number">Number.</param>
		public static bool IsValidAustralianMobileNumber(this string number)
		{
			var regEx = new Regex(@"^(?:\+?61|0)4\)?(?:[ -]?[0-9]){7}[0-9]$");

			return regEx.IsMatch(number);
		}
		/// <summary>
		/// Returns the specified mobile number formatted as an international number
		/// </summary>
		/// <returns>The international mobile format.</returns>
		/// <param name="mobile">Mobile.</param>
		public static string FormatAsInternationalMobileFormat(this string mobile)
		{

			if (!String.IsNullOrWhiteSpace(mobile) && !mobile.StartsWith("+"))
			{
				return String.Format("+{0}", mobile);
			}

			return mobile;
		}

		private static string RemoveSpaces(this string input)
		{
			return String.IsNullOrWhiteSpace(input) ? input : Regex.Replace(input, @"[^0-9]+", string.Empty);
		}


	}
}
