using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace System.Net.Http.Mocks
{
    public class TestableObjectContent : ObjectContent
    {
        public TestableObjectContent(Type type, object value, MediaTypeFormatter formatter)
            : base(type, value, formatter)
        {
        }

        public bool CallTryComputeLength(out long length)
        {
            return TryComputeLength(out length);
        }

        public Task CallSerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return SerializeToStreamAsync(stream, context);
        }
    }
}