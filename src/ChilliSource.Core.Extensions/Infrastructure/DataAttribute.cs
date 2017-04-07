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
    /// Describes additional meta-data information in an Enum value.
    /// This can be obtained by the GetData Extension method (e.g TestEnum.Value.GetData<string>("metadata_xyz"))
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class DataAttribute : Attribute
    {
        /// <summary>
        /// Meta-data name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Meta-data value. Must be a compile-time constant.
        /// </summary>
        public object Value { get; set; }

        public DataAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }

    /// <summary>
    /// Attaches an alias to an Enum value.
    /// The Enum value can be obtained from the alias by using ModelEnumExtensions.GetFromAlias<Enum>(alias) .
    /// </summary>
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Alias name
        /// </summary>
        public string Name { get; set; }

        public AliasAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Describes a custom order attribute in an Enum value.
    /// e.g :
    /// public enum ResponseToEvent
    /// {
    /// [Order(1)]
    ///  Going,
    /// [Order(3)]
    ///  NotGoing,
    /// [Order(2)]
    /// Maybe
    /// }
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class OrderAttribute : Attribute
    {
        public readonly int Order;
        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
