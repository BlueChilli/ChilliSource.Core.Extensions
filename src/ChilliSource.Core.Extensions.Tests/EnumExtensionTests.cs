#region License

/*
Licensed to Blue Chilli Technology Pty Ltd and the contributors under the MIT License (the "License").
You may not use this file except in compliance with the License.
See the LICENSE file in the project root for more information.
*/

#endregion

using System;
using ChilliSource.Core.Extensions;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using NuGet.Frameworks;

namespace Tests
{
    public class EnumExtensionsTests
    {
        public enum TestEnum
        {
            [System.ComponentModel.Description("Test1 Description")]
            [Data("Number", 1)]
            [Data("String", "ABC")]
            Test1 = 1,
            [System.ComponentModel.Description("Test2 Description")]
            [Data("Number", 2)]
            [Data("String", "XYZ")]
            Test2,
            [Obsolete]
            Test3
        }

        public enum TestEnumOrder
        {
            Test3,
            [Order(2)]
            Test2,
            [Order(1)]
            Test1
        }

        [Fact]
        public void ToValueString_ReturnsEnumValueAsString()
        {
            var result = TestEnum.Test1.ToValueString();
            Assert.Equal("1", result);

            result = TestEnum.Test2.ToValueString();
            Assert.NotEqual("1", result);
        }

        [Fact]
        public void GetValues_ShouldReturnListOfValuesForEnumType()
        {
            var result = EnumHelper.GetValues<TestEnum>();
            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.Equal(TestEnum.Test1, result.First());
        }

        public void GetValues_ShouldReturnListOfValuesForEnumTypeWithObsolete()
        {
            var result = EnumHelper.GetValues<TestEnum>(excludeObsolete: false);
            Assert.NotNull(result);
            Assert.True(result.Count() == 3);
#pragma warning disable CS0612 // Type or member is obsolete
            Assert.Contains(TestEnum.Test3, result);
        }

        [Fact]
        public void ToList_ShouldReturnListOfValuesForEnumType()
        {
            var result = EnumHelper.ToList<TestEnum>();
            Assert.NotNull(result);
            Assert.True(result.Count() == 2);
            Assert.Equal(TestEnum.Test2, result.Last());
        }

        [Fact]
        public void Parse_ReturnsEnumIfFound()
        {
            var result = EnumHelper.Parse<TestEnum>("Test1");
            Assert.Equal(TestEnum.Test1, result);

            var result2 = EnumHelper.Parse<TestEnum>("Rabbit");
            Assert.NotEqual(TestEnum.Test1, result2);

            var result3 = EnumHelper.Parse<TestEnum>("1");
            Assert.Equal(TestEnum.Test1, result3);
        }

        [Fact]
        public void Contains_ReturnsTrue_WhenEnumVariableContainsValue()
        {
            var test1 = TestEnum.Test2;
            Assert.True(test1.Contains(TestEnum.Test1, TestEnum.Test2));

            var test2 = TestEnum.Test1;
#pragma warning disable CS0612 // Type or member is obsolete
            Assert.False(test2.Contains(TestEnum.Test2, TestEnum.Test3));
#pragma warning restore CS0612 // Type or member is obsolete

            var test3 = FlagsEnum.Dog | FlagsEnum.Owner;
            Assert.True(test3.Contains(FlagsEnum.Fleas, FlagsEnum.Owner));
        }

        [Fact]
        public void Next_ReturnsNextEnumInSequence()
        {
            var test1 = TestEnum.Test1;
            var result = test1.Next();
            Assert.Equal(TestEnum.Test2, result);

            result = result.Next();
            Assert.Equal(TestEnum.Test1, result);

            Assert.Equal(TestEnumOrder.Test2, TestEnumOrder.Test1.Next());
            Assert.Equal(TestEnumOrder.Test3, TestEnumOrder.Test1.Next().Next());
        }

        [Fact]
        public void Previous_ReturnsPreviousEnumInSequence()
        {
            var test2 = TestEnum.Test2;
            var result = test2.Previous();
            Assert.Equal(TestEnum.Test1, result);

            result = result.Previous();
            Assert.Equal(TestEnum.Test2, result);

            Assert.Equal(TestEnumOrder.Test3, TestEnumOrder.Test1.Previous());
        }

        [Fact]
        public void IsIn_Works()
        {
            var enums = new List<TestEnum> { TestEnum.Test1, TestEnum.Test2 };

            Assert.True(TestEnum.Test1.IsIn(enums));

            Assert.True(TestEnum.Test2.IsIn(enums));

            Assert.False(TestEnum.Test3.IsIn(enums));
        }

