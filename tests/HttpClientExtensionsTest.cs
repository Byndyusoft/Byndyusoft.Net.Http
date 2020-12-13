using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Mocks;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace System.Net.Http
{
    public class HttpClientExtensionsTest
    {
        private readonly HttpClient _client;
        private readonly MediaTypeFormatter _formatter = new MockMediaTypeFormatter {CallBase = true};
        private readonly MediaTypeHeaderValue _mediaTypeHeader = MediaTypeHeaderValue.Parse("foo/bar; charset=utf-16");

        public HttpClientExtensionsTest()
        {
            var handlerMock = new Mock<TestableHttpMessageHandler> {CallBase = true};
            handlerMock
                .Setup(h => h.SendAsyncPublic(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Returns((HttpRequestMessage request, CancellationToken _) =>
                    Task.FromResult(new HttpResponseMessage {RequestMessage = request}));

            _client = new HttpClient(handlerMock.Object);
        }

        [Fact]
        public async Task PostAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PostAsync("http://www.example.com", new object(), new MockMediaTypeFormatter(),
                    "text/json"));

            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsync_String_WhenRequestUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PostAsync((string) null, new object(), new TestableMediaTypeFormatter(), "text/json"));

            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PostAsync_String_WhenRequestUriIsSet_CreatesRequestWithAppropriateUri()
        {
            _client.BaseAddress = new Uri("http://example.com/");

            var response = await _client.PostAsync("myapi/", new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Equal("http://example.com/myapi/", request.RequestUri.ToString());
        }

        [Fact]
        public async Task PostAsync_String_WhenAuthoritativeMediaTypeIsSet_CreatesRequestWithAppropriateContentType()
        {
            var response = await _client.PostAsync("http://example.com/myapi/", new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-16", request.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public async Task
            PostAsync_String_WhenAuthoritativeMediaTypeStringIsSet_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PostAsync("http://example.com/myapi/", new object(), _formatter, mediaType,
                CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task
            PostAsync_String_WhenAuthoritativeMediaTypeStringIsSetWithoutCT_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PostAsync("http://example.com/myapi/", new object(), _formatter, mediaType);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task PostAsync_String_WhenFormatterIsSet_CreatesRequestWithObjectContentAndCorrectFormatter()
        {
            var response = await _client.PostAsync("http://example.com/myapi/", new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            var content = Assert.IsType<ObjectContent<object>>(request.Content);
            Assert.Same(_formatter, content.Formatter);
        }

        [Fact]
        public async Task PostAsync_String_IssuesPostRequest()
        {
            var response = await _client.PostAsync("http://example.com/myapi/", new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Same(HttpMethod.Post, request.Method);
        }

        [Fact]
        public async Task PostAsync_String_WhenMediaTypeFormatterIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PostAsync("http://www.example.com", new object(), null));

            Assert.Equal("formatter", exception.ParamName);
        }

        [Fact]
        public async Task PutAsync_String_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsync("http://www.example.com", new object(), new MockMediaTypeFormatter(),
                    "text/json"));

            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsync_String_WhenRequestUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PutAsync((string) null, new object(), new TestableMediaTypeFormatter(), "text/json"));

            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsync_String_WhenRequestUriIsSet_CreatesRequestWithAppropriateUri()
        {
            _client.BaseAddress = new Uri("http://example.com/");

            var response = await _client.PutAsync("myapi/", new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Equal("http://example.com/myapi/", request.RequestUri.ToString());
        }

        [Fact]
        public async Task PutAsync_String_WhenAuthoritativeMediaTypeIsSet_CreatesRequestWithAppropriateContentType()
        {
            var response = await _client.PutAsync("http://example.com/myapi/", new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-16", request.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public async Task PutAsync_String_WhenFormatterIsSet_CreatesRequestWithObjectContentAndCorrectFormatter()
        {
            var response = await _client.PutAsync("http://example.com/myapi/", new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            var content = Assert.IsType<ObjectContent<object>>(request.Content);
            Assert.Same(_formatter, content.Formatter);
        }

        [Fact]
        public async Task
            PutAsync_String_WhenAuthoritativeMediaTypeStringIsSet_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PutAsync("http://example.com/myapi/", new object(), _formatter, mediaType,
                CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task
            PutAsync_String_WhenAuthoritativeMediaTypeStringIsSetWithoutCT_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PutAsync("http://example.com/myapi/", new object(), _formatter, mediaType);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task PutAsync_String_IssuesPutRequest()
        {
            var response = await _client.PutAsync("http://example.com/myapi/", new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Same(HttpMethod.Put, request.Method);
        }

        [Fact]
        public async Task PutAsync_String_WhenMediaTypeFormatterIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PutAsync("http://www.example.com", new object(), null));

            Assert.Equal("formatter", exception.ParamName);
        }


        [Fact]
        public async Task PostAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsync("http://www.example.com", new object(), new MockMediaTypeFormatter(),
                    "text/json"));

            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PostAsync_Uri_WhenRequestUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PostAsync((Uri) null, new object(), new TestableMediaTypeFormatter(), "text/json"));

            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PostAsync_Uri_WhenRequestUriIsSet_CreatesRequestWithAppropriateUri()
        {
            _client.BaseAddress = new Uri("http://example.com/");

            var response = await _client.PostAsync(new Uri("myapi/", UriKind.Relative), new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Equal("http://example.com/myapi/", request.RequestUri.ToString());
        }

        [Fact]
        public async Task PostAsync_Uri_WhenAuthoritativeMediaTypeIsSet_CreatesRequestWithAppropriateContentType()
        {
            var response = await _client.PostAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-16", request.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public async Task PostAsync_Uri_WhenFormatterIsSet_CreatesRequestWithObjectContentAndCorrectFormatter()
        {
            var response = await _client.PostAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            var content = Assert.IsType<ObjectContent<object>>(request.Content);
            Assert.Same(_formatter, content.Formatter);
        }

        [Fact]
        public async Task PostAsync_Uri_WhenAuthoritativeMediaTypeStringIsSet_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PostAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                mediaType, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task
            PostAsync_Uri_WhenAuthoritativeMediaTypeStringIsSetWithoutCT_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PostAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                mediaType);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task PostAsync_Uri_IssuesPostRequest()
        {
            var response = await _client.PostAsync(new Uri("http://example.com/myapi/"), new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Same(HttpMethod.Post, request.Method);
        }

        [Fact]
        public async Task PostAsync_Uri_WhenMediaTypeFormatterIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PostAsync(new Uri("http://example.com"), new object(), null));

            Assert.Equal("formatter", exception.ParamName);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenClientIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ((HttpClient) null).PutAsync(new Uri("http://www.example.com"), new object(),
                    new MockMediaTypeFormatter(), "text/json"));

            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenRequestUriIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _client.PutAsync((Uri) null, new object(), new TestableMediaTypeFormatter(), "text/json"));

            Assert.Equal(
                "An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.",
                exception.Message);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenRequestUriIsSet_CreatesRequestWithAppropriateUri()
        {
            _client.BaseAddress = new Uri("http://example.com/");

            var response = await _client.PutAsync(new Uri("myapi/", UriKind.Relative), new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Equal("http://example.com/myapi/", request.RequestUri.ToString());
        }

        [Fact]
        public async Task PutAsync_Uri_WhenAuthoritativeMediaTypeIsSet_CreatesRequestWithAppropriateContentType()
        {
            var response = await _client.PutAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
            Assert.Equal("utf-16", request.Content.Headers.ContentType.CharSet);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenFormatterIsSet_CreatesRequestWithObjectContentAndCorrectFormatter()
        {
            var response = await _client.PutAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                _mediaTypeHeader, CancellationToken.None);

            var request = response.RequestMessage;
            var content = Assert.IsType<ObjectContent<object>>(request.Content);
            Assert.Same(_formatter, content.Formatter);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenAuthoritativeMediaTypeStringIsSet_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PutAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                mediaType, CancellationToken.None);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task
            PutAsync_Uri_WhenAuthoritativeMediaTypeStringIsSetWithoutCT_CreatesRequestWithAppropriateContentType()
        {
            var mediaType = _mediaTypeHeader.MediaType;
            var response = await _client.PutAsync(new Uri("http://example.com/myapi/"), new object(), _formatter,
                mediaType);

            var request = response.RequestMessage;
            Assert.Equal("foo/bar", request.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task PutAsync_Uri_IssuesPutRequest()
        {
            var response = await _client.PutAsync(new Uri("http://example.com/myapi/"), new object(), _formatter);

            var request = response.RequestMessage;
            Assert.Same(HttpMethod.Put, request.Method);
        }

        [Fact]
        public async Task PutAsync_Uri_WhenMediaTypeFormatterIsNull_ThrowsException()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _client.PutAsync(new Uri("http://example.com"), new object(), null));

            Assert.Equal("formatter", exception.ParamName);
        }
    }
}