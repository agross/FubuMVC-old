using System.Collections.Generic;
using FubuMVC.Core.Config;
using FubuMVC.Core.Runtime;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Runtime
{
    [TestFixture]
    public class FubuRouteHandlerTester
    {
        private FubuRouteHandler _handler;
        private UrlAction _action;
        private IActionInvoker _invoker;
        private ICurrentAction _currentAction;
        private IRequestDictionaryHandler _dictHandler;

        [SetUp]
        public void SetUp()
        {
            _action = new UrlAction();
            _invoker = MockRepository.GenerateStub<IActionInvoker>();
            _currentAction = MockRepository.GenerateStub<ICurrentAction>();
            _dictHandler = MockRepository.GenerateStub<IRequestDictionaryHandler>();
            _handler = new FubuRouteHandler(_action, _invoker, _currentAction, _dictHandler);
        }

        [Test]
        public void should_set_the_current_action()
        {
            _handler.GetHttpHandler(null);

            _currentAction.Current.ShouldBeTheSameAs(_action);
        }

        [Test]
        public void should_generate_dictionary_from_request_context()
        {
            _handler.GetHttpHandler(null);

            _dictHandler.AssertWasCalled(d => d.GetDictionary(null));
        }

        [Test]
        public void should_pass_dictionary_to_invoker()
        {
            var dict = new Dictionary<string, object>();
            _dictHandler.Expect(d => d.GetDictionary(null)).Return(dict);

            _handler.GetHttpHandler(null).ProcessRequest(null);

            _invoker.AssertWasCalled(i => i.Invoke(dict));
        }
    }
}