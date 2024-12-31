using System.Runtime.CompilerServices;

namespace Prelude;

/// <summary>
/// Provides extension methods for applying Option functions.
/// </summary>
public static class OptionApplicativeExtensions
{
    private static Option<TResult> ApplyInternal<T1, T2, TResult>(
        Option<T1> apply,
        Option<T2> value,
        Func<T1, T2, TResult> resultSelector)
    {
        return (apply, value) switch
        {
            ({ IsSome: true } f, { IsSome: true } v) =>
                Option.Some(resultSelector(f.UnsafeValue, v.UnsafeValue)),
            _ => Option.None<TResult>()
        };
    }

    /// <summary>
    /// Applies a function to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> Apply<T1, TResult>(
        this Option<Func<T1, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) => f(v));

    /// <summary>
    /// Applies a function with two parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, TResult>> Apply<T1, T2, TResult>(
        this Option<Func<T1, T2, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2) => f(v, t2);
            return g;
        });

    /// <summary>
    /// Applies a function with three parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, TResult>> Apply<T1, T2, T3, TResult>(
        this Option<Func<T1, T2, T3, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
    {
        var g = (T2 t2, T3 t3) => f(v, t2, t3);
        return g;
    });

    /// <summary>
    /// Applies a function with four parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, T4, TResult>> Apply<T1, T2, T3, T4, TResult>(
        this Option<Func<T1, T2, T3, T4, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4) => f(v, t2, t3, t4);
            return g;
        });

    /// <summary>
    /// Applies a function with five parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, T4, T5, TResult>> Apply<T1, T2, T3, T4, T5, TResult>(
        this Option<Func<T1, T2, T3, T4, T5, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5) => f(v, t2, t3, t4, t5);
            return g;
        });

    /// <summary>
    /// Applies a function with six parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, T4, T5, T6, TResult>> Apply<T1, T2, T3, T4, T5, T6, TResult>(
        this Option<Func<T1, T2, T3, T4, T5, T6, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) => f(v, t2, t3, t4, t5, t6);
            return g;
        });

    /// <summary>
    /// Applies a function with seven parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, T4, T5, T6, T7, TResult>> Apply<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Option<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) => f(v, t2, t3, t4, t5, t6, t7);
            return g;
        });

    /// <summary>
    /// Applies a function with eight parameters to a value within an Option context.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Option<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> apply,
        Option<T1> value) => ApplyInternal(apply, value, (f, v) =>
        {
            var g = (T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) => f(v, t2, t3, t4, t5, t6, t7, t8);
            return g;
        });
}
