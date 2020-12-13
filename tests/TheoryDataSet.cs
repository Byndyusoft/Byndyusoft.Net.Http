﻿using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace System.Net.Http
{
    /// <summary>
    ///     Helper class for generating test data for XUnit's <see cref="TheoryAttribute" />-based tests.
    ///     Should be used in combination with <see cref="Xunit.Extensions.PropertyDataAttribute" />.
    /// </summary>
    /// <typeparam name="TParam">First parameter type</typeparam>
    public class TheoryDataSet<TParam> : TheoryDataSet
    {
        public void Add(TParam p)
        {
            AddItem(p);
        }
    }

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
    ///     Helper class for generating test data for XUnit's <see cref="TheoryAttribute" />-based tests.
    ///     Should be used in combination with <see cref="Xunit.Extensions.PropertyDataAttribute" />.
    /// </summary>
    /// <typeparam name="TParam1">First parameter type</typeparam>
    /// <typeparam name="TParam2">Second parameter type</typeparam>
    /// <typeparam name="TParam3">Third parameter type</typeparam>
    public class TheoryDataSet<TParam1, TParam2, TParam3> : TheoryDataSet
    {
        public void Add(TParam1 p1, TParam2 p2, TParam3 p3)
        {
            AddItem(p1, p2, p3);
        }
    }

    /// <summary>
    ///     Helper class for generating test data for XUnit's <see cref="TheoryAttribute" />-based tests.
    ///     Should be used in combination with <see cref="Xunit.Extensions.PropertyDataAttribute" />.
    /// </summary>
    /// <typeparam name="TParam1">First parameter type</typeparam>
    /// <typeparam name="TParam2">Second parameter type</typeparam>
    /// <typeparam name="TParam3">Third parameter type</typeparam>
    /// <typeparam name="TParam4">Fourth parameter type</typeparam>
    public class TheoryDataSet<TParam1, TParam2, TParam3, TParam4> : TheoryDataSet
    {
        public void Add(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            AddItem(p1, p2, p3, p4);
        }
    }

    /// <summary>
    ///     Helper class for generating test data for XUnit's <see cref="TheoryAttribute" />-based tests.
    ///     Should be used in combination with <see cref="Xunit.Extensions.PropertyDataAttribute" />.
    /// </summary>
    /// <typeparam name="TParam1">First parameter type</typeparam>
    /// <typeparam name="TParam2">Second parameter type</typeparam>
    /// <typeparam name="TParam3">Third parameter type</typeparam>
    /// <typeparam name="TParam4">Fourth parameter type</typeparam>
    /// <typeparam name="TParam5">Fifth parameter type</typeparam>
    public class TheoryDataSet<TParam1, TParam2, TParam3, TParam4, TParam5> : TheoryDataSet
    {
        public void Add(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            AddItem(p1, p2, p3, p4, p5);
        }
    }

    /// <summary>
    ///     Base class for <c>TheoryDataSet</c> classes.
    /// </summary>
    public abstract class TheoryDataSet : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>();

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void AddItem(params object[] values)
        {
            _data.Add(values);
        }
    }
}