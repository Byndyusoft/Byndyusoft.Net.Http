using System.IO;
using System.Net.Http.Headers;
using System.Net.Http.Internal;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    ///     Provides HTTP content based on a file content.
    /// </summary>
    public class FileContent : HttpContent
    {
        private readonly Stream _stream;
        private readonly long? _length;
        private readonly string _fileName;

        [Obsolete("Use constructor (stream, fileName, length, mediaType) instead")]
        public FileContent(Stream stream, long? length, string fileName, MediaTypeHeaderValue? mediaType)
            : this(stream, fileName, length, mediaType)
        {
        }

        [Obsolete("Use constructor (stream, fileName, length, mediaType) instead")]
        public FileContent(Stream stream, long? length, string fileName, string? mediaType = null)
            : this(stream,  fileName, length, BuildHeaderValue(mediaType))
        {
        }

        public FileContent(Stream stream, string fileName, long? length = null, string? mediaType = null)
            : this(stream, fileName, length, BuildHeaderValue(mediaType))
        {
        }

        public FileContent(Stream stream, string fileName, long? length, MediaTypeHeaderValue? mediaType)
        {
            if (ReferenceEquals(stream, null)) throw Error.ArgumentNull(nameof(stream));
            if (ReferenceEquals(fileName, null)) throw Error.ArgumentNull(nameof(fileName));

            _stream = stream!;
            _length = length;
            _fileName = fileName!;

            Headers.ContentType = mediaType ?? MediaTypeConstants.ApplicationOctetStreamMediaType;
            Headers.ContentLength = length;
        }

        /// <summary>
        ///     Gets the file length in bytes.
        /// </summary>
        public long Length => _length ?? _stream.Length;

        /// <summary>
        ///     Gets the file name.
        /// </summary>
        public string FileName => _fileName;

        /// <summary>
        ///     Get the media type of the file.
        /// </summary>
        public string MediaType => Headers.ContentType.MediaType;

        /// <summary>
        ///     Get stream for reading the file.
        /// </summary>
        public Stream Stream => _stream;

        /// <summary>
        ///     Opens the request stream for reading the file.
        /// </summary>
        /// <returns><see cref="Stream"/></returns>
        public virtual Stream OpenReadStream()
        {
            _stream.Seek(0, SeekOrigin.Begin);
            return new NonClosingDelegatingStream(_stream, _length);
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            await _stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _length ?? 0;
            return _length != null;
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