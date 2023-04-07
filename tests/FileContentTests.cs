using System.IO;
using System.Linq;
using Xunit;

namespace System.Net.Http
{
    public class FileContentTests
    {
        [Fact]
        public void Constructor_WhenStreamArgumentIsNull_ThrowsEsxception()
        {
            // act
            var exception = Assert.Throws<ArgumentNullException>(() => new FileContent(null!, 0, "fileName"));

            // assert
            Assert.Equal("stream", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenFileNameArgumentIsNull_ThrowsEsxception()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            var exception = Assert.Throws<ArgumentNullException>(() => new FileContent(stream, 0, null!));

            // assert
            Assert.Equal("fileName", exception.ParamName);
        }

        [Fact]
        public void Constructor_Test()
        {
            // arrange
            var stream = new MemoryStream();

            // act
            var fileContent = new FileContent(stream, 100, "fileName", "video/mp4");

            // assert
            Assert.Equal("fileName", fileContent.FileName);
            Assert.Equal(100, fileContent.Length);
            Assert.Equal("video/mp4", fileContent.MediaType);
        }

        [Fact]
        public void OpenReadStream_Test()
        {
            // arrange
            var content = Enumerable.Range(0, 100).Select(x => (byte)x).ToArray();

            // act
            var fileContent = new FileContent(new MemoryStream(content), content.Length, "fileName");
            using var stream = fileContent.OpenReadStream();

            // assert
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            Assert.Equal(content, ms.ToArray());
        }
    }
}