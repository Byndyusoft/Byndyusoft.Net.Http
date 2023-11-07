using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     Extension methods for <see cref="MediaTypeFormatter" />.
    /// </summary>
    public static class MediaTypeFormatterExtensions
    {
        /// <summary>
        ///     Returns a <see cref="Task" /> to deserialize an object of the given <paramref name="type" /> from the given
        ///     <paramref name="readStream" />
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This implementation throws a <see cref="NotSupportedException" />. Derived types should override this method if
        ///         the formatter supports reading.
        ///     </para>
        ///     <para>
        ///         An implementation of this method should NOT close <paramref name="readStream" /> upon completion. The stream
        ///         will be closed independently when
        ///         the <see cref="HttpContent" /> instance is disposed.
        ///     </para>
        /// </remarks>
        /// <param name="formatter">The <see cref="MediaTypeFormatter"/> instance.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The <see cref="Stream" /> to read.</param>
        /// <param name="content">The <see cref="HttpContent" /> if available. It may be <c>null</c>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task" /> whose result will be an object of the given type.</returns>
        /// <exception cref="NotSupportedException">Derived types need to support reading.</exception>
        public static Task<object?> ReadFromStreamAsync(this MediaTypeFormatter formatter,
            Type type, Stream readStream, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (formatter == null) throw Error.ArgumentNull(nameof(formatter));

            return formatter.ReadFromStreamAsync(type, readStream, content, null, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> to deserialize an object of the given <paramref name="type" /> from the given
        ///     <paramref name="readStream" />
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This implementation throws a <see cref="NotSupportedException" />. Derived types should override this method if
        ///         the formatter supports reading.
        ///     </para>
        ///     <para>
        ///         An implementation of this method should NOT close <paramref name="readStream" /> upon completion. The stream
        ///         will be closed independently when
        ///         the <see cref="HttpContent" /> instance is disposed.
        ///     </para>
        /// </remarks>
        /// <param name="formatter">The <see cref="MediaTypeFormatter"/> instance.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="readStream">The <see cref="Stream" /> to read.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task" /> whose result will be an object of the given type.</returns>
        /// <exception cref="NotSupportedException">Derived types need to support reading.</exception>
        public static Task<object?> ReadFromStreamAsync(this MediaTypeFormatter formatter,
            Type type, Stream readStream, CancellationToken cancellationToken = default)
        {
            if (formatter == null) throw Error.ArgumentNull(nameof(formatter));

            return formatter.ReadFromStreamAsync(type, readStream, null, null, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that serializes the given <paramref name="value" /> of the given
        ///     <paramref name="type" />
        ///     to the given <paramref name="writeStream" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This implementation throws a <see cref="NotSupportedException" />. Derived types should override this method if
        ///         the formatter
        ///         supports reading.
        ///     </para>
        ///     <para>
        ///         An implementation of this method should NOT close <paramref name="writeStream" /> upon completion. The stream
        ///         will be closed independently when
        ///         the <see cref="HttpContent" /> instance is disposed.
        ///     </para>
        /// </remarks>
        /// <param name="formatter">The <see cref="MediaTypeFormatter"/> instance.</param>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write.  It may be <c>null</c>.</param>
        /// <param name="writeStream">The <see cref="Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="HttpContent" /> if available. It may be <c>null</c>.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task" /> that will perform the write.</returns>
        /// <exception cref="NotSupportedException">Derived types need to support writing.</exception>
        public static Task WriteToStreamAsync(this MediaTypeFormatter formatter,
            Type type, object? value, Stream writeStream, HttpContent content, CancellationToken cancellationToken = default)
        {
            if (formatter == null) throw Error.ArgumentNull(nameof(formatter));

            return formatter.WriteToStreamAsync(type, value, writeStream, content, null, cancellationToken);
        }

        /// <summary>
        ///     Returns a <see cref="Task" /> that serializes the given <paramref name="value" /> of the given
        ///     <paramref name="type" />
        ///     to the given <paramref name="writeStream" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This implementation throws a <see cref="NotSupportedException" />. Derived types should override this method if
        ///         the formatter
        ///         supports reading.
        ///     </para>
        ///     <para>
        ///         An implementation of this method should NOT close <paramref name="writeStream" /> upon completion. The stream
        ///         will be closed independently when
        ///         the <see cref="HttpContent" /> instance is disposed.
        ///     </para>
        /// </remarks>
        /// <param name="formatter">The <see cref="MediaTypeFormatter"/> instance.</param>
        /// <param name="type">The type of the object to write.</param>
        /// <param name="value">The object value to write.  It may be <c>null</c>.</param>
        /// <param name="writeStream">The <see cref="Stream" /> to which to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task" /> that will perform the write.</returns>
        /// <exception cref="NotSupportedException">Derived types need to support writing.</exception>
        public static Task WriteToStreamAsync(this MediaTypeFormatter formatter,
            Type type, object? value, Stream writeStream, CancellationToken cancellationToken = default)
        {
            if (formatter == null) throw Error.ArgumentNull(nameof(formatter));

            return formatter.WriteToStreamAsync(type, value, writeStream, null, null, cancellationToken);
        }
    }
}