        #region Attributes
        [Fact]
        public void GetDescription_ShouldReturnDescriptionAttributeValue_IfExists()
        {
            var result = TestEnum.Test1.GetDescription();
            Assert.NotNull(result);
            Assert.Equal("Test1 Description", result);

            var result2 = TestEnum.Test2.GetDescription();
            Assert.NotNull(result2);
            Assert.Equal("Test2 Description", result2);

            var result3 = TestEnum.Test3.GetDescription();
            Assert.Equal("Test 3", result3);

            var result4 = ((TestEnum)0).GetDescription();
            Assert.Equal("", result4);
        }

        [Fact]
        public void GetAttribute_ShouldReturnAttribute_IfTypeExists()
        {
            var result = TestEnum.Test1.GetAttribute<System.ComponentModel.DescriptionAttribute>();

            Assert.NotNull(result);
            Assert.Equal(typeof(System.ComponentModel.DescriptionAttribute), result.GetType());
            Assert.Equal("Test1 Description", result.Description);

            var result2 = TestEnum.Test1.GetAttribute<System.ComponentModel.DisplayNameAttribute>();
            Assert.Null(result2);
        }

        [Fact]
        public void GetAttributes_ShouldReturnAttributes_IfTypeExists()
        {
            var result = TestEnum.Test1.GetAttributes<DataAttribute>();

            Assert.NotNull(result);
            Assert.Equal(typeof(DataAttribute[]), result.GetType());
            Assert.Equal(2, result.Count());
            Assert.Equal("Number", result.First().Name);

            var result2 = TestEnum.Test1.GetAttributes<System.ComponentModel.DisplayNameAttribute>();
            Assert.Equal(0, result2.Count());

        }

        [Fact]
        public void GetData_ShouldReturnData_ByName_IfExists()
        {
            var result = TestEnum.Test1.GetData<string>("String");
            Assert.Equal("ABC", result);

            var result2 = TestEnum.Test1.GetData<int>("Number");
            Assert.Equal(1, result2);

            var result3 = TestEnum.Test1.GetData<decimal>("X");
            Assert.Equal(0M, result3);
        }
        #endregion

        #region Flags
        [Flags]
        enum FlagsEnum
        {
            Dog = 1,
            Puppy = 2,
            Pup = 4,
            Fleas = 8,
            Stray = 16,
            Owner = 32
        }

        [Fact]
        public void AddFlag_ShouldAddFlagToFlagsEnum()
        {
            var flags = FlagsEnum.Dog;

            var result = flags.AddFlag(FlagsEnum.Pup);

            Assert.True(result.HasFlag(FlagsEnum.Pup));
            Assert.True(result.HasFlag(FlagsEnum.Dog));
            Assert.False(result.HasFlag(FlagsEnum.Puppy));
        }

        [Fact]
        public void RemoveFlag_ShouldRemoveFlagFromFlagsEnum_IfExists()
        {
            var flags = FlagsEnum.Dog;

            var result = flags.AddFlag(FlagsEnum.Pup);
            result = result.AddFlag(FlagsEnum.Puppy);
            result = result.RemoveFlag(FlagsEnum.Pup);

            Assert.True(result.HasFlag(FlagsEnum.Puppy));
            Assert.True(result.HasFlag(FlagsEnum.Dog));
            Assert.False(result.HasFlag(FlagsEnum.Pup));
        }

        [Fact]
        public void ToFlagsList_ReturnsListOfEnumValues_ThatHasFlags()
        {
            var flags = FlagsEnum.Dog;
            flags = flags.AddFlag(FlagsEnum.Owner);
            flags = flags.AddFlag(FlagsEnum.Puppy);

            var result = flags.ToFlagsList();

            Assert.Equal(3, result.Count);
            Assert.Contains(FlagsEnum.Dog, result);
            Assert.Contains(FlagsEnum.Owner, result);
            Assert.Contains(FlagsEnum.Puppy, result);
            Assert.DoesNotContain(FlagsEnum.Fleas, result);
        }

        [Fact]
        public void ToFlags_ReturnsFlags()
        {
            var list = new List<FlagsEnum> { FlagsEnum.Dog, FlagsEnum.Owner, FlagsEnum.Stray };

            var result = list.ToFlags();

            Assert.True(result.HasFlag(FlagsEnum.Dog));
            Assert.True(result.HasFlag(FlagsEnum.Owner));
            Assert.True(result.HasFlag(FlagsEnum.Stray));
            Assert.False(result.HasFlag(FlagsEnum.Fleas));
        }
        #endregion

    }
}
