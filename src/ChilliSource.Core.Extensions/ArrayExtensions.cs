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
    public static class ArrayExtensions
    {
        public static T[] EmptyArray<T>() { return EmptyArrayHolder<T>.Value; }
    }

    internal static class EmptyArrayHolder<T>
    {
        public static readonly T[] Value = new T[0] { };
    }
}
