using System;
using NUnit.Framework;
using FubuMVC.Core;

namespace FubuMVC.Tests
{
    [TestFixture]
    public class BasicExtensionsTester
    {
        [Test]
        public void If_should_return_original_value()
        {
            var test = new TestObject { Boolean = true };

            "test".If(() => test.Boolean).ShouldEqual("test");
        }

        [Test]
        public void If_should_return_empty_string_value()
        {
            var test = new TestObject { Boolean = false };

            "test".If(() => test.Boolean).ShouldEqual("");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void If_should_return_an_ArgumentException()
        {
            var test = new TestObject { Value = 1 };

            "test".If(() => test.Value == 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void If_should_also_return_an_ArgumentException()
        {
            var test = new TestObject { Boolean = true };

            "test".If(() => test.Boolean && test.Boolean);
        }

        [Test]
        public void IfNot_should_return_original_value()
        {
            var test = new TestObject { Boolean = true };

            "test".IfNot(() => test.Boolean).ShouldEqual("");
        }

        [Test]
        public void IfNot_should_return_empty_string_value()
        {
            var test = new TestObject { Boolean = false };

            "test".IfNot(() => test.Boolean).ShouldEqual("test");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void IfNot_should_return_an_ArgumentException()
        {
            var test = new TestObject { Value = 1 };

            "test".IfNot(() => test.Value == 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void IfNot_should_also_return_an_ArgumentException()
        {
            var test = new TestObject { Boolean = true };

            "test".IfNot(() => test.Boolean && test.Boolean);
        }

        [Test]
        public void ValueOrDefault_should_return_null_if_the_root_is_null()
        {
            const TestObject nullTest = null;

            nullTest.ValueOrDefault(t => t.Child).ShouldBeNull();
        }

        [Test]
        public void ValueOrDefault_should_return_null_if_the_expression_results_in_null()
        {
            var test = new TestObject {Child = new TestObject()};

            test.ValueOrDefault(t => t.Child.Child).ShouldBeNull();
        }

        [Test]
        public void ValueOrDefault_should_return_the_result_of_the_expression_if_there_are_no_nulls()
        {
            var test = new TestObject { Child = new TestObject{Child = new TestObject()} };

            test.ValueOrDefault(t => t.Child.Child).ShouldNotBeNull();
        }

        [Test]
        public void ValueOrDefault_should_return_the_default_value_of_the_type_if_the_return_type_is_not_nullable_and_there_value_could_not_be_retrieved()
        {
            var test = new TestObject { Child = new TestObject() };

            test.ValueOrDefault(t => t.Child.Child.Value).ShouldEqual(0);
        }

        public class TestObject
        {
            public TestObject Child { get; set; }
            public int Value { get; set; }
            public bool Boolean { get; set; }
        }
    }
}