using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     Collection class that contains <see cref="MediaTypeFormatter" /> instances.
    /// </summary>
    public class MediaTypeFormatterCollection : Collection<MediaTypeFormatter>
    {
        private static MediaTypeFormatterCollection? _default;

        private static readonly Type _mediaTypeFormatterType = typeof(MediaTypeFormatter);

        /// <summary>
        ///     Initializes a new instance of the <see cref="MediaTypeFormatterCollection" /> class.
        /// </summary>
        /// <remarks>
        ///     This collection will be initialized to contain default <see cref="MediaTypeFormatter" />
        ///     instances for Xml, JsonValue and Json.
        /// </remarks>
        public MediaTypeFormatterCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MediaTypeFormatterCollection" /> class.
        /// </summary>
        /// <param name="formatters">A collection of <see cref="MediaTypeFormatter" /> instances to place in the collection.</param>
        public MediaTypeFormatterCollection(IEnumerable<MediaTypeFormatter> formatters) : this()
        {
            VerifyAndSetFormatters(formatters);
        }

        /// <summary>
        ///     The collection of default <see cref="MediaTypeFormatter" /> instances.
        /// </summary>
        public static MediaTypeFormatterCollection Default => _default ??= new MediaTypeFormatterCollection();

        /// <summary>
        ///     Helper to search a collection for a formatter that can read the .NET type in the given mediaType.
        /// </summary>
        /// <param name="type">.NET type to read</param>
        /// <param name="mediaType">media type to match on.</param>
        /// <returns>Formatter that can read the type. Null if no formatter found.</returns>
        public MediaTypeFormatter? FindReader(Type type, MediaTypeHeaderValue mediaType)
        {
            if (type == null) throw Error.ArgumentNull("type");
            if (mediaType == null) throw Error.ArgumentNull("mediaType");

            foreach (var formatter in Items)
                if (formatter != null && formatter.CanReadType(type))
                    foreach (var supportedMediaType in formatter.SupportedMediaTypes)
                        if (supportedMediaType != null && supportedMediaType.IsSubsetOf(mediaType))
                            return formatter;

            return null;
        }

        /// <summary>
        ///     Helper to search a collection for a formatter that can write the .NET type in the given mediaType.
        /// </summary>
        /// <param name="type">.NET type to read</param>
        /// <param name="mediaType">media type to match on.</param>
        /// <returns>Formatter that can write the type. Null if no formatter found.</returns>
        public MediaTypeFormatter? FindWriter(Type type, MediaTypeHeaderValue mediaType)
        {
            if (type == null) throw Error.ArgumentNull("type");
            if (mediaType == null) throw Error.ArgumentNull("mediaType");

            foreach (var formatter in Items)
                if (formatter != null && formatter.CanWriteType(type))
                    foreach (var supportedMediaType in formatter.SupportedMediaTypes)
                        if (supportedMediaType != null && supportedMediaType.IsSubsetOf(mediaType))
                            return formatter;

            return null;
        }

        private void VerifyAndSetFormatters(IEnumerable<MediaTypeFormatter> formatters)
        {
            if (formatters == null) throw Error.ArgumentNull("formatters");

            foreach (var formatter in formatters)
            {
                if (formatter == null)
                    throw Error.Argument("formatters", Properties.Resources.CannotHaveNullInList,
                        _mediaTypeFormatterType.Name);

                Add(formatter);
            }
        }
    }
}