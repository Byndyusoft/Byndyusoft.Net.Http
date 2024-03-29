using System.IO;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    ///     Contains a value as well as an associated <see cref="MediaTypeFormatter" /> that will be
    ///     used to serialize the value when writing this content.
    /// </summary>
    public class ObjectContent : HttpContent, IAsyncDisposable
    {
        private object? _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectContent" /> class.
        /// </summary>
        /// <param name="type">The type of object this instance will contain.</param>
        /// <param name="value">The value of the object this instance will contain.</param>
        /// <param name="formatter">The formatter to use when serializing the value.</param>
        /// <param name="mediaType">
        ///     The authoritative value of the content's Content-Type header. Can be <c>null</c> in which case the
        ///     <paramref name="formatter">formatter's</paramref> default content type will be used.
        /// </param>
        public ObjectContent(Type type, object? value, MediaTypeFormatter formatter, string mediaType)
            : this(type, value, formatter, BuildHeaderValue(mediaType))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectContent" /> class.
        /// </summary>
        /// <param name="type">The type of object this instance will contain.</param>
        /// <param name="value">The value of the object this instance will contain.</param>
        /// <param name="formatter">The formatter to use when serializing the value.</param>
        /// <param name="mediaType">
        ///     The authoritative value of the content's Content-Type header. Can be <c>null</c> in which case the
        ///     <paramref name="formatter">formatter's</paramref> default content type will be used.
        /// </param>
        public ObjectContent(Type type, object? value, MediaTypeFormatter formatter,
            MediaTypeHeaderValue? mediaType = null)
        {
            if (type == null) throw Error.ArgumentNull(nameof(type));
            if (formatter == null) throw Error.ArgumentNull(nameof(formatter));

            if (!formatter.CanWriteType(type))
                throw Error.InvalidOperation(Properties.Resources.ObjectContent_FormatterCannotWriteType,
                    formatter.GetType().FullName, type.Name);

            Formatter = formatter;
            ObjectType = type;

            VerifyAndSetObject(value);
            Formatter.SetDefaultContentHeaders(type, Headers, mediaType);
        }

        /// <summary>
        ///     Gets the type of object managed by this <see cref="ObjectContent" /> instance.
        /// </summary>
        public Type ObjectType { get; }

        /// <summary>
        ///     The <see cref="MediaTypeFormatter">formatter</see> associated with this content instance.
        /// </summary>
        public MediaTypeFormatter Formatter { get; }

        /// <summary>
        ///     Gets or sets the value of the current <see cref="ObjectContent" />.
        /// </summary>
        public object? Value
        {
            get => _value;
            set => VerifyAndSetObject(value);
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        internal static MediaTypeHeaderValue? BuildHeaderValue(string? mediaType)
        {
            return mediaType != null ? new MediaTypeHeaderValue(mediaType) : null;
        }

        protected virtual ValueTask DisposeAsyncCore()
        {
            return new ValueTask();
        }

        /// <summary>
        ///     Asynchronously serializes the object's content to the given <paramref name="stream" />.
        /// </summary>
        /// <param name="stream">The <see cref="Stream" /> to which to write.</param>
        /// <param name="context">The associated <see cref="TransportContext" />.</param>
        /// <returns>A <see cref="Task" /> instance that is asynchronously serializing the object's content.</returns>
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Formatter.WriteToStreamAsync(ObjectType, Value, stream, this, context);
        }

        /// <summary>
        ///     Computes the length of the stream if possible.
        /// </summary>
        /// <param name="length">The computed length of the stream.</param>
        /// <returns><c>true</c> if the length has been computed; otherwise <c>false</c>.</returns>
        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        private static bool IsTypeNullable(Type type)
        {
            return !type.IsValueType ||
                   type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private void VerifyAndSetObject(object? value)
        {
            if (value == null)
            {
                // Null may not be assigned to value types (unless Nullable<T>)
                if (!IsTypeNullable(ObjectType))
                    throw Error.InvalidOperation(Properties.Resources.CannotUseNullValueType, nameof(ObjectContent),
                        ObjectType.Name);
            }
            else
            {
                // Non-null objects must be a type assignable to Type
                var objectType = value.GetType();
                if (!ObjectType.IsAssignableFrom(objectType))
                    throw Error.Argument(nameof(value), Properties.Resources.ObjectAndTypeDisagree, objectType.Name,
                        ObjectType.Name);
            }

            _value = value;
        }
    }
}