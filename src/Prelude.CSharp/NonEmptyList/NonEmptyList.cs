using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Prelude;

/// <summary>
/// Represents a list that is guaranteed to contain at least one element. This ensures that operations on the list
/// can be performed without needing to handle empty cases, making it more predictable and reliable to work with.
/// </summary>
/// <typeparam name="T">The type of elements contained in the list.</typeparam>
/// <remarks>
/// The <see cref="NonEmptyList{T}"/> class is immutable, meaning that any modifications to the list result in the
/// creation of a new list, leaving the original list unchanged.
/// This makes it suitable for making illegal state unrepresentable.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[System.Text.Json.Serialization.JsonConverter(typeof(NonEmptyListJsonConverterFactory))]
public partial record NonEmptyList<T> : IReadOnlyList<T>, ICollection
{
    private readonly ImmutableList<T> _innerList;

    internal ImmutableList<T> InnerList => _innerList;

    /// <summary>
    /// Creates a new <see cref="NonEmptyList{T}"/> from the specified head and tail.
    /// </summary>
    /// <param name="head">The head element.</param>
    /// <param name="tail">The tail of a list.</param>
    /// <returns>
    /// A <see cref="NonEmptyList{T}"/> starting with a <paramref name="head"/> element
    /// and the rest of the <paramref name="tail" /> elements.
    /// </returns>
    public NonEmptyList(T head, IEnumerable<T>? tail = null)
    {
        if (head == null) throw new ArgumentNullException(nameof(head));
        _innerList = ImmutableList<T>.Empty.Add(head).AddRange(tail ?? ImmutableList<T>.Empty);
    }

    internal NonEmptyList(ImmutableList<T> innerList)
    {
        if (innerList.Count == 0) throw new ArgumentException("List must not be empty", nameof(innerList));
        _innerList = innerList;
    }

    /// <summary>
    /// Converts a <see cref="NonEmptyList{T}"/> to an <see cref="ImmutableList{T}"/>.
    /// </summary>
    /// <returns>An <see cref="ImmutableList{T}"/> with the same elements.</returns>
    public ImmutableList<T> ToImmutableList() => _innerList;

    /// <inheritdoc />
    public T this[int index] => _innerList.ItemRef(index);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    object ICollection.SyncRoot => this;

    /// <summary>
    /// See the <see cref="ICollection"/> interface.
    /// </summary>
    /// <devremarks>
    /// This type is immutable, so it is always thread-safe.
    /// </devremarks>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    bool ICollection.IsSynchronized => true;

    /// <summary>
    /// Return a number of elements in the list.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public int Count => _innerList.Count;

    /// <summary>
    /// Performs the specified action on each element of the list.
    /// </summary>
    /// <param name="action">The System.Action&lt;T&gt; delegate to perform on each element of the list.</param>
    public void ForEach(Action<T> action) => _innerList.ForEach(action);

    /// <summary>
    /// Converts the elements in the current <see cref="NonEmptyList{T}"/> to
    /// another type, and returns a list containing the converted elements.
    /// </summary>
    /// <param name="selector">
    /// A <see cref="Func{T, TResult}"/> delegate that converts each element from
    /// one type to another type.
    /// </param>
    /// <typeparam name="TResult">
    /// The type of the elements of the target array.
    /// </typeparam>
    /// <returns>
    /// A <see cref="NonEmptyList{T}"/> of the target type containing the converted
    /// elements from the current <see cref="NonEmptyList{T}"/>.
    /// </returns>
    public NonEmptyList<TResult> Map<TResult>(Func<T, TResult> selector) =>
        new(_innerList.ConvertAll(selector));

