using System.Collections.Immutable;

namespace Prelude;

public partial record NonEmptyList<T>
{
    /// <summary>Removes the first occurrence of a specified object from the list.</summary>
    /// <param name="value">The object to remove from the list.</param>
    /// <param name="equalityComparer">The equality comparer to use to locate <paramref name="value" />.</param>
    /// <returns>A new list with the specified object removed.</returns>
    public Option<NonEmptyList<T>> Remove(T value, IEqualityComparer<T>? equalityComparer) =>
        _innerList.Remove(value, equalityComparer).ToNonEmptyList();

    /// <summary>Removes the first occurrence of a specified object from the list.</summary>
    /// <param name="value">The object to remove from the list.</param>
    /// <returns>A new list with the specified object removed.</returns>
    public Option<NonEmptyList<T>> Remove(T value) =>
        _innerList.Remove(value).ToNonEmptyList();

    /// <summary>Removes all the elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="match">The delegate that defines the conditions of the elements to remove.</param>
    /// <returns>A new immutable list with the elements removed.</returns>
    public Option<NonEmptyList<T>> RemoveAll(Predicate<T> match) =>
        _innerList.RemoveAll(match).ToNonEmptyList();

    /// <summary>Removes the specified object from the list.</summary>
    /// <param name="items">The objects to remove from the list.</param>
    /// <param name="equalityComparer">The equality comparer to use to determine if <paramref name="items" /> match any objects in the list.</param>
    /// <returns>A new immutable list with the specified objects removed, if <paramref name="items" /> matched objects in the list.</returns>
    public Option<NonEmptyList<T>> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) =>
        _innerList.RemoveRange(items, equalityComparer).ToNonEmptyList();

    /// <summary>Removes a range of elements from the list.</summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <returns>A new immutable list with the elements removed.</returns>
    public Option<NonEmptyList<T>> RemoveRange(int index, int count) =>
        _innerList.RemoveRange(index, count).ToNonEmptyList();

    // <summary>Removes the element at the specified index of the list.</summary>
    /// <param name="index">The index of the element to remove.</param>
    /// <returns>A new list with the element removed.</returns>
    public Option<NonEmptyList<T>> RemoveAt(int index) =>
        _innerList.RemoveAt(index).ToNonEmptyList();
}
