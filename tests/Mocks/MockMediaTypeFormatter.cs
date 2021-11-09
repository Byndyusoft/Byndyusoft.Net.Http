using System.Net.Http.Formatting;

namespace System.Net.Http.Mocks
{
    public class MockMediaTypeFormatter : MediaTypeFormatter
    {
        public bool CallBase { get; set; }
        public Func<Type, bool>? CanReadTypeCallback { get; set; }
        public Func<Type, bool>? CanWriteTypeCallback { get; set; }

        internal override bool CanWriteAnyTypes => CanWriteAnyTypesReturn;

        public bool CanWriteAnyTypesReturn { get; set; } = true;

        public override bool CanReadType(Type type)
        {
            if (!CallBase && CanReadTypeCallback == null)
                throw new InvalidOperationException("CallBase or CanReadTypeCallback must be set first.");

            return CanReadTypeCallback?.Invoke(type) ?? true;
        }

        public override bool CanWriteType(Type type)
        {
            if (!CallBase && CanWriteTypeCallback == null)
                throw new InvalidOperationException("CallBase or CanWriteTypeCallback must be set first.");

            return CanWriteTypeCallback?.Invoke(type) ?? true;
        }
    }
}