    /// <summary>
    /// Projects each element of the current <see cref="NonEmptyList{T}"/> to a <see cref="NonEmptyList{TResult}"/>
    /// and flattens the resulting lists into a single <see cref="NonEmptyList{TResult}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the elements in the resulting <see cref="NonEmptyList{TResult}"/>.</typeparam>
    /// <param name="selector">A transform function to apply to each element of the list.</param>
    /// <returns>
    /// A flattened <see cref="NonEmptyList{TResult}"/> that contains the elements from the lists returned by the
    /// <paramref name="selector"/> function.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="selector"/> is null.</exception>
    public NonEmptyList<TResult> FlatMap<TResult>(Func<T, NonEmptyList<TResult>> selector) =>
        new(_innerList.SelectMany(x => selector(x)._innerList).ToImmutableList());

    /// <summary>
    /// Projects each element of the current <see cref="NonEmptyList{T}"/> to a <see cref="NonEmptyList{TElement}"/> and then
    /// invokes a result selector function on each element of the original list and each element of the corresponding projected list.
    /// The results are then flattened into a single <see cref="NonEmptyList{TResult}"/>.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements in the intermediate <see cref="NonEmptyList{TElement}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the resulting <see cref="NonEmptyList{TResult}"/>.</typeparam>
    /// <param name="collectionSelector">A transform function to apply to each element of the list, which produces an intermediate <see cref="NonEmptyList{TCollection}"/>.</param>
    /// <param name="resultSelector">A transform function to apply to each pair of original and intermediate elements.</param>
    /// <returns>
    /// A flattened <see cref="NonEmptyList{TResult}"/> that contains the elements from the lists returned by the
    /// <paramref name="collectionSelector"/> function, transformed by the <paramref name="resultSelector"/> function.
    /// </returns>
    public NonEmptyList<TResult> FlatMap<TElement, TResult>(
        Func<T, NonEmptyList<TElement>> collectionSelector,
        Func<T, TElement, TResult> resultSelector) =>
        new(_innerList.SelectMany(x => collectionSelector(x)._innerList, resultSelector).ToImmutableList());

    /// <summary>
    /// Filters a sequence of values based on a predicate.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>
    ///  A new list that contains elements from the input list that satisfy the condition.
    ///  If no elements pass the test, <see cref="Option{T}.None"/> is returned.
    /// </returns>
    public Option<NonEmptyList<T>> Filter(Func<T, bool> predicate) =>
        _innerList.Where(predicate).ToNonEmptyList();

    /// <summary>
    /// Returns a new <see cref="NonEmptyList{T}"/> that contains distinct elements from the current list,
    /// preserving their original order.
    /// </summary>
    /// <returns>
    /// A new <see cref="NonEmptyList{T}"/> containing distinct elements from the current list.
    /// </returns>
    /// <remarks>
    /// This method uses the default equality comparer to determine equality of elements.
    /// </remarks>
    public NonEmptyList<T> Distinct() => new(_innerList.Distinct().ToImmutableList());

    /// <summary>
    /// Returns a new <see cref="NonEmptyList{T}"/> that contains distinct elements from the current list,
    /// determined by a specified key selector function.
    /// </summary>
    /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="keySelector">
    /// A function to extract a key from each element in the list.
    /// </param>
    /// <returns>
    /// A new <see cref="NonEmptyList{T}"/> containing distinct elements as determined by the keys
    /// returned by <paramref name="keySelector"/>.
    /// </returns>
    /// <remarks>
    /// The default equality comparer for the key type is used to compare keys.
    /// </remarks>
    public NonEmptyList<T> DistinctBy<K>(Func<T, K> keySelector) =>
        new(_innerList.DistinctBy(keySelector).ToImmutableList());

    /// <summary>
    /// Returns a new <see cref="NonEmptyList{T}"/> that contains distinct elements from the current list,
    /// determined by a specified key selector function and a custom equality comparer.
    /// </summary>
    /// <typeparam name="K">The type of the key returned by <paramref name="keySelector"/>.</typeparam>
    /// <param name="keySelector">
    /// A function to extract a key from each element in the list.
    /// </param>
    /// <param name="comparer">
    /// An <see cref="IEqualityComparer{K}"/> to compare keys.
    /// </param>
    /// <returns>
    /// A new <see cref="NonEmptyList{T}"/> containing distinct elements as determined by the keys
    /// returned by <paramref name="keySelector"/> and compared using <paramref name="comparer"/>.
    /// </returns>
    public NonEmptyList<T> DistinctBy<K>(Func<T, K> keySelector, IEqualityComparer<K> comparer) =>
        new(_innerList.DistinctBy(keySelector, comparer).ToImmutableList());

