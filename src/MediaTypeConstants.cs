using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace System.Net.Http
{
    /// <summary>
    ///     Constants related to media types.
    /// </summary>
    public static class MediaTypeConstants
    {
        // ReSharper disable InconsistentNaming
        private static readonly MediaTypeHeaderValue _defaultApplicationProtoBufMediaType =
            new MediaTypeHeaderValue("application/protobuf");

        private static readonly MediaTypeHeaderValue _defaultApplicationMessagePackMediaType =
            new MediaTypeHeaderValue("application/msgpack");

        private static readonly MediaTypeHeaderValue _defaultApplicationXmlMediaType =
            new MediaTypeHeaderValue("application/xml");

        private static readonly MediaTypeHeaderValue _defaultTextXmlMediaType = new MediaTypeHeaderValue("text/xml");

        private static readonly MediaTypeHeaderValue _defaultApplicationJsonMediaType =
            new MediaTypeHeaderValue("application/json");

        private static readonly MediaTypeHeaderValue _defaultTextJsonMediaType = new MediaTypeHeaderValue("text/json");

        private static readonly MediaTypeHeaderValue _defaultApplicationOctetStreamMediaType =
            new MediaTypeHeaderValue("application/octet-stream");

        private static readonly MediaTypeHeaderValue _defaultApplicationFormUrlEncodedMediaType =
            new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        // ReSharper enable InconsistentNaming

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/octet-stream</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/octet-stream</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationOctetStreamMediaType =>
            _defaultApplicationOctetStreamMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/xml</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/xml</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationXmlMediaType => _defaultApplicationXmlMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/json</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/json</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationJsonMediaType => _defaultApplicationJsonMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>text/xml</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>text/xml</c>.
        /// </value>
        public static MediaTypeHeaderValue TextXmlMediaType => _defaultTextXmlMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>text/json</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>text/json</c>.
        /// </value>
        public static MediaTypeHeaderValue TextJsonMediaType => _defaultTextJsonMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/x-www-form-urlencoded</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/x-www-form-urlencoded</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationFormUrlEncodedMediaType =>
            _defaultApplicationFormUrlEncodedMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/protobuf</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/protobuf</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationProtobufMediaType => _defaultApplicationProtoBufMediaType.Clone();

        /// <summary>
        ///     Gets a <see cref="MediaTypeHeaderValue" /> instance representing <c>application/msgpack</c>.
        /// </summary>
        /// <value>
        ///     A new <see cref="MediaTypeHeaderValue" /> instance representing <c>application/msgpack</c>.
        /// </value>
        public static MediaTypeHeaderValue ApplicationMessagePackMediaType =>
            _defaultApplicationMessagePackMediaType.Clone();
    }
}