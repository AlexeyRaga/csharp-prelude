using System.Collections;

namespace Prelude;

public partial record NonEmptyList<T>
{
    /// <summary>
    /// Copies the entire <see cref="NonEmptyList{T}"/> to a compatible one-dimensional
    /// array, starting at the beginning of the target array.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional <see cref="Array"/> that is the destination of the elements
    /// copied from <see cref="NonEmptyList{T}"/>. The <see cref="Array"/> must have
    /// zero-based indexing.
    /// </param>
    public void CopyTo(T[] array) => _innerList.CopyTo(array);

    /// <inheritdoc />
    public void CopyTo(Array array, int index) => ((ICollection)_innerList).CopyTo(array, index);

    /// <summary>
    /// Copies a range of elements from the <see cref="NonEmptyList{T}"/> to
    /// a compatible one-dimensional array, starting at the specified index of the
    /// target array.
    /// </summary>
    /// <param name="index">
    /// The zero-based index in the source <see cref="NonEmptyList{T}"/> at
    /// which copying begins.
    /// </param>
    /// <param name="array">
    /// The one-dimensional <see cref="Array"/> that is the destination of the elements
    /// copied from <see cref="NonEmptyList{T}"/>. The <see cref="Array"/> must have
    /// zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    /// <param name="count">The number of elements to copy.</param>
    public void CopyTo(int index, T[] array, int arrayIndex, int count) => _innerList.CopyTo(index, array, arrayIndex, count);

    /// <summary>
    /// Copies the entire <see cref="NonEmptyList{T}"/> to a compatible one-dimensional
    /// array, starting at the specified index of the target array.
    /// </summary>
    /// <param name="array">
    /// The one-dimensional <see cref="Array"/> that is the destination of the elements
    /// copied from <see cref="NonEmptyList{T}"/>. The <see cref="Array"/> must have
    /// zero-based indexing.
    /// </param>
    /// <param name="arrayIndex">
    /// The zero-based index in array at which copying begins.
    /// </param>
    public void CopyTo(T[] array, int arrayIndex) => _innerList.CopyTo(array, arrayIndex);
}
