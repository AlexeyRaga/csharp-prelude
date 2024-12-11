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
    /// <returns>True if the instance is a Left value, otherwise false.</returns>
    public bool IsLeft => this is Left;

    /// <summary>
    /// Determines whether the specified <see cref="Either{_,_}"/> is <see cref="Either{_,_}.Right"/>.
    /// </summary>
    /// <returns>True if the instance is a Right value, otherwise false.</returns>
    public bool IsRight => this is Right;

    /// <summary>
    /// Represents the left side of the <see cref="Either{_,_}"/> type.
    /// </summary>
    /// <param name="Value">The value contained in the Left instance.</param>
    /// <returns>A Left instance containing the specified value.</returns>
    /// <exclude />
    public sealed record Left(TLeft Value) : Either<TLeft, TRight>;

    /// <summary>
    /// Represents the right side of the <see cref="Either{_,_}"/> type.
    /// </summary>
    /// <param name="Value">The value contained in the Right instance.</param>
    /// <returns>A Right instance containing the specified value.</returns>
    /// <exclude />
    public sealed record Right(TRight Value) : Either<TLeft, TRight>;

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Left"/> case.
    /// </summary>
    /// <param name="value">The value to wrap in a Left instance.</param>
    /// <returns>A Left instance containing the specified value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> NewLeft(TLeft value) => new Left(value);

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Right"/> case.
    /// </summary>
    /// <param name="value">The value to wrap in a Right instance.</param>
    /// <returns>A Right instance containing the specified value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Either<TLeft, TRight> NewRight(TRight value) => new Right(value);

    /// <inheridoc />
    public override string ToString() => this switch
    {
        Left left => $"Left({left.Value})",
        Right right => $"Right({right.Value})",
        _ => throw new InvalidOperationException("Invalid Either case.")
    };
}

/// <summary>
/// Convenience class for creating <see cref="Either{TLeft, TRight}"/> instances.
/// </summary>
/// <typeparam name="TLeft"></typeparam>
public static class Either<TLeft>
{
    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Right"/> case.
    /// <br/>
    /// Synonym to <see cref="Either{TLeft}.Right{TRight}"/>.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="value">The value to wrap in a Right instance.</param>
    /// <returns>A Right instance containing the specified value.</returns>
    public static Either<TLeft, TRight> Pure<TRight>(TRight value) => Either.Right<TLeft, TRight>(value);

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Left"/> case.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="value">The value to wrap in a Left instance.</param>
    /// <returns>A Left instance containing the specified value.</returns>
    public static Either<TLeft, TRight> Left<TRight>(TLeft value) => Either.Left<TLeft, TRight>(value);

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Right"/> case.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="value">The value to wrap in a Right instance.</param>
    /// <returns>A Right instance containing the specified value.</returns>
    public static Either<TLeft, TRight> Right<TRight>(TRight value) => Either.Right<TLeft, TRight>(value);
}

/// <summary>
/// Provides a set of static methods for creating instances of <see cref="Either{TLeft,TRight}" alt="Foo"/> type.
/// </summary>
public static class Either
{
    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Left"/> case.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="value">The value to wrap in a Left instance.</param>
    /// <returns>A Left instance containing the specified value.</returns>
    public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft value) =>
        Either<TLeft, TRight>.NewLeft(value);

    /// <summary>
    /// Creates a new <see cref="Either{_,_}"/> value with the <see cref="Either{_,_}.Right"/> case.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="value">The value to wrap in a Right instance.</param>
    /// <returns>A Right instance containing the specified value.</returns>
    public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight value) =>
        Either<TLeft, TRight>.NewRight(value);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="leftValue">The value to wrap in a Left instance if the right value is null.</param>
    /// <param name="value">The possibly null value to convert to either Left or Right.</param>
    /// <returns>A Right instance if the value is non-null, otherwise a Left instance.</returns>
    public static Either<TLeft, TRight> FromNullable<TLeft, TRight>(TLeft leftValue, TRight? value) where TRight : class =>
        value is not null ? Right<TLeft, TRight>(value) : Left<TLeft, TRight>(leftValue);

    /// <summary>
    /// Converts possibly null value to an <see cref="Either{_,_}"/>
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="leftValue">The value to wrap in a Left instance if the right value is null.</param>
    /// <param name="value">The nullable value to convert to either Left or Right.</param>
    /// <returns>A Right instance if the value has a value, otherwise a Left instance.</returns>
    public static Either<TLeft, TRight> FromNullable<TLeft, TRight>(TLeft leftValue, TRight? value) where TRight : struct =>
        value.HasValue ? Right<TLeft, TRight>(value.Value) : Left<TLeft, TRight>(leftValue);

    /// <summary>
    /// Combines two <see cref="Either{TLeft, TRight}"/> instances by applying the specified combine function
    /// to their <see cref="Either{TLeft, TRight}.Right"/> values if both are in the "Right" state.
    /// </summary>
    /// <typeparam name="TLeft">The type of the "Left" value, representing an error or alternative state.</typeparam>
    /// <typeparam name="TRight">The type of the "Right" value, representing a successful result.</typeparam>
    /// <param name="first">
    /// The first <see cref="Either{TLeft, TRight}"/> to combine.
    /// </param>
    /// <param name="second">
    /// The second <see cref="Either{TLeft, TRight}"/> to combine.
    /// </param>
    /// <param name="combine">
    /// A function that specifies how to combine the <see cref="Either{TLeft, TRight}.Right"/> values
    /// of <paramref name="first"/> and <paramref name="second"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Either{TLeft, TRight}"/>:
    /// <list type="bullet">
    /// <item><description>
    /// If both <paramref name="first"/> and <paramref name="second"/> are in the "Right" state, returns a "Right"
    /// containing the result of the <paramref name="combine"/> function.
    /// </description></item>
    /// <item><description>
    /// If either <paramref name="first"/> or <paramref name="second"/> is in the "Left" state, returns the first "Left" encountered.
    /// </description></item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This method does not handle accumulation of "Left" values by default. If you need to accumulate errors,
    /// ensure <typeparamref name="TLeft"/> supports such operations (e.g., a list of errors).
    /// </remarks>
    public static Either<TLeft, TRight> Combine<TLeft, TRight>(
        Either<TLeft, TRight> first,
        Either<TLeft, TRight> second,
        Func<TRight, TRight, TRight> combine)
    {
        return (first, second) switch
        {
            (Either<TLeft, TRight>.Right r1, Either<TLeft, TRight>.Right r2) =>
                Right<TLeft, TRight>(combine(r1.Value, r2.Value)),
            (Either<TLeft, TRight>.Left l, _) => l,
            (_, Either<TLeft, TRight>.Left l) => l,
            _ => throw new InvalidOperationException("Unhandled case for Either.")
        };
    }

    public static Either<TLeft, TRight> Pure<TLeft, TRight>(TRight value) => Right<TLeft, TRight>(value);
}
