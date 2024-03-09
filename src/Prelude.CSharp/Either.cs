using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// The <see cref="Either{_, _}"/> type represents values with two possibilities:
/// a value of this type either <see cref="Left(TLeft)"/> Left a or <see cref="Right(TRight)"/>.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="Either{_,_}" /> type is sometimes used to represent a value which is either correct or an error;
/// </para>
/// <para>
/// by convention,the <see cref="Left" /> constructor is used to hold an error value
/// and the <see cref="Right" /> constructor is used to hold a correct value (mnemonic: "right" also means "correct").
/// </para>
/// </remarks>
public abstract record Either<TLeft, TRight>
{
    // there can be no more cases defined!
    private Either()
    {
    }

    /// <summary>
    /// Determines whether the specified <see cref="Either{_,_}"/> is <see cref="Either{_,_}.Left"/>.
    /// </summary>
    public bool IsLeft => this is Left;

    /// <summary>
    /// Determines whether the specified <see cref="Either{_,_}"/> is <see cref="Either{_,_}.Right"/>.
    /// </summary>
    public bool IsRight => this is Right;

    /// <summary>
    /// Represents the left side of the <see cref="Either{_,_}"/> type.
    /// </summary>
    public sealed record Left(TLeft Value) : Either<TLeft, TRight>;

    /// <summary>
    /// Represents the right side of the <see cref="Either{_,_}"/> type.
    /// </summary>
    public sealed record Right(TRight Value) : Either<TLeft, TRight>;

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Left"/> case.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> NewLeft(TLeft value) => new Left(value);

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Right"/> case.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> NewRight(TRight value) => new Right(value);
}

/// <summary>
/// Provides a set of static methods for creating instances of <see cref="Either{_,_}"/> type.
/// </summary>
public static class Either
{
    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Left"/> case.
    /// </summary>
    public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) =>
        Either<TLeft, TRight>.NewLeft(value);
    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Right"/> case.
    /// </summary>
    public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) =>
        Either<TLeft, TRight>.NewRight(value);
}
