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
    /// <summary>
    /// Changes the behaviour of EnumExtensions.GetDescription from Sentence case to Camel case
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class CamelCaseAttribute : Attribute
    {
    }
}
