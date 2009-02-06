using NUnit.Framework;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class ControllerActionConfigTester
    {
        private ControllerActionConfig _config;

        [SetUp]
        public void SetUp()
        {
            _config =
                ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                    (c, i) => c.SomeAction(i), null);
        }

        [Test]
        public void key_should_a_unique_guid()
        {
            _config.UniqueID.ShouldNotEqual(
                ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                    (c, i) => c.SomeAction(i), null).UniqueID);
        }

        [Test]
        public void should_create_a_new_list_of_behaviors_for_that_controller_action()
        {
            _config.GetBehaviors().ShouldNotBeNull();
        }

        [Test]
        public void IsTheSameActionAs_should_be_false_if_the_method_is_not_OMIOMO()
        {
            _config.IsTheSameActionAs<string>(s => s.Clone()).ShouldBeFalse();
        }

        [Test]
        public void IsTheSameActionAs_should_be_false_if_the_method_signature_does_not_match()
        {
            _config.IsTheSameActionAs<TestController>(c => c.SomeAction(0))
                .ShouldBeFalse();
        }

        [Test]
        public void IsTheSameActionAs_should_compare_action_expression_and_return_true_if_they_represent_the_same_action()
        {
            _config.IsTheSameActionAs<TestController>(c => c.SomeAction(null))
                .ShouldBeTrue();
        }

        [Test]
        public void IsTheSameActionAs_should_compare_action_expression_and_return_false_if_they_do_not_represent_the_same_action()
        {
            _config.IsTheSameActionAs<TestController>(c => c.AnotherAction(null))
                .ShouldBeFalse();
        }

        [Test]
        public void IsTheSameActionAs_should_compare_other_config_and_return_true_if_they_represent_the_same_action()
        {
            var otherConfig = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i), null);

            _config.IsTheSameActionAs(otherConfig).ShouldBeTrue(); ;
        }

        [Test]
        public void IsTheSameActionAs_should_compare_other_config_and_return_false_if_they_do_not_represent_the_same_action()
        {
            var otherConfig = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>((c, i) => c.AnotherAction(i), null);

            _config.IsTheSameActionAs(otherConfig).ShouldBeFalse(); ;
        }

        [Test]
        public void ApplyDefaultBehaviors_should_add_the_list_of_default_behaviors_to_the_list()
        {
            _config.ApplyDefaultBehaviors(new[]{typeof(TestBehavior), typeof(TestBehavior2)});

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_add_new_behaviors_after_default_ones()
        {
            _config.ApplyDefaultBehaviors(new[] { typeof(TestBehavior) });

            _config.AddBehavior<TestBehavior2>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_remember_behaviors_for_a_specified_route()
        {
            _config.AddBehavior<TestBehavior>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior));
        }

        [Test]
        public void should_remember_order_of_behaviors_for_a_specified_route()
        {
            _config.AddBehavior<TestBehavior>();
            _config.AddBehavior<TestBehavior2>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_remove_specified_behavior_for_route()
        {
            _config.AddBehavior<TestBehavior>();
            _config.AddBehavior<TestBehavior2>();
            _config.RemoveBehavior<TestBehavior>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2));
        }

        [Test]
        public void should_remove_all_behaviors_for_route()
        {
            _config.AddBehavior<TestBehavior>();
            _config.AddBehavior<TestBehavior2>();
            _config.RemoveAllBehaviors();

            _config.GetBehaviors().ShouldHaveCount(0);
        }

        [Test]
        public void should_add_other_urls()
        {
            var url = "url";
            _config.AddOtherUrl(url);
            _config.GetOtherUrls().ShouldHaveTheSameElementsAs(url);
        }

        [Test]
        public void should_not_add_dupe_urls()
        {
            var url = "url";
            _config.AddOtherUrl(url);
            _config.AddOtherUrl(url);
            _config.GetOtherUrls().ShouldHaveTheSameElementsAs(url);   
        }

        [Test]
        public void should_remove_other_urls()
        {
            var url1 = "url1";
            var url2 = "url2";
            _config.AddOtherUrl(url1);
            _config.AddOtherUrl(url2);

            _config.RemoveOtherUrl(url2);

            _config.GetOtherUrls().ShouldHaveTheSameElementsAs(url1);
        }
    }
}
