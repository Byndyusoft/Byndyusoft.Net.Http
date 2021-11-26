using Moq;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Mocks;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace System.Net.Http
{
    public class ObjectContentTests
    {
        private readonly MediaTypeFormatter _formatter = new TestableMediaTypeFormatter();
        private readonly MediaTypeHeaderValue _jsonHeaderValue = new MediaTypeHeaderValue("application/json");
        private readonly object _value = new object();

        public static TheoryDataSet<Type, object?> ValidValueTypePairs =>
            new TheoryDataSet<Type, object?>
            {
                {typeof(int?), null},
                {typeof(string), null},
                {typeof(int), 42},
                {typeof(object), "abc"},
                {typeof(string), "abc"},
                {typeof(IList<string>), new List<string>()}
            };

        [Fact]
        public void Constructor_WhenTypeArgumentIsNull_ThrowsEsxception()
        {
            Func<ObjectContent>[] actions =
            {
                () => new ObjectContent(null!, _value, _formatter),
                () => new ObjectContent(null!, _value, _formatter, "foo/bar"),
                () => new ObjectContent(null!, _value, _formatter, _jsonHeaderValue)
            };

            foreach (var action in actions)
            {
                var exception = Assert.Throws<ArgumentNullException>(action);
                Assert.Equal("type", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_WhenFormatterArgumentIsNull_ThrowsEsxception()
        {
            Func<ObjectContent>[] actions =
            {
                () => new ObjectContent(typeof(object), _value, null!),
                () => new ObjectContent(typeof(object), _value, null!, "foo/bar"),
                () => new ObjectContent(typeof(object), _value, null!, _jsonHeaderValue)
            };

            foreach (var action in actions)
            {
                var exception = Assert.Throws<ArgumentNullException>(action);
                Assert.Equal("formatter", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_WhenValueIsNullAndTypeIsNotCompatible_ThrowsException()
        {
            var exception =
                Assert.Throws<InvalidOperationException>(() => new ObjectContent(typeof(int), null, _formatter));

            Assert.Equal("The 'ObjectContent' type cannot accept a null value for the value type 'Int32'.",
                exception.Message);
        }

        [Fact]
        public void Constructor_WhenValueIsNotNullButTypeDoesNotMatch_ThrowsException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ObjectContent(typeof(IList<string>),
                new Dictionary<string, string>(), _formatter));

            Assert.Equal("value", exception.ParamName);
            Assert.Equal(
                "An object of type 'Dictionary`2' cannot be used with a type parameter of 'IList`1'. (Parameter 'value')",
                exception.Message);
        }

        [Fact]
        public void Constructor_WhenValueIsNotSupportedByFormatter_ThrowsException()
        {
            var formatterMock = new Mock<MediaTypeFormatter>();
            formatterMock.Setup(f => f.CanWriteType(typeof(List<string>))).Returns(false).Verifiable();

            var exception =
                Assert.Throws<InvalidOperationException>(() =>
                    new ObjectContent(typeof(List<string>), new List<string>(), formatterMock.Object));

            Assert.Equal(
                "The configured formatter 'Castle.Proxies.MediaTypeFormatterProxy' cannot write an object of type 'List`1'.",
                exception.Message);
            formatterMock.Verify();
        }

        [Fact]
        public void Constructor_Tests()
        {
            var contentType = "application/json";

            var content = new ObjectContent(typeof(object), _value, _formatter, contentType);

            Assert.Same(_formatter, content.Formatter);
            Assert.Same(_value, content.Value);
            Assert.Same(typeof(object), content.ObjectType);
            Assert.Equal(MediaTypeHeaderValue.Parse(contentType), content.Headers.ContentType);
        }

        [Fact]
        public void Value_Setter_Test()
        {
            var newValue = "newValue";
            var content = new ObjectContent(typeof(object), _value, _formatter);

            content.Value = newValue;

            Assert.Same(newValue, content.Value);
        }

        [Fact]
        public void Constructor_CallsFormattersGetDefaultContentHeadersMethod()
        {
            var formatterMock = new Mock<MediaTypeFormatter>();
            formatterMock.Setup(f => f.CanWriteType(typeof(string))).Returns(true);

            var content = new ObjectContent(typeof(string), "", formatterMock.Object, _jsonHeaderValue);

            formatterMock.Verify(f => f.SetDefaultContentHeaders(typeof(string), content.Headers, _jsonHeaderValue),
                Times.Once());
        }

        [Theory]
        [MemberData(nameof(ValidValueTypePairs))]
        public void Constructor_WhenValueAndTypeAreCompatible_SetsValue(Type type, object value)
        {
            var oc = new ObjectContent(type, value, _formatter);

            Assert.Same(value, oc.Value);
            Assert.Equal(type, oc.ObjectType);
        }

        [Fact]
        public void Constructor_WhenTypeIsNotSupportedByFormatter_ThrowsException()
        {
            var formatterMock = new Mock<MediaTypeFormatter>();
            formatterMock.Setup(f => f.CanWriteType(typeof(string))).Returns(true);
            formatterMock.Setup(f => f.CanWriteType(typeof(object))).Returns(false).Verifiable();

            var exception =
                Assert.Throws<InvalidOperationException>(() =>
                    new ObjectContent(typeof(object), "", formatterMock.Object));

            Assert.Equal(
                "The configured formatter 'Castle.Proxies.MediaTypeFormatterProxy' cannot write an object of type 'Object'.",
                exception.Message);
            formatterMock.Verify();
        }

        [Fact]
        public void SerializeToStreamAsync_CallsUnderlyingFormatter()
        {
            var stream = Stream.Null;
            var context = new Mock<TransportContext>().Object;
            var formatterMock = new Mock<TestableMediaTypeFormatter> { CallBase = true };
            var oc = new TestableObjectContent(typeof(string), "abc", formatterMock.Object);
            var task = new Task(() => { });
            formatterMock.Setup(f => f.WriteToStreamAsync(typeof(string), "abc", stream, oc, context, CancellationToken.None))
                .Returns(task).Verifiable();

            var result = oc.CallSerializeToStreamAsync(stream, context);

            Assert.Same(task, result);
            formatterMock.VerifyAll();
        }

        [Fact]
        public void TryComputeLength_ReturnsFalseAnd0()
        {
            var oc = new TestableObjectContent(typeof(string), null!, _formatter);

            var result = oc.CallTryComputeLength(out var length);

            Assert.False(result);
            Assert.Equal(-1, length);
        }

        [Fact]
        public async Task DisposeAsync()
        {
            var content = new ObjectContent(typeof(object), _value, _formatter);

            await content.DisposeAsync();
        }
    }
}