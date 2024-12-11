using System.Collections.Immutable;

namespace Prelude;

public partial record NonEmptyList<T>
{
    /// <summary>
    /// Determines whether the <see cref="NonEmptyList{T}"/> contains elements
    /// that match the conditions defined by the specified predicate.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements
    /// to search for.
    /// </param>
    /// <returns>
    /// true if the <see cref="NonEmptyList{T}"/> contains one or more elements
    /// that match the conditions defined by the specified predicate; otherwise,
    /// false.
    /// </returns>
    public bool Exists(Predicate<T> match) => _innerList.Exists(match);

    /// <summary>
    /// See the <see cref="NonEmptyList{T}"/> interface.
    /// </summary>
    public bool Contains(T value) => _innerList.Contains(value, EqualityComparer<T>.Default);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the first occurrence within the entire <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.
    /// </param>
    /// <returns>
    /// The first element that matches the conditions defined by the specified predicate,
    /// if found; otherwise, <see cref="Option{T}.None"/>.
    /// </returns>
    public Option<T> Find(Predicate<T> match)
    {
        foreach (var value in _innerList)
            if (match(value)) return Option.Some(value);

        return Option.None<T>();
    }

    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the specified
    /// predicate.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements
    /// to search for.
    /// </param>
    /// <returns>
    /// A <see cref="ImmutableList{T}"/> containing all the elements that match
    /// the conditions defined by the specified predicate, if found; otherwise, an
    /// empty <see cref="ImmutableList{T}"/>.
    /// </returns>
    public ImmutableList<T> FindAll(Predicate<T> match) => _innerList.FindAll(match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the first occurrence within
    /// the entire <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None"/>.
    /// </returns>
    public Option<int> FindIndex(Predicate<T> match) =>
        _innerList.FindIndex(match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the first occurrence within
    /// the range of elements in the <see cref="NonEmptyList{T}"/> that extends
    /// from the specified index to the last element.
    /// </summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None"/>.
    /// </returns>
    public Option<int> FindIndex(int startIndex, Predicate<T> match) =>
        _innerList.FindIndex(startIndex, match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };


    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the first occurrence within
    /// the range of elements in the <see cref="NonEmptyList{T}"/> that starts
    /// at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
    /// <returns>
    /// The zero-based index of the first occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None"/>.
    /// </returns>
    public Option<int> FindIndex(int startIndex, int count, Predicate<T> match) =>
        _innerList.FindIndex(startIndex, count, match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the last occurrence within the entire <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.
    /// </param>
    /// <returns>
    /// The last element that matches the conditions defined by the specified predicate,
    /// if found; otherwise, <see cref="Option{T}.None" />.
    /// </returns>
    public Option<T> FindLast(Predicate<T> match)
    {
        for (var i = _innerList.Count - 1; i >= 0; i--)
            if (match(_innerList[i])) return Option.Some(_innerList[i]);

        return Option.None<T>();
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the last occurrence within
    /// the entire <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.
    /// </param>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None" /> />.
    /// </returns>
    public Option<int> FindLastIndex(Predicate<T> match) =>
        _innerList.FindLastIndex(match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the last occurrence within
    /// the range of elements in the <see cref="NonEmptyList{T}"/> that extends
    /// from the first element to the specified index.
    /// </summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.</param>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None" /> />.
    /// </returns>
    public Option<int> FindLastIndex(int startIndex, Predicate<T> match) =>
        _innerList.FindLastIndex(startIndex, match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified
    /// predicate, and returns the zero-based index of the last occurrence within
    /// the range of elements in the <see cref="NonEmptyList{T}"/> that contains
    /// the specified number of elements and ends at the specified index.
    /// </summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">
    /// The <see cref="Predicate{T}"/> delegate that defines the conditions of the element
    /// to search for.
    /// </param>
    /// <returns>
    /// The zero-based index of the last occurrence of an element that matches the
    /// conditions defined by <paramref name="match"/>, if found; otherwise, <see cref="Option{Int32}.None" /> />.
    /// </returns>
    public Option<int> FindLastIndex(int startIndex, int count, Predicate<T> match) =>
        _innerList.FindLastIndex(startIndex, count, match) switch
        {
            -1 => Option.None<int>(),
            var index => Option.Some(index)
        };

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>
    /// that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="index">
    /// The zero-based starting index of the search. 0 (zero) is valid in an empty
    /// list.
    /// </param>
    /// <param name="count">
    /// The number of elements in the section to search.
    /// </param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the range of
    /// elements in the <see cref="NonEmptyList{T}"/> that starts at <paramref name="index"/> and
    /// contains <paramref name="count"/> number of elements, if found; otherwise, <see cref="Option{Int32}.None" />.
    /// </returns>
    public Option<int> IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
        _innerList.IndexOf(item, index, count, equalityComparer) switch
        {
            -1 => Option.None<int>(),
            var i => Option.Some(i)
        };

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// last occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>
    /// that contains the specified number of elements and ends at the specified
    /// index.
    /// </summary>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="index">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="equalityComparer">
    /// The equality comparer to use in the search.
    /// </param>
    /// <returns>
    /// The zero-based index of the last occurrence of <paramref name="item"/> within the range of elements
    /// in the <see cref="NonEmptyList{T}"/> that contains <paramref name="count"/> number of elements
    /// and ends at <paramref name="index"/>, if found; otherwise, <see cref="Option{Int32}.None" /> />.
    /// </returns>
    public Option<int> LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) =>
        _innerList.LastIndexOf(item, index, count, equalityComparer) switch
        {
            -1 => Option.None<int>(),
            var i => Option.Some(i)
        };


}
