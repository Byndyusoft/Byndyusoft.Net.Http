using System.Net.Http.Headers;

namespace System.Net.Http.Formatting
{
    internal class ParsedMediaTypeHeaderValue
    {
        private const string MediaRangeAsterisk = "*";
        private const char MediaTypeSubtypeDelimiter = '/';

        private bool? _isAllMediaRange;
        private bool? _isSubtypeMediaRange;

        public ParsedMediaTypeHeaderValue(MediaTypeHeaderValue mediaType)
        {
            if (mediaType == null) throw Error.ArgumentNull(nameof(mediaType));

            var splitMediaType = mediaType.MediaType.Split(MediaTypeSubtypeDelimiter);

            if (splitMediaType.Length != 2)
                throw Error.Argument(nameof(mediaType),
                    "The constructor of the MediaTypeHeaderValue would have failed if there wasn't a type and subtype.");

            Type = splitMediaType[0];
            Subtype = splitMediaType[1];
        }

        public string Type { get; }

        public string Subtype { get; }

        public bool IsAllMediaRange
        {
            get
            {
                if (!_isAllMediaRange.HasValue)
                    _isAllMediaRange = IsSubtypeMediaRange &&
                                       string.Equals(MediaRangeAsterisk, Type, StringComparison.Ordinal);
                return _isAllMediaRange.Value;
            }
        }

        public bool IsSubtypeMediaRange
        {
            get
            {
                if (!_isSubtypeMediaRange.HasValue)
                    _isSubtypeMediaRange = string.Equals(MediaRangeAsterisk, Subtype, StringComparison.Ordinal);
                return _isSubtypeMediaRange.Value;
            }
        }
    }
}