using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Mocks
{
    public class TestableMediaTypeFormatter : MediaTypeFormatter
    {
        public TestableMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override Task WriteToStreamAsync(Type type, object? value, Stream stream, HttpContent? content,
            TransportContext? transportContext, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}