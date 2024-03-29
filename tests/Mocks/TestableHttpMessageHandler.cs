using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Mocks
{
    public class TestableHttpMessageHandler : HttpMessageHandler
    {
        public virtual Task<HttpResponseMessage> SendAsyncPublic(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return SendAsyncPublic(request, cancellationToken);
        }
    }
}