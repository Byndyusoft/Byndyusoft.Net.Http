using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Moq;
using Xunit;

namespace System.Net.Http
{
    public class ObjectContentOfTTests
    {
        private readonly MediaTypeHeaderValue _jsonHeaderValue = new MediaTypeHeaderValue("application/json");

        [Fact]
        public void Constructor_WhenFormatterParameterIsNull_Throws()
        {
            Func<ObjectContent<string>>[] actions =
            {
                () => new ObjectContent<string>("", null),
                () => new ObjectContent<string>("", null, "foo/bar"),
                () => new ObjectContent<string>("", null, _jsonHeaderValue)
            };

            foreach (var action in actions)
            {
                var exception = Assert.Throws<ArgumentNullException>(action);
                Assert.Equal("formatter", exception.ParamName);
            }
        }

        [Fact]
        public void Constructor_SetsFormatterProperty()
        {
            var formatterMock = new Mock<MediaTypeFormatter>();
            formatterMock.Setup(f => f.CanWriteType(typeof(string))).Returns(true);
            var formatter = formatterMock.Object;

            var content = new ObjectContent<string>(null, formatter);

            Assert.Same(formatter, content.Formatter);
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