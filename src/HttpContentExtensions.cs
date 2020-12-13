using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    ///     Extension methods to allow strongly typed objects to be read from <see cref="HttpContent" /> instances.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class HttpContentExtensions
    {
        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance.
        /// </summary>
        /// <remarks>This override use the built-in collection of formatters.</remarks>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type)
        {
            return content.ReadAsAsync(type, MediaTypeFormatterCollection.Default);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance.
        /// </summary>
        /// <remarks>This override use the built-in collection of formatters.</remarks>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type, CancellationToken cancellationToken)
        {
            return content.ReadAsAsync(type, MediaTypeFormatterCollection.Default, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance using one of the provided <paramref name="formatters" />
        ///     to deserialize the content.
        /// </summary>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters)
        {
            return ReadAsAsync<object>(content, type, formatters, null);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance using one of the provided <paramref name="formatters" />
        ///     to deserialize the content.
        /// </summary>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters,
            CancellationToken cancellationToken)
        {
            return ReadAsAsync<object>(content, type, formatters, null, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance using one of the provided <paramref name="formatters" />
        ///     to deserialize the content.
        /// </summary>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="formatterLogger">The <see cref="IFormatterLogger" /> to log events to.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger)
        {
            return ReadAsAsync<object>(content, type, formatters, formatterLogger);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified <paramref name="type" />
        ///     from the <paramref name="content" /> instance using one of the provided <paramref name="formatters" />
        ///     to deserialize the content.
        /// </summary>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="type">The type of the object to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="formatterLogger">The <see cref="IFormatterLogger" /> to log events to.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<object> ReadAsAsync(this HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            return ReadAsAsync<object>(content, type, formatters, formatterLogger, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <remarks>This override use the built-in collection of formatters.</remarks>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            return content.ReadAsAsync<T>(MediaTypeFormatterCollection.Default);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <remarks>This override use the built-in collection of formatters.</remarks>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content, CancellationToken cancellationToken)
        {
            return content.ReadAsAsync<T>(MediaTypeFormatterCollection.Default, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content, IEnumerable<MediaTypeFormatter> formatters)
        {
            return ReadAsAsync<T>(content, typeof(T), formatters, null);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content, IEnumerable<MediaTypeFormatter> formatters,
            CancellationToken cancellationToken)
        {
            return ReadAsAsync<T>(content, typeof(T), formatters, null, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="formatterLogger">The <see cref="IFormatterLogger" /> to log events to.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content, IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger)
        {
            return ReadAsAsync<T>(content, typeof(T), formatters, formatterLogger);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that will yield an object of the specified
        ///     type <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">The <see cref="HttpContent" /> instance from which to read.</param>
        /// <param name="formatters">The collection of <see cref="MediaTypeFormatter" /> instances to use.</param>
        /// <param name="formatterLogger">The <see cref="IFormatterLogger" /> to log events to.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing reading the content as an object of the specified type.</returns>
        public static Task<T> ReadAsAsync<T>(this HttpContent content, IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            return ReadAsAsync<T>(content, typeof(T), formatters, formatterLogger, cancellationToken);
        }

        private static Task<T> ReadAsAsync<T>(HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger)
        {
            return ReadAsAsync<T>(content, type, formatters, formatterLogger, CancellationToken.None);
        }

        // There are many helper overloads for ReadAs*(). Provide one worker function to ensure the logic is shared.
        //
        // For loosely typed, T = Object, type = specific class.
        // For strongly typed, T == type.GetType()
        private static Task<T> ReadAsAsync<T>(HttpContent content, Type type,
            IEnumerable<MediaTypeFormatter> formatters,
            IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            if (content == null) throw Error.ArgumentNull(nameof(content));
            if (type == null) throw Error.ArgumentNull(nameof(type));
            if (formatters == null) throw Error.ArgumentNull(nameof(formatters));

            if (content is ObjectContent objectContent && objectContent.Value != null &&
                type.IsInstanceOfType(objectContent.Value)) return Task.FromResult((T) objectContent.Value);

            MediaTypeFormatter formatter;
            // Default to "application/octet-stream" if there is no content-type in accordance with section 7.2.1 of the HTTP spec
            var mediaType = content.Headers.ContentType ?? MediaTypeConstants.ApplicationOctetStreamMediaType;

            formatter = new MediaTypeFormatterCollection(formatters).FindReader(type, mediaType);

            if (formatter == null)
            {
                if (content.Headers.ContentLength == 0)
                {
                    var defaultValue = (T) MediaTypeFormatter.GetDefaultValueForType(type);
                    return Task.FromResult(defaultValue);
                }

                throw Error.UnsupportedMediaType(type, mediaType);
            }

            return ReadAsAsyncCore<T>(content, type, formatterLogger, formatter, cancellationToken);
        }

        private static async Task<T> ReadAsAsyncCore<T>(HttpContent content, Type type,
            IFormatterLogger formatterLogger,
            MediaTypeFormatter formatter, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var stream = await content.ReadAsStreamAsync();

            var result = await formatter.ReadFromStreamAsync(type, stream, content, formatterLogger, cancellationToken);
            return (T) result;
        }
    }
}