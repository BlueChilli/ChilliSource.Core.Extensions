using System;
namespace ChilliSource.Core.Extensions
{
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
        /// <summary>
        /// The order.
        /// </summary>
        public int Order { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ChilliSource.Core.Extensions.OrderAttribute"/> class.
        /// </summary>
        /// <param name="order">Order.</param>
        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
