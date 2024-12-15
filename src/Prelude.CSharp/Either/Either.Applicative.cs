using System;

namespace Prelude;

/// <summary>
/// Provides extension methods for applying Either functions.
/// </summary>
public static class EitherApplicativeExtensions
{
    private static Either<TLeft, TResult> ApplyInternal<TLeft, T1, T2, TResult>(
        Either<TLeft, T1> apply,
        Either<TLeft, T2> value,
        Func<T1, T2, TResult> resultSelector)
    {
        return (apply, value) switch
        {
            (Either<TLeft, T1>.Right(var f), Either<TLeft, T2>.Right(var v)) =>
                Either.Right<TLeft, TResult>(resultSelector(f, v)),
            (Either<TLeft, T1>.Left(var l), _) => Either.Left<TLeft, TResult>(l),
            (_, Either<TLeft, T2>.Left(var l)) => Either.Left<TLeft, TResult>(l)
        };
    }

    /// <summary>
    /// Applies a function to a value within an Either context.
    /// </summary>
    public static Either<TLeft, TResult> Apply<TLeft, T1, TResult>(
        this Either<TLeft, Func<T1, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) => f(v));
    }

    /// <summary>
    /// Applies a function with two parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, TResult>> Apply<TLeft, T1, T2, TResult>(
        this Either<TLeft, Func<T1, T2, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2) => f(v, t2);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with three parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, TResult>> Apply<TLeft, T1, T2, T3, TResult>(
        this Either<TLeft, Func<T1, T2, T3, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3) => f(v, t2, t3);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with four parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, T4, TResult>> Apply<TLeft, T1, T2, T3, T4, TResult>(
        this Either<TLeft, Func<T1, T2, T3, T4, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4) => f(v, t2, t3, t4);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with five parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, T4, T5, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, TResult>(
        this Either<TLeft, Func<T1, T2, T3, T4, T5, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5) => f(v, t2, t3, t4, t5);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with six parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, T4, T5, T6, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, TResult>(
        this Either<TLeft, Func<T1, T2, T3, T4, T5, T6, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => f(v, t2, t3, t4, t5, t6);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with seven parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, T4, T5, T6, T7, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Either<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => f(v, t2, t3, t4, t5, t6, t7);
            return g;
        });
    }

    /// <summary>
    /// Applies a function with eight parameters to a value within an Either context.
    /// </summary>
    public static Either<TLeft, Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Either<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> apply,
        Either<TLeft, T1> value)
    {
        return ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => f(v, t2, t3, t4, t5, t6, t7, t8);
            return g;
        });
    }
}
