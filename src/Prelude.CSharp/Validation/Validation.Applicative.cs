using System.Collections.Immutable;

namespace Prelude;

/// <summary>
/// Provides extension methods for applying Validation functions.
/// </summary>
public static class ValidationApplicativeExtensions
{
    private static Either<ImmutableList<TLeft>, TResult> ApplyInternal<TLeft, T1, T2, TResult>(
        Either<ImmutableList<TLeft>, T1> value1,
        Either<ImmutableList<TLeft>, T2> value2,
        Func<T1, T2, TResult> resultSelector)
    {
        return (value1, value2) switch
        {
            (Either<ImmutableList<TLeft>, T1>.Right(var f), Either<ImmutableList<TLeft>, T2>.Right(var v)) =>
                Either.Right<ImmutableList<TLeft>, TResult>(resultSelector(f, v)),
            _ => Either.Left<ImmutableList<TLeft>, TResult>(
                value1
                    .Fold(x => x, _ => [])
                    .Concat(value2.Fold(x => x, _ => []))
                    .ToImmutableList())
        };
    }

    /// <summary>
    /// Applies a function to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, TResult> Apply<TLeft, T1, TResult>(
        this Validation<TLeft, Func<T1, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var rrr = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (x1, x2) => x1(x2));
        return new Validation<TLeft, TResult>(rrr);
    }

    /// <summary>
    /// Applies a function to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, TResult> Apply<TLeft, T1, TResult>(
        this Validation<TLeft, Func<T1, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var rrr = ApplyInternal(apply.Either, value.Either, (x1, x2) => x1(x2));
        return new Validation<TLeft, TResult>(rrr);
    }

    /// <summary>
    /// Applies a function with two parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, TResult>> Apply<TLeft, T1, T2, TResult>(
        this Validation<TLeft, Func<T1, T2, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, x) =>
        {
            var g = (T2 t) => f(x, t);
            return g;
        });

        return new Validation<TLeft, Func<T2, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with two parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, TResult>> Apply<TLeft, T1, T2, TResult>(
        this Validation<TLeft, Func<T1, T2, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, x) =>
        {
            var g = (T2 t) => f(x, t);
            return g;
        });

        return new Validation<TLeft, Func<T2, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with three parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, TResult>> Apply<TLeft, T1, T2, T3, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3) => f(t1, t2, t3);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with three parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, TResult>> Apply<TLeft, T1, T2, T3, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3) => f(t1, t2, t3);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with four parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, TResult>> Apply<TLeft, T1, T2, T3, T4, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4) => f(t1, t2, t3, t4);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with four parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, TResult>> Apply<TLeft, T1, T2, T3, T4, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4) => f(t1, t2, t3, t4);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with five parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5) => f(t1, t2, t3, t4, t5);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with five parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5) => f(t1, t2, t3, t4, t5);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with six parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => f(t1, t2, t3, t4, t5, t6);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with six parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => f(t1, t2, t3, t4, t5, t6);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with seven parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => f(t1, t2, t3, t4, t5, t6, t7);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with seven parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => f(t1, t2, t3, t4, t5, t6, t7);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with eight parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> apply,
        Either<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.SelectLeft(ImmutableList.Create), (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => f(t1, t2, t3, t4, t5, t6, t7, t8);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, T8, TResult>>(res);
    }

    /// <summary>
    /// Applies a function with eight parameters to a value within a Validation context.
    /// </summary>
    public static Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Apply<TLeft, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Validation<TLeft, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> apply,
        Validation<TLeft, T1> value)
    {
        var res = ApplyInternal(apply.Either, value.Either, (f, t1) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => f(t1, t2, t3, t4, t5, t6, t7, t8);
            return g;
        });

        return new Validation<TLeft, Func<T2, T3, T4, T5, T6, T7, T8, TResult>>(res);
    }
}
