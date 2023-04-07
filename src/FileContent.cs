using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    ///     Provides HTTP content based on a file content.
    /// </summary>
    public class FileContent : HttpContent
    {
        private readonly Stream _stream;
        private readonly long _length;
        private readonly string _fileName;

        public FileContent(Stream stream, long length, string fileName, string? mediaType = null)
            : this(stream, length, fileName, BuildHeaderValue(mediaType))
        {
            _stream = stream;
            _length = length;
            _fileName = fileName;
        }

        public FileContent(Stream stream, long length, string fileName, MediaTypeHeaderValue? mediaType)
        {
            if (ReferenceEquals(stream, null)) throw Error.ArgumentNull(nameof(stream));
            if (ReferenceEquals(fileName,null)) throw Error.ArgumentNull(nameof(fileName));

            _stream = stream!;
            _length = length;
            _fileName = fileName!;

            Headers.ContentType = mediaType ?? MediaTypeConstants.ApplicationOctetStreamMediaType;
            Headers.ContentLength = length;
        }

        /// <summary>
        ///     Gets the file length in bytes.
        /// </summary>
        public long Length => _length;

        /// <summary>
        ///     Gets the file name.
        /// </summary>
        public string FileName => _fileName;

        /// <summary>
        ///     Get the media type of the file.
        /// </summary>
        public string MediaType => Headers.ContentType.MediaType;

        /// <summary>
        ///     Opens the request stream for reading the file.
        /// </summary>
        /// <returns><see cref="Stream"/></returns>
        public Stream OpenReadStream() => _stream;

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await _stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _length;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _stream.Dispose();
        }

        private static MediaTypeHeaderValue? BuildHeaderValue(string? mediaType)
        {
            return mediaType != null ? new MediaTypeHeaderValue(mediaType) : null;
        }
    }
}