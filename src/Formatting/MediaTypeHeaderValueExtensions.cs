using System.Linq;
using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    /// <summary>
    ///     Extension methods for <see cref="MediaTypeHeaderValue" />.
    /// </summary>
    internal static class MediaTypeHeaderValueExtensions
    {
        /// <summary>
        ///     Determines whether two <see cref="MediaTypeHeaderValue" /> instances match. The instance
        ///     <paramref name="mediaType1" /> is said to match <paramref name="mediaType2" /> if and only if
        ///     <paramref name="mediaType1" /> is a strict subset of the values and parameters of <paramref name="mediaType2" />.
        ///     That is, if the media type and media type parameters of <paramref name="mediaType1" /> are all present
        ///     and match those of <paramref name="mediaType2" /> then it is a match even though <paramref name="mediaType2" /> may
        ///     have additional
        ///     parameters.
        /// </summary>
        /// <param name="mediaType1">The first media type.</param>
        /// <param name="mediaType2">The second media type.</param>
        /// <returns><c>true</c> if this is a subset of <paramref name="mediaType2" />; false otherwise.</returns>
        public static bool IsSubsetOf(this MediaTypeHeaderValue mediaType1, MediaTypeHeaderValue? mediaType2)
        {
            return IsSubsetOf(mediaType1, mediaType2, out _);
        }

        /// <summary>
        ///     Determines whether two <see cref="MediaTypeHeaderValue" /> instances match. The instance
        ///     <paramref name="mediaType1" /> is said to match <paramref name="mediaType2" /> if and only if
        ///     <paramref name="mediaType1" /> is a strict subset of the values and parameters of <paramref name="mediaType2" />.
        ///     That is, if the media type and media type parameters of <paramref name="mediaType1" /> are all present
        ///     and match those of <paramref name="mediaType2" /> then it is a match even though <paramref name="mediaType2" /> may
        ///     have additional
        ///     parameters.
        /// </summary>
        /// <param name="mediaType1">The first media type.</param>
        /// <param name="mediaType2">The second media type.</param>
        /// <param name="mediaType2Range">
        ///     Indicates whether <paramref name="mediaType2" /> is a regular media type, a subtype media
        ///     range, or a full media range
        /// </param>
        /// <returns><c>true</c> if this is a subset of <paramref name="mediaType2" />; false otherwise.</returns>
        public static bool IsSubsetOf(this MediaTypeHeaderValue mediaType1, MediaTypeHeaderValue? mediaType2,
            out MediaTypeHeaderValueRange mediaType2Range)
        {
            if (mediaType2 == null)
            {
                mediaType2Range = MediaTypeHeaderValueRange.None;
                return false;
            }

            var parsedMediaType1 = new ParsedMediaTypeHeaderValue(mediaType1);
            var parsedMediaType2 = new ParsedMediaTypeHeaderValue(mediaType2);
            mediaType2Range = parsedMediaType2.IsAllMediaRange ? MediaTypeHeaderValueRange.AllMediaRange :
                parsedMediaType2.IsSubtypeMediaRange ? MediaTypeHeaderValueRange.SubtypeMediaRange :
                MediaTypeHeaderValueRange.None;

            if (!string.Equals(parsedMediaType1.Type, parsedMediaType2.Type, StringComparison.OrdinalIgnoreCase))
            {
                if (mediaType2Range != MediaTypeHeaderValueRange.AllMediaRange) return false;
            }
            else if (!string.Equals(parsedMediaType1.Subtype, parsedMediaType2.Subtype,
                StringComparison.OrdinalIgnoreCase))
            {
                if (mediaType2Range != MediaTypeHeaderValueRange.SubtypeMediaRange) return false;
            }

            // So far we either have a full match or a subset match. Now check that all of 
            // mediaType1's parameters are present and equal in mediatype2
            foreach (var parameter1 in mediaType1.Parameters)
                if (!mediaType2.Parameters.Any(parameter2 => parameter1.Equals(parameter2)))
                    return false;

            return true;
        }

        public static MediaTypeHeaderValue Clone(this MediaTypeHeaderValue mediaType)
        {
            return (MediaTypeHeaderValue)((ICloneable)mediaType).Clone();
        }
    }
}