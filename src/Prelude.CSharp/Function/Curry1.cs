using System.Runtime.CompilerServices;

namespace Prelude;

public static partial class Func
{
    /// <summary>
    /// Curries a function with 2 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, TResult>> Curry1<T1, T2, TResult>(this Func<T1, T2, TResult> func) =>
        t1 => t2 => func(t1, t2);

    /// <summary>
    /// Curries a function with 3 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, TResult>> Curry1<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func) =>
        t1 => (t2, t3) => func(t1, t2, t3);

    /// <summary>
    /// Curries a function with 4 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, T4, TResult>> Curry1<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func) =>
        t1 => (t2, t3, t4) => func(t1, t2, t3, t4);

    /// <summary>
    /// Curries a function with 5 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, T4, T5, TResult>> Curry1<T1, T2, T3, T4, T5, TResult>(
        this Func<T1, T2, T3, T4, T5, TResult> func) =>
        t1 => (t2, t3, t4, t5) => func(t1, t2, t3, t4, t5);

    /// <summary>
    /// Curries a function with 6 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, T4, T5, T6, TResult>> Curry1<T1, T2, T3, T4, T5, T6, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, TResult> func) =>
        t1 => (t2, t3, t4, t5, t6) => func(t1, t2, t3, t4, t5, t6);

    /// <summary>
    /// Curries a function with 7 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, T4, T5, T6, T7, TResult>> Curry1<T1, T2, T3, T4, T5, T6, T7, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func) =>
        t1 => (t2, t3, t4, t5, t6, t7) => func(t1, t2, t3, t4, t5, t6, t7);

    /// <summary>
    /// Curries a function with 8 parameters into a function of one parameter and a remaining function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, Func<T2, T3, T4, T5, T6, T7, T8, TResult>> Curry1<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func) =>
        t1 => (t2, t3, t4, t5, t6, t7, t8) => func(t1, t2, t3, t4, t5, t6, t7, t8);
}
