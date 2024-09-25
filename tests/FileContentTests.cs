using System.IO;
using System.Linq;
using Xunit;

namespace System.Net.Http
{
    public class FileContentTests
    {
        [Fact]
        public void Constructor_WhenStreamArgumentIsNull_ThrowsException()
        {
            // act
            var exception = Assert.Throws<ArgumentNullException>(() => new FileContent(null!, "fileName", 0));

            // assert
            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenFileNameArgumentIsNull_ThrowsException()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            var exception = Assert.Throws<ArgumentNullException>(() => new FileContent(stream, null!, 0));

            // assert
            Assert.Equal("fileName", exception.ParamName);
        }

        [Fact]
        public void Constructor_Test()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            var fileContent = new FileContent(stream, "fileName", 100, "video/mp4");

            // assert
            Assert.Equal("fileName", fileContent.FileName);
            Assert.Equal(100, fileContent.Length);
            Assert.Equal("video/mp4", fileContent.MediaType);
        }

        [Fact]
        public void Stream_Test()
        {
            // arrange
            var content = Enumerable.Range(0, 100).Select(x => (byte)x).ToArray();

            // act
            var fileContent = new FileContent(new MemoryStream(content),  "fileName", content.Length);
            using var stream = fileContent.Stream;

            // assert
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            Assert.Equal(content, ms.ToArray());
        }

        [Fact]
        public void Length_IfSpecified_Test()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            var fileContent = new FileContent(stream, "fileName", 100);

            // assert
            Assert.Equal(100, fileContent.Length);
        }

        [Fact]
        public void Length_FromStreamIfNotSpecified_Test()
        {
            // arrange
            var stream = new MemoryStream(new byte[100]);

            // act
            var fileContent = new FileContent(stream, "fileName");

            // assert
            Assert.Equal(stream.Length, fileContent.Length);
        }
    }
}