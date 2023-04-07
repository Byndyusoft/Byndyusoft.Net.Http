using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace System.Net.Http
{
    /// <summary>
    ///     Helper class for generating test data for XUnit's <see cref="TheoryAttribute" />-based tests.
    ///     Should be used in combination with <see cref="Xunit.Extensions.PropertyDataAttribute" />.
    /// </summary>
    /// <typeparam name="TParam1">First parameter type</typeparam>
    /// <typeparam name="TParam2">Second parameter type</typeparam>
    public class TheoryDataSet<TParam1, TParam2> : TheoryDataSet
    {
        public void Add(TParam1 p1, TParam2 p2)
        {
            AddItem(p1, p2);
        }
    }

    /// <summary>
    ///     Base class for <c>TheoryDataSet</c> classes.
    /// </summary>
    public abstract class TheoryDataSet : IEnumerable<object?[]>
    {
        private readonly List<object?[]> _data = new List<object?[]>();

        public IEnumerator<object?[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void AddItem(params object?[] values)
        {
            _data.Add(values);
        }
    }
}