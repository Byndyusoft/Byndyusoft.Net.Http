using System.Net.Http.Headers;

namespace System.Net.Http
{
    /// <summary>
    ///     Defines an exception type for signalling that a request's media type was not supported.
    /// </summary>
    public class UnsupportedMediaTypeException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnsupportedMediaTypeException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="mediaType">The unsupported media type.</param>
        public UnsupportedMediaTypeException(string message, MediaTypeHeaderValue mediaType)
            : base(message)
        {
            MediaType = mediaType ?? throw Error.ArgumentNull(nameof(mediaType));
        }

        /// <summary>
        ///     The unsupported media type.
        /// </summary>
        public MediaTypeHeaderValue MediaType { get; }
    }
}