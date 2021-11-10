using Moq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Xunit;

namespace System.Net.Http
{
    public class ObjectContentOfTTests
    {
        private readonly MediaTypeHeaderValue _jsonHeaderValue = new MediaTypeHeaderValue("application/json");
        private readonly MediaTypeFormatter _formatter;

        public ObjectContentOfTTests()
        {
            var formatterMock = new Mock<MediaTypeFormatter>{CallBase = true};
            formatterMock.Setup(f => f.CanWriteType(It.IsAny<Type>())).Returns(true);
            _formatter = formatterMock.Object;
        }

        [Fact]
        public void Constructor_WhenFormatterParameterIsNull_Throws()
        {
            Func<ObjectContent<string>>[] actions =
            {
                () => new ObjectContent<string>("", null!),
                () => new ObjectContent<string>("", null!, "foo/bar"),
                () => new ObjectContent<string>("", null!, _jsonHeaderValue)
            };

            foreach (var action in actions)
            {
                var exception = Assert.Throws<ArgumentNullException>(action);
                Assert.Equal("formatter", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_Tests()
        {
            var value = "value";
            var contentType = "application/json";

            var content = new ObjectContent<string>(value, _formatter, contentType);

            Assert.Same(_formatter, content.Formatter);
            Assert.Same(value, content.Value);
            Assert.Same(typeof(string), content.ObjectType);
            Assert.Equal(MediaTypeHeaderValue.Parse(contentType), content.Headers.ContentType);
        }

        [Fact]
        public void Value_Setter_Test()
        {
            var newValue = "newValue";
            var content = new ObjectContent<string>("old", _formatter);

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
    }
}