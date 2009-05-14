using System.Web;
using FubuMVC.Core.SessionState;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.SessionState
{
    [TestFixture]
    public class RequestDataProviderTester
    {
        private HttpContextBase _httpContext;
        private HttpSessionStateBase _sessionState;
        private RequestDataProvider _requestDataProvider;

        [SetUp]
        public void Setup_Test()
        {
            _httpContext = MockRepository.GenerateMock<HttpContextBase>();
            _sessionState = MockRepository.GenerateMock<HttpSessionStateBase>();
        }

        [Test]
        public void Should_do_nothing_when_trying_to_set_the_SessionState_when_the_current_session_is_null()
        {
            _httpContext.Stub(x => x.Session).Return(null);

            _requestDataProvider = new RequestDataProvider(_httpContext);

            _requestDataProvider.SetRequestData("This is My Data");

            _requestDataProvider.GetRequestData().ShouldBeNull();

            _httpContext.VerifyAllExpectations();
        }

        [Test]
        public void Should_be_able_to_store_and_retrieve_value_when_Session_is_not_null()
        {
            string dataToStoreInSession = "This is some test data";

            _httpContext.Stub(x => x.Session).Return(_sessionState);
            _sessionState.Expect(c => c.Add(RequestDataProvider.REQUESTDATA_KEY, dataToStoreInSession));
            _sessionState.Expect(c => c[RequestDataProvider.REQUESTDATA_KEY]).Return(dataToStoreInSession);

            _requestDataProvider = new RequestDataProvider(_httpContext);
            _requestDataProvider.SetRequestData(dataToStoreInSession);

            _requestDataProvider.GetRequestData().ShouldEqual(dataToStoreInSession);
            _httpContext.VerifyAllExpectations();
            _sessionState.VerifyAllExpectations();
        }

        [Test]
        public void Should_return_a_null_value_on_subsequent_call_if_no_new_data_was_set()
        {
            string dataToStoreInSession = "This is some test data";

            _httpContext.Stub(x => x.Session).Return(_sessionState);

            _sessionState.Expect(c => c.Add(RequestDataProvider.REQUESTDATA_KEY, dataToStoreInSession));
            _sessionState.Expect(c => c[RequestDataProvider.REQUESTDATA_KEY]).Return(dataToStoreInSession);

            _requestDataProvider = new RequestDataProvider(_httpContext);

            _requestDataProvider.SetRequestData(dataToStoreInSession);


            _requestDataProvider.GetRequestData().ShouldEqual(dataToStoreInSession);
            _requestDataProvider.GetRequestData().ShouldBeNull();
            _httpContext.VerifyAllExpectations();
            _sessionState.VerifyAllExpectations();
        }
    }

}