    /// <summary>
    /// Sorts the elements in the entire <see cref="NonEmptyList{T}"/> using
    /// the default comparer.
    /// </summary>
    public NonEmptyList<T> Sort() => new(_innerList.Sort());

    /// <summary>
    /// Sorts the elements in the entire <see cref="NonEmptyList{T}"/> using
    /// the specified <see cref="Comparison{T}"/>.
    /// </summary>
    /// <param name="comparison">
    /// The <see cref="Comparison{T}"/> to use when comparing elements.
    /// </param>
    /// <returns>The sorted list.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="comparison"/> is null.</exception>
    public NonEmptyList<T> Sort(Comparison<T> comparison) => new(_innerList.Sort(comparison));

    /// <summary>
    /// Sorts the elements in the entire <see cref="NonEmptyList{T}"/> using
    /// the specified comparer.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> implementation to use when comparing
    /// elements, or null to use the default comparer <see cref="Comparer{T}.Default"/>.
    /// </param>
    /// <returns>The sorted list.</returns>
    public NonEmptyList<T> Sort(IComparer<T>? comparer) => new(_innerList.Sort(comparer));

    /// <summary>
    /// Sorts the elements in a range of elements in <see cref="NonEmptyList{T}"/>
    /// using the specified comparer.
    /// </summary>
    /// <param name="index">
    /// The zero-based starting index of the range to sort.
    /// </param>
    /// <param name="count">
    /// The length of the range to sort.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> implementation to use when comparing
    /// elements, or null to use the default comparer <see cref="Comparer{T}.Default"/>.
    /// </param>
    /// <returns>The sorted list.</returns>
    public NonEmptyList<T> Sort(int index, int count, IComparer<T>? comparer) =>
        new(_innerList.Sort(index, count, comparer));

    /// <summary>
    /// Reverses the order of the elements in the entire <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <returns>The reversed list.</returns>
    public NonEmptyList<T> Reverse() => new(_innerList.Reverse());

    /// <summary>
    /// Reverses the order of the elements in the specified range.
    /// </summary>
    /// <param name="index">The zero-based starting index of the range to reverse.</param>
    /// <param name="count">The number of elements in the range to reverse.</param>
    /// <returns>The reversed list.</returns>
    public NonEmptyList<T> Reverse(int index, int count) => new(_innerList.Reverse(index, count));

    /// <summary>
    /// Gets a read-only reference to the element of the set at the given index.
    /// </summary>
    /// <param name="index">The 0-based index of the element in the set to return.</param>
    /// <returns>A read-only reference to the element at the given position.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="index"/> is negative or not less than <see cref="Count"/>.</exception>
    public ref readonly T ItemRef(int index) => ref _innerList.ItemRef(index);

    /// <summary>Makes a copy of the list, and adds the specified object to the end of the copied list.</summary>
    /// <param name="value">The object to add to the list.</param>
    /// <returns>A new list with the object added.</returns>
    public NonEmptyList<T> Add(T value) => new(_innerList.Add(value));

    /// <summary>Makes a copy of the list and adds the specified objects to the end of the copied list.</summary>
    /// <param name="items">The objects to add to the list.</param>
    /// <returns>A new list with the elements added.</returns>
    public NonEmptyList<T> AddRange(IEnumerable<T> items) => new(_innerList.AddRange(items));

    /// <summary>Inserts the specified element at the specified index in the list.</summary>
    /// <param name="index">The zero-based index at which to insert the value.</param>
    /// <param name="item">The object to insert.</param>
    /// <returns>A new list that includes the specified element.</returns>
    public NonEmptyList<T> Insert(int index, T item) => new(_innerList.Insert(index, item));

