using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Runtime.Invokers;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Runtime.Invokers
{
    [TestFixture]
    public class PureBehaviorActionInvokerTester
    {
        private PureBehaviorActionInvoker<TestInputModel> _invoker;
        private IDictionaryConverter<TestInputModel> _converter;
        private IActionBehavior _behavior;
        private ICollection<ConvertProblem> _problems;

        [SetUp]
        public void SetUp()
        {
            _converter = MockRepository.GenerateStub<IDictionaryConverter<TestInputModel>>();
            _behavior = MockRepository.GenerateStub<IActionBehavior>();
            _invoker = new PureBehaviorActionInvoker<TestInputModel>(_converter, _behavior);
        }

        [Test]
        public void should_use_the_converter_to_convert_from_input()
        {
            var dict = new Dictionary<string, object>{{"PropInf", "99"}};
            
            _invoker.Invoke(dict);

            _converter.AssertWasCalled(c => c.ConvertFrom(dict, out _problems));
        }

        [Test]
        public void should_invoke_behavior_with_newly_converted_item()
        {
            var item = new TestInputModel();
            _converter.Stub(c => c.ConvertFrom(null, out _problems)).IgnoreArguments().Return(item);
            
            _invoker.Invoke(null);

            _behavior.AssertWasCalled(b => b.Invoke(item));
        }
    }
}