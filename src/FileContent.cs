using System.IO;
using System.Net.Http.Headers;

namespace System.Net.Http
{
    /// <summary>
    ///     Provides HTTP content based on a file content.
    /// </summary>
    public class FileContent : StreamContent
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
            : this(stream, fileName, length, BuildHeaderValue(mediaType))
        {
        }

        public FileContent(Stream stream, string fileName, long? length = null, string? mediaType = null)
            : this(stream, fileName, length, BuildHeaderValue(mediaType))
        {
        }

        public FileContent(Stream stream, string fileName, long? length, MediaTypeHeaderValue? mediaType)
            : base(NotNull(stream, argName: nameof(stream)))
        {
            _stream = NotNull(stream, argName: nameof(stream));
            _fileName = NotNull(fileName, argName: nameof(fileName));
            _length = length;

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

        protected override bool TryComputeLength(out long length)
        {
            if (_length != null)
            {
                length = _length.Value;
                return true;
            }

            return base.TryComputeLength(out length);
        }

        private static MediaTypeHeaderValue? BuildHeaderValue(string? mediaType)
        {
            return mediaType != null ? new MediaTypeHeaderValue(mediaType) : null;
        }

        private static T NotNull<T>(T? value, string argName) where T : class
        {
            if (ReferenceEquals(value, null)) throw Error.ArgumentNull(argName);
            return value;
        }
    }
}