using System.Collections.Specialized;
using NUnit.Framework;
using FubuMVC.Core.Routing;

namespace FubuMVC.Tests.Routing
{
    [TestFixture]
    public class AggregateDictionaryTester
    {
        [Test]
        public void indxer_should_work_without_any_locators_registered()
        {
            var dict = new AggregateDictionary();

            dict["foo"].ShouldBeNull();
        }

        [Test]
        public void TryGetValue_should_work_without_any_locators_registered()
        {
            var dict = new AggregateDictionary();

            object value;

            dict.TryGetValue("foo", out value).ShouldBeFalse();

            value.ShouldBeNull();
        }

        [Test]
        public void indexer_should_find_value_with_one_locator_registered()
        {
            var values = new NameValueCollection { { "foo", "value" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values[s]);

            dict["foo"].ShouldEqual("value");
        }

        [Test]
        public void indexer_should_not_error_if_no_locator_finds_the_value()
        {
            var values1 = new NameValueCollection { { "foo", "value" } };
            var values2 = new NameValueCollection { { "bar", "baz" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values1[s]);
            dict.AddLocator(s => values2[s]);

            dict["zzzz"].ShouldBeNull();
        }

        [Test]
        public void indexer_should_find_value_with_multiple_locators_registered()
        {
            var values1 = new NameValueCollection { { "foo", "value" } };
            var values2 = new NameValueCollection { { "bar", "baz" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values1[s]);
            dict.AddLocator(s => values2[s]);

            dict["bar"].ShouldEqual("baz");
        }

        [Test]
        public void TryGetValue_should_find_value_with_one_locator_registered()
        {
            var values = new NameValueCollection { { "foo", "value" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values[s]);

            object value;
            dict.TryGetValue("foo", out value).ShouldBeTrue();
            value.ShouldEqual("value");
        }

        [Test]
        public void TryGetValue_should_not_error_if_no_locator_finds_the_value()
        {
            var values1 = new NameValueCollection { { "foo", "value" } };
            var values2 = new NameValueCollection { { "bar", "baz" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values1[s]);
            dict.AddLocator(s => values2[s]);

            object value;
            dict.TryGetValue("zzzz", out value).ShouldBeFalse();
            value.ShouldBeNull();
        }

        [Test]
        public void TryGetValue_should_find_value_with_multiple_locators_registered()
        {
            var values1 = new NameValueCollection { { "foo", "value" } };
            var values2 = new NameValueCollection { { "bar", "baz" } };
            var dict = new AggregateDictionary();

            dict.AddLocator(s => values1[s]);
            dict.AddLocator(s => values2[s]);

            object value;
            dict.TryGetValue("bar", out value).ShouldBeTrue();
            value.ShouldEqual("baz");
        }


    }
}