using System.Globalization;
using System.Net.Http.Headers;

namespace System.Net.Http
{
    /// <summary>
    ///     Utility class for creating and unwrapping <see cref="Exception" /> instances.
    /// </summary>
    internal static class Error
    {
        /// <summary>
        ///     Formats the specified resource string using <see cref="M:CultureInfo.CurrentCulture" />.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns>The formatted string.</returns>
        private static string Format(string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        ///     Creates an <see cref="ArgumentException" /> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentException Argument(string parameterName, string messageFormat,
            params object[] messageArgs)
        {
            return new ArgumentException(Format(messageFormat, messageArgs), parameterName);
        }

        /// <summary>
        ///     Creates an <see cref="ArgumentNullException" /> with the provided properties.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentNullException ArgumentNull(string parameterName)
        {
            return new ArgumentNullException(parameterName);
        }

        /// <summary>
        ///     Creates an <see cref="InvalidOperationException" />.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static InvalidOperationException InvalidOperation(string messageFormat, params object[] messageArgs)
        {
            return new InvalidOperationException(Format(messageFormat, messageArgs));
        }

        /// <summary>
        ///     Creates an <see cref="NotSupportedException" />.
        /// </summary>
        /// <param name="messageFormat">A composite format string explaining the reason for the exception.</param>
        /// <param name="messageArgs">An object array that contains zero or more objects to format.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static NotSupportedException NotSupported(string messageFormat, params object[] messageArgs)
        {
            return new NotSupportedException(Format(messageFormat, messageArgs));
        }

        /// <summary>
        ///     Creates an <see cref="UnsupportedMediaTypeException" />.
        /// </summary>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static UnsupportedMediaTypeException UnsupportedMediaType(Type type, MediaTypeHeaderValue mediaType)
        {
            return new UnsupportedMediaTypeException(
                Format(Properties.Resources.NoReadSerializerAvailable, type.Name, mediaType.MediaType), mediaType);
        }

        /// <summary>
        ///     Creates an <see cref="ArgumentOutOfRangeException" /> with a message saying that the argument must be greater than
        ///     or equal to <paramref name="minValue" />.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the current exception.</param>
        /// <param name="actualValue">The value of the argument that causes this exception.</param>
        /// <param name="minValue">The minimum size of the argument.</param>
        /// <returns>The logged <see cref="Exception" />.</returns>
        internal static ArgumentOutOfRangeException ArgumentMustBeGreaterThanOrEqualTo(string parameterName,
            object actualValue, object minValue)
        {
            return new ArgumentOutOfRangeException(parameterName, actualValue,
                Format(Properties.Resources.ArgumentMustBeGreaterThanOrEqualTo, minValue));
        }
    }
}