    /// <summary>Inserts the specified elements at the specified index in the list.</summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="items">The elements to insert.</param>
    /// <returns>A new list that includes the specified elements.</returns>
    public NonEmptyList<T> InsertRange(int index, IEnumerable<T> items) => new(_innerList.InsertRange(index, items));

    /// <summary>
    /// Replaces an element in the list at a given position with the specified element.
    /// </summary>
    /// <param name="index">The position in the list of the element to replace.</param>
    /// <param name="value">The element to replace the old element with.</param>
    /// <returns>The new list.</returns>
    public NonEmptyList<T> SetItem(int index, T value) => new(_innerList.SetItem(index, value));

    /// <summary>Returns a new list with the first matching element in the list replaced with the specified element.</summary>
    /// <param name="oldValue">The element to be replaced.</param>
    /// <param name="newValue">The element to replace the first occurrence of <paramref name="oldValue" /> with.</param>
    /// <param name="equalityComparer">The equality comparer to use for matching <paramref name="oldValue" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="oldValue" /> does not exist in the list.</exception>
    /// <returns>A new list that contains <paramref name="newValue" />, even if <paramref name="oldValue" /> is the same as <paramref name="newValue" />.</returns>
    public NonEmptyList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) =>
        new(_innerList.Replace(oldValue, newValue, equalityComparer));

    /// <summary>
    /// Creates a shallow copy of a range of elements in the source <see cref="NonEmptyList{T}"/>.
    /// </summary>
    /// <param name="index">
    /// The zero-based <see cref="NonEmptyList{T}"/> index at which the range
    /// starts.
    /// </param>
    /// <param name="count">
    /// The number of elements in the range.
    /// </param>
    /// <returns>
    /// A shallow copy of a range of elements in the source <see cref="ImmutableList{T}"/>.
    /// </returns>
    public ImmutableList<T> GetRange(int index, int count) => _innerList.GetRange(index, count);

    /// <summary>
    /// Combines the elements of the current <see cref="NonEmptyList{T}"/> with the elements of another
    /// <see cref="NonEmptyList{TOther}"/> into a new <see cref="NonEmptyList{T}"/> of tuples.
    /// </summary>
    /// <typeparam name="TOther">The type of the elements in the other list.</typeparam>
    /// <param name="other">
    /// The other <see cref="NonEmptyList{TOther}"/> whose elements to combine with the current list.
    /// </param>
    /// <returns>
    /// A new <see cref="NonEmptyList{T}"/> of tuples where each tuple contains an element from the current list
    /// and the corresponding element from <paramref name="other"/>.
    /// </returns>
    /// <remarks>
    /// The resulting list has the same number of elements as the shorter of the two input lists.
    /// </remarks>
    public NonEmptyList<(T, TOther)> Zip<TOther>(NonEmptyList<TOther> other) =>
        new(_innerList.Zip(other._innerList).ToImmutableList());

    /// <summary>
    /// Combines the elements of the current <see cref="NonEmptyList{T}"/> with the elements of another
    /// <see cref="NonEmptyList{TOther}"/> into a new <see cref="NonEmptyList{T}"/> by applying a specified function.
    /// </summary>
    /// <typeparam name="TOther">The type of the elements in the other list.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the resulting list.</typeparam>
    /// <param name="other">
    /// The other <see cref="NonEmptyList{TOther}"/> whose elements to combine with the current list.
    /// </param>
    /// <param name="selector">
    /// A function that specifies how to combine corresponding elements from the two lists into a single result element.
    /// </param>
    /// <returns>
    /// A new <see cref="NonEmptyList{T}"/> whose elements are the result of invoking <paramref name="selector"/>
    /// on each pair of corresponding elements from the current list and <paramref name="other"/>.
    /// </returns>
    /// <remarks>
    /// The resulting list has the same number of elements as the shorter of the two input lists.
    /// </remarks>
    public NonEmptyList<TResult> Zip<TOther, TResult>(NonEmptyList<TOther> other, Func<T, TOther, TResult> selector) =>
        new(_innerList.Zip(other._innerList, selector).ToImmutableList());

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => _innerList.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
