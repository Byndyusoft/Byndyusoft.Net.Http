using System.IO;
using System.Xml;
using System.Net.Http.Formatting;

namespace System.Net.Http.Internal
{
    /// <summary>
    ///     Stream that doesn't close the inner stream when closed. This is to work around a limitation
    ///     in the <see cref="XmlDictionaryReader" /> insisting of closing the inner stream.
    ///     The regular <see cref="XmlReader" /> does allow for not closing the inner stream but that
    ///     doesn't have the quota that we need for security reasons. Implementations of
    ///     <see cref="MediaTypeFormatter" />
    ///     should not close the input stream when reading or writing so hence this workaround.
    /// </summary>
    internal class NonClosingDelegatingStream : DelegatingStream
    {
        public NonClosingDelegatingStream(Stream innerStream, long? length = null)
            : base(innerStream, length)
        {
        }

        public override void Close()
        {
        }
    }
}