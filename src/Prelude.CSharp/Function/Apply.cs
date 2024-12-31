using System.Runtime.CompilerServices;

namespace Prelude;

public static partial class Func
{
    /// <summary>
    /// Applies a single argument to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T2 Apply<T1, T2>(this Func<T1, T2> f, T1 x) => f(x);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3> Apply<T1, T2, T3>(this Func<T1, T2, T3> f, T1 t1) =>
        t2 => f(t1, t2);

    /// <summary>
    /// Applies two arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T3 Apply<T1, T2, T3>(this Func<T1, T2, T3> f, T1 t1, T2 t2) =>
        f(t1, t2);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3, T4> Apply<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f, T1 t1) =>
        (t2, t3) => f(t1, t2, t3);

    /// <summary>
    /// Applies the first two arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T3, T4> Apply<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f, T1 t1, T2 t2) =>
        t3 => f(t1, t2, t3);

    /// <summary>
    /// Applies three arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T4 Apply<T1, T2, T3, T4>(this Func<T1, T2, T3, T4> f, T1 t1, T2 t2, T3 t3) =>
        f(t1, t2, t3);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 t1) =>
        (t2, t3, t4) => f(t1, t2, t3, t4);

    /// <summary>
    /// Applies the first two arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 t1, T2 t2) =>
        (t3, t4) => f(t1, t2, t3, t4);

    /// <summary>
    /// Applies the first three arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T4, T5> Apply<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 t1, T2 t2, T3 t3) =>
        t4 => f(t1, t2, t3, t4);

    /// <summary>
    /// Applies four arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T5 Apply<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5> f, T1 t1, T2 t2, T3 t3, T4 t4) =>
        f(t1, t2, t3, t4);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3, T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 t1) =>
        (t2, t3, t4, t5) => f(t1, t2, t3, t4, t5);

    /// <summary>
    /// Applies the first two arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T3, T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 t1, T2 t2) =>
        (t3, t4, t5) => f(t1, t2, t3, t4, t5);

    /// <summary>
    /// Applies the first three arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 t1, T2 t2, T3 t3) =>
        (t4, t5) => f(t1, t2, t3, t4, t5);

    /// <summary>
    /// Applies the first four arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 t1, T2 t2, T3 t3, T4 t4) =>
        t5 => f(t1, t2, t3, t4, t5);

    /// <summary>
    /// Applies five arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T6 Apply<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
        f(t1, t2, t3, t4, t5);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3, T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1) =>
        (t2, t3, t4, t5, t6) => f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies the first two arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T3, T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1, T2 t2) =>
        (t3, t4, t5, t6) => f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies the first three arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1, T2 t2, T3 t3) =>
        (t4, t5, t6) => f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies the first four arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1, T2 t2, T3 t3, T4 t4) =>
        (t5, t6) => f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies the first five arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
        t6 => f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies six arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T7 Apply<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) =>
        f(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Applies the first argument to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T2, T3, T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1) =>
        (t2, t3, t4, t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies the first two arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T3, T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2) =>
        (t3, t4, t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies the first three arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2, T3 t3) =>
        (t4, t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies the first four arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2, T3 t3, T4 t4) =>
        (t5, t6, t7) => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies the first five arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) =>
        (t6, t7) => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies the first six arguments to a function and returns a new function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) =>
        t7 => f(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Applies seven arguments to a function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T8 Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8> f, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) =>
        f(t1, t2, t3, t4, t5, t6, t7);
}
