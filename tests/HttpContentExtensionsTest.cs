using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Mocks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Net.Http
{
    public class HttpContentExtensionsTest
    {
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly Mock<MediaTypeFormatter> _formatterMock = new Mock<MediaTypeFormatter> { CallBase = true };
        private readonly MediaTypeFormatter[] _formatters;
        private readonly MediaTypeHeaderValue _mediaType = new MediaTypeHeaderValue("foo/bar");

        public HttpContentExtensionsTest()
        {
            _formatterMock.Object.SupportedMediaTypes.Add(_mediaType);
            _formatters = new[] { _formatterMock.Object };
        }

        [Fact]
        public async Task ReadAsAsync_WhenContentParameterIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => HttpContentExtensions.ReadAsAsync(null!, typeof(string), Enumerable.Empty<MediaTypeFormatter>(),
                    _cancellationToken));

            Assert.Equal("content", exception.ParamName);
        }

        [Fact]
        public async Task ReadAsAsync_WhenTypeParameterIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                new StringContent("").ReadAsAsync(null!, Enumerable.Empty<MediaTypeFormatter>(), _cancellationToken));

            Assert.Equal("type", exception.ParamName);
        }

        [Fact]
        public async Task ReadAsAsync_WhenFormattersParameterIsNull_Throws()
        {
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    new StringContent("").ReadAsAsync(typeof(string), null!, _cancellationToken));

            Assert.Equal("formatters", exception.ParamName);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_WhenContentParameterIsNull_Throws()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                HttpContentExtensions.ReadAsAsync<string>(null!, Enumerable.Empty<MediaTypeFormatter>(),
                    _cancellationToken));

            Assert.Equal("content", exception.ParamName);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_WhenFormattersParameterIsNull_Throws()
        {
            var exception =
                await Assert.ThrowsAsync<ArgumentNullException>(() =>
                    new StringContent("").ReadAsAsync<string>(null!, _cancellationToken));

            Assert.Equal("formatters", exception.ParamName);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_WhenNoMatchingFormatterFound_Throws()
        {
            var content = new StringContent("{}");
            content.Headers.ContentType = _mediaType;
            content.Headers.ContentType.CharSet = "utf-16";
            var formatters = new MediaTypeFormatter[] { new MockMediaTypeFormatter { CallBase = true } };

            var exception = await
                Assert.ThrowsAsync<UnsupportedMediaTypeException>(() =>
                    content.ReadAsAsync<List<string>>(formatters, _cancellationToken));
            Assert.Equal(
                "No MediaTypeFormatter is available to read an object of type 'List`1' from content with media type 'foo/bar'.",
                exception.Message);
            Assert.Equal(_mediaType, exception.MediaType);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_WhenTypeIsReferenceTypeAndNoMediaType_Throws()
        {
            var content = new StringContent("{}");
            content.Headers.ContentType = null;
            var formatters = new MediaTypeFormatter[] { new MockMediaTypeFormatter { CallBase = true } };

            var exception = await
                Assert.ThrowsAsync<UnsupportedMediaTypeException>(() =>
                    content.ReadAsAsync<List<string>>(formatters, _cancellationToken));
            Assert.Equal(
                "No MediaTypeFormatter is available to read an object of type 'List`1' from content with media type 'application/octet-stream'.",
                exception.Message);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_WhenTypeIsValueTypeAndNoMediaType_Throws()
        {
            var content = new StringContent("123456");
            content.Headers.ContentType = null;
            var formatters = new MediaTypeFormatter[] { new MockMediaTypeFormatter { CallBase = true } };

            var exception = await
                Assert.ThrowsAsync<UnsupportedMediaTypeException>(() =>
                    content.ReadAsAsync<int>(formatters, _cancellationToken));
            Assert.Equal(
                "No MediaTypeFormatter is available to read an object of type 'Int32' from content with media type 'application/octet-stream'.",
                exception.Message);
        }

        [Fact]
        public async Task ReadAsAsyncOfT_ReadsFromContent_ThenInvokesFormattersReadFromStreamMethod()
        {
            Stream? contentStream = null;
            var value = "42";
            var contentMock = new Mock<TestableHttpContent> { CallBase = true };
            contentMock.Setup(c => c.SerializeToStreamAsyncPublic(It.IsAny<Stream>(), It.IsAny<TransportContext>()))
                .Returns(Task.CompletedTask)
                .Callback((Stream s, TransportContext _) => contentStream = s)
                .Verifiable();
            HttpContent content = contentMock.Object;
            content.Headers.ContentType = _mediaType;
            _formatterMock
                .Setup(f => f.ReadFromStreamAsync(typeof(string), It.IsAny<Stream>(), It.IsAny<HttpContent>(),
                    It.IsAny<IFormatterLogger>(), _cancellationToken))
                .Returns(Task.FromResult<object?>(value));
            _formatterMock.Setup(f => f.CanReadType(typeof(string))).Returns(true);

            var resultValue = await content.ReadAsAsync<string>(_formatters, _cancellationToken);

            Assert.Same(value, resultValue);
            contentMock.Verify();
            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(typeof(string), contentStream!, content, null, _cancellationToken),
                Times.Once());
        }

        [Fact]
        public async Task ReadAsAsyncOfT_InvokesFormatterEvenIfContentLengthIsZero()
        {
            var content = new StringContent("");
            _formatterMock.Setup(f => f.CanReadType(typeof(string))).Returns(true);
            _formatterMock.Object.SupportedMediaTypes.Add(content.Headers.ContentType);
            _formatterMock
                .Setup(f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(), It.IsAny<HttpContent>(),
                    It.IsAny<IFormatterLogger>(), _cancellationToken))
                .Returns(Task.FromResult<object?>(null));

            await content.ReadAsAsync<string>(_formatters, _cancellationToken);

            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(typeof(string), It.IsAny<Stream>(), content, It.IsAny<IFormatterLogger>(),
                    _cancellationToken),
                Times.Once());
        }

        [Fact]
        public async Task ReadAsAsync_WhenContentIsObjectContentAndValueIsCompatibleType_ReadsValueFromObjectContent()
        {
            _formatterMock.Setup(f => f.CanWriteType(typeof(TestClass))).Returns(true);
            var value = new TestClass();
            var content = new ObjectContent<TestClass>(value, _formatterMock.Object);

            Assert.Same(value, await content.ReadAsAsync<object>(_formatters, _cancellationToken));
            Assert.Same(value, await content.ReadAsAsync<TestClass>(_formatters, _cancellationToken));
            Assert.Same(value, await content.ReadAsAsync(typeof(object), _formatters, _cancellationToken));
            Assert.Same(value, await content.ReadAsAsync(typeof(TestClass), _formatters, _cancellationToken));

            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(), content, It.IsAny<IFormatterLogger>(),
                    _cancellationToken),
                Times.Never());
        }

        [Fact]
        public async Task
            ReadAsAsync_WhenContentIsObjectContentAndValueIsNull_IfTypeIsNullable_SerializesAndDeserializesValue()
        {
            _formatterMock.Setup(f => f.CanWriteType(typeof(object))).Returns(true);
            _formatterMock.Setup(f => f.CanReadType(It.IsAny<Type>())).Returns(true);
            var content = new ObjectContent<object>(null!, _formatterMock.Object);
            SetupUpRoundTripSerialization(type => null!);

            Assert.Null(await content.ReadAsAsync<object>(_formatters, _cancellationToken));
            Assert.Null(await content.ReadAsAsync<TestClass>(_formatters, _cancellationToken));
            Assert.Null(await content.ReadAsAsync<int?>(_formatters, _cancellationToken));
            Assert.Null(await content.ReadAsAsync(typeof(object), _formatters, _cancellationToken));
            Assert.Null(await content.ReadAsAsync(typeof(TestClass), _formatters, _cancellationToken));
            Assert.Null(await content.ReadAsAsync(typeof(int?), _formatters, _cancellationToken));

            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(), content, It.IsAny<IFormatterLogger>(),
                    _cancellationToken),
                Times.Exactly(6));
        }

        [Fact]
        public async Task
            ReadAsAsync_WhenContentIsObjectContentAndValueIsNull_IfTypeIsNotNullable_SerializesAndDeserializesValue()
        {
            _formatterMock.Setup(f => f.CanWriteType(typeof(object))).Returns(true);
            _formatterMock.Setup(f => f.CanReadType(typeof(int))).Returns(true);
            var content = new ObjectContent<object>(null!, _formatterMock.Object, _mediaType);
            SetupUpRoundTripSerialization();

            Assert.IsType<int>(await content.ReadAsAsync<int>(_formatters, _cancellationToken));
            Assert.IsType<int>(await content.ReadAsAsync(typeof(int), _formatters, _cancellationToken));

            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(), content, It.IsAny<IFormatterLogger>(),
                    _cancellationToken),
                Times.Exactly(2));
        }

        [Fact]
        public async Task
            ReadAsAsync_WhenContentIsObjectContentAndValueIsNotCompatibleType_SerializesAndDeserializesValue()
        {
            _formatterMock.Setup(f => f.CanWriteType(typeof(TestClass))).Returns(true);
            _formatterMock.Setup(f => f.CanReadType(typeof(string))).Returns(true);
            var value = new TestClass();
            var content = new ObjectContent<TestClass>(value, _formatterMock.Object, _mediaType);
            SetupUpRoundTripSerialization(type => new TestClass());

            await Assert.ThrowsAsync<InvalidCastException>(() =>
                content.ReadAsAsync<string>(_formatters, _cancellationToken));

            Assert.IsNotType<string>(await content.ReadAsAsync(typeof(string), _formatters, _cancellationToken));

            _formatterMock.Verify(
                f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(), content, It.IsAny<IFormatterLogger>(),
                    _cancellationToken),
                Times.Exactly(2));
        }

        [Fact]
        public async Task ReadAsAsync_type_cancellationToken_PassesCancellationTokenFurther()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            HttpContent content = new StringContent("42", Encoding.Default, "application/json");

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                content.ReadAsAsync(typeof(int), new MediaTypeFormatter[] { new TestableMediaTypeFormatter() },
                    cts.Token));
        }

        [Fact]
        public async Task ReadAsAsync_type_formatters_cancellationToken_PassesCancellationTokenFurther()
        {
            // Arrange
            Stream stream = new MemoryStream();
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/test");
            var token = new CancellationToken();
            var formatter = new Mock<MediaTypeFormatter>(MockBehavior.Strict);
            formatter.Object.SupportedMediaTypes.Add(content.Headers.ContentType);
            formatter.Setup(f => f.CanReadType(typeof(int))).Returns(true);
            formatter
                .Setup(f => f.ReadFromStreamAsync(typeof(int), It.IsAny<Stream>(), content, null, token))
                .Returns(Task.FromResult<object?>(42))
                .Verifiable();

            // Act
            await content.ReadAsAsync(typeof(int), new[] { formatter.Object }, token);

            // Assert
            formatter.Verify();
        }

        [Fact]
        public async Task ReadAsAsync_type_formatters_formatterLogger_cancellationToken_PassesCancellationTokenFurther()
        {
            // Arrange
            Stream stream = new MemoryStream();
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/test");
            var token = new CancellationToken();
            var formatterLogger = new Mock<IFormatterLogger>().Object;

            var formatter = new Mock<MediaTypeFormatter>(MockBehavior.Strict);
            formatter.Object.SupportedMediaTypes.Add(content.Headers.ContentType);
            formatter.Setup(f => f.CanReadType(typeof(int))).Returns(true);
            formatter
                .Setup(f => f.ReadFromStreamAsync(typeof(int), It.IsAny<Stream>(), content, formatterLogger, token))
                .Returns(Task.FromResult<object?>(42))
                .Verifiable();

            // Act
            await content.ReadAsAsync(typeof(int), new[] { formatter.Object }, formatterLogger, token);

            // Assert
            formatter.Verify();
        }

        [Fact]
        public Task ReadAsAsyncOfT_cancellationToken_PassesCancellationTokenFurther()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            HttpContent content = new StringContent("42", Encoding.Default, "application/json");

            return Assert.ThrowsAsync<OperationCanceledException>(() =>
                content.ReadAsAsync<int>(new MediaTypeFormatter[] { new TestableMediaTypeFormatter() }, cts.Token));
        }

        [Fact]
        public async Task ReadAsAsyncOfT_formatters_cancellationToken_PassesCancellationTokenFurther()
        {
            // Arrange
            Stream stream = new MemoryStream();
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/test");
            var token = new CancellationToken();
            var formatter = new Mock<MediaTypeFormatter>(MockBehavior.Strict);
            formatter.Object.SupportedMediaTypes.Add(content.Headers.ContentType);
            formatter.Setup(f => f.CanReadType(typeof(int))).Returns(true);
            formatter
                .Setup(f => f.ReadFromStreamAsync(typeof(int), It.IsAny<Stream>(), content, null, token))
                .Returns(Task.FromResult<object?>(42))
                .Verifiable();

            // Act
            await content.ReadAsAsync<int>(new[] { formatter.Object }, token);

            // Assert
            formatter.Verify();
        }

        [Fact]
        public async Task ReadAsAsyncOfT_formatters_formatterLogger_cancellationToken_PassesCancellationTokenFurther()
        {
            // Arrange
            Stream stream = new MemoryStream();
            HttpContent content = new StreamContent(stream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/test");
            var token = new CancellationToken();
            var formatterLogger = new Mock<IFormatterLogger>().Object;

            var formatter = new Mock<MediaTypeFormatter>(MockBehavior.Strict);
            formatter.Object.SupportedMediaTypes.Add(content.Headers.ContentType);
            formatter.Setup(f => f.CanReadType(typeof(int))).Returns(true);
            formatter
                .Setup(f => f.ReadFromStreamAsync(typeof(int), It.IsAny<Stream>(), content, formatterLogger, token))
                .Returns(Task.FromResult<object?>(42))
                .Verifiable();

            // Act
            await content.ReadAsAsync<int>(new[] { formatter.Object }, formatterLogger, token);

            // Assert
            formatter.Verify();
        }

        private void SetupUpRoundTripSerialization(Func<Type, object?>? factory = null)
        {
            factory ??= Activator.CreateInstance;
            _formatterMock.Setup(f => f.WriteToStreamAsync(It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<Stream>(),
                    It.IsAny<HttpContent>(), It.IsAny<TransportContext>(), _cancellationToken))
                .Returns(Task.CompletedTask);
            _formatterMock.Setup(f => f.ReadFromStreamAsync(It.IsAny<Type>(), It.IsAny<Stream>(),
                    It.IsAny<HttpContent>(), It.IsAny<IFormatterLogger>(), _cancellationToken))
                .Returns<Type, Stream, HttpContent, IFormatterLogger, CancellationToken>(
                    (type, stream, content, logger, ct) =>
                        Task.FromResult(factory(type)));
        }

        public class TestClass
        {
        }

        public abstract class TestableHttpContent : HttpContent
        {
            protected override Task<Stream> CreateContentReadStreamAsync()
            {
                return CreateContentReadStreamAsyncPublic();
            }

            public virtual Task<Stream> CreateContentReadStreamAsyncPublic()
            {
                return base.CreateContentReadStreamAsync();
            }

            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                return SerializeToStreamAsyncPublic(stream, context);
            }

            public abstract Task SerializeToStreamAsyncPublic(Stream stream, TransportContext context);

            protected override bool TryComputeLength(out long length)
            {
                return TryComputeLengthPublic(out length);
            }

            public abstract bool TryComputeLengthPublic(out long length);
        }
    }
}