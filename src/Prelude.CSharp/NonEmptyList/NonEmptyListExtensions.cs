using System.Collections.Immutable;

namespace Prelude;

/// <summary>
/// Static methods for creating and manipulating <see cref="NonEmptyList{T}"/> instances.
/// </summary>
public static class NonEmptyList
{
    /// <summary>
    /// Creates a new <see cref="NonEmptyList{T}"/> from the specified head and tail.
    /// </summary>
    /// <param name="head">The head element.</param>
    /// <param name="tail">The tail of a list.</param>
    /// <returns>
    /// A <see cref="NonEmptyList{T}"/> starting with a <paramref name="head"/> element
    /// and the rest of the <paramref name="tail" /> elements.
    /// </returns>
    public static NonEmptyList<T> Create<T>(T head, IEnumerable<T>? tail = null) => new(head, tail);

    /// <summary>
    /// Converts an <see cref="IEnumerable{T}"/> to a <see cref="NonEmptyList{T}"/> if possible.
    /// </summary>
    /// <param name="source">The <see cref="IEnumerable{T}"/> to convert.</param>
    /// <returns>
    /// <see cref="Option.Some{T}"/> containing the <see cref="NonEmptyList{T}"/> if the source is not empty;
    /// otherwise, <see cref="Option.None{T}"/>.
    /// </returns>
    public static Option<NonEmptyList<T>> ToNonEmptyList<T>(this IEnumerable<T> source) =>
        source switch
        {
            NonEmptyList<T> nel => Option.Some(nel),
            ImmutableList<T> { IsEmpty: false } il => Option.Some(new NonEmptyList<T>(il)),
            _ => source.ToImmutableList() switch
            {
                { IsEmpty: true } => Option.None<NonEmptyList<T>>(),
                var xs => Option.Some(new NonEmptyList<T>(xs))
            }
        };

    /// <summary>Returns a new list with the first matching element in the list replaced with the specified element.</summary>
    /// <param name="source">A list to operate on</param>
    /// <param name="oldValue">The element to be replaced.</param>
    /// <param name="newValue">The element to replace the first occurrence of <paramref name="oldValue" /> with.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="oldValue" /> does not exist in the list.</exception>
    /// <returns>A new list that contains <paramref name="newValue" />, even if <paramref name="oldValue" /> is the same as <paramref name="newValue" />.</returns>
    public static NonEmptyList<T> Replace<T>(this NonEmptyList<T> source, T oldValue, T newValue) =>
        source.Replace(oldValue, newValue, EqualityComparer<T>.Default);

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>
    /// that starts at the specified index and contains the specified number of elements.
    /// </summary>
    /// <param name="source">A list to operate on.</param>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the range of
    /// elements in the <see cref="NonEmptyList{T}"/>, if found; otherwise, <see cref="Option{T}.None" />.
    /// </returns>
    public static Option<int> IndexOf<T>(this NonEmptyList<T> source, T item) =>
        source.IndexOf(item, 0, source.Count, EqualityComparer<T>.Default);

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="source">A list to operate on.</param>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="equalityComparer">
    /// An <see cref="IEqualityComparer{T}"/> to compare values. If null, the default equality comparer is used.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the range of
    /// elements in the <see cref="NonEmptyList{T}"/>, if found; otherwise, <see cref="Option{T}.None" />.
    /// </returns>
    public static Option<int> IndexOf<T>(this NonEmptyList<T> source, T item, IEqualityComparer<T>? equalityComparer) =>
        source.IndexOf(item, 0, source.Count, equalityComparer);

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>
    /// starting at the specified index.
    /// </summary>
    /// <param name="source">A list to operate on.</param>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="startIndex">
    /// The zero-based starting index of the search.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the range of
    /// elements in the <see cref="NonEmptyList{T}"/>, if found; otherwise, <see cref="Option{T}.None" />.
    /// </returns>
    public static Option<int> IndexOf<T>(this NonEmptyList<T> source, T item, int startIndex) =>
        source.IndexOf(item, startIndex, source.Count - startIndex, EqualityComparer<T>.Default);

    /// <summary>
    /// Searches for the specified object and returns the zero-based index of the
    /// first occurrence within the range of elements in the <see cref="NonEmptyList{T}"/>
    /// starting at the specified index and containing the specified number of elements.
    /// </summary>
    /// <param name="source">A list to operate on.</param>
    /// <param name="item">
    /// The object to locate in the <see cref="NonEmptyList{T}"/>. The value
    /// can be null for reference types.
    /// </param>
    /// <param name="startIndex">
    /// The zero-based starting index of the search.
    /// </param>
    /// <param name="count">
    /// The number of elements in the range to search.
    /// </param>
    /// <returns>
    /// The zero-based index of the first occurrence of <paramref name="item"/> within the range of
    /// elements in the <see cref="NonEmptyList{T}"/>, if found; otherwise, <see cref="Option{T}.None" />.
    /// </returns>
    public static Option<int> IndexOf<T>(this NonEmptyList<T> source, T item, int startIndex, int count) =>
        source.IndexOf(item, startIndex, count, EqualityComparer<T>.Default);
}
