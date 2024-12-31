namespace Prelude;

/// <summary>
/// Provides extension methods for chaining function calls.
/// </summary>
public static partial class ThenExtensions
{
    /// <summary>
    /// Applies a function to the given object.
    /// </summary>
    public static TResult Then<TSelf, TResult>(this TSelf self, Func<TSelf, TResult> f) =>
        f(self);

    /// <summary>
    /// Applies a function with one additional parameter to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, TResult>(this TSelf self, Func<P1, TSelf, TResult> f, P1 p1) =>
        f(p1, self);

    /// <summary>
    /// Applies a function with two additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, TResult>(this TSelf self, Func<P1, P2, TSelf, TResult> f, P1 p1, P2 p2) =>
        f(p1, p2, self);

    /// <summary>
    /// Applies a function with three additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, TResult>(this TSelf self, Func<P1, P2, P3, TSelf, TResult> f, P1 p1, P2 p2, P3 p3) =>
        f(p1, p2, p3, self);

    /// <summary>
    /// Applies a function with four additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, P4, TResult>(this TSelf self, Func<P1, P2, P3, P4, TSelf, TResult> f, P1 p1, P2 p2, P3 p3, P4 p4) =>
        f(p1, p2, p3, p4, self);

    /// <summary>
    /// Applies a function with five additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, P4, P5, TResult>(this TSelf self, Func<P1, P2, P3, P4, P5, TSelf, TResult> f, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5) =>
        f(p1, p2, p3, p4, p5, self);

    /// <summary>
    /// Applies a function with six additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, P4, P5, P6, TResult>(this TSelf self, Func<P1, P2, P3, P4, P5, P6, TSelf, TResult> f, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6) =>
        f(p1, p2, p3, p4, p5, p6, self);

    /// <summary>
    /// Applies a function with seven additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, P4, P5, P6, P7, TResult>(this TSelf self, Func<P1, P2, P3, P4, P5, P6, P7, TSelf, TResult> f, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7) =>
        f(p1, p2, p3, p4, p5, p6, p7, self);

    /// <summary>
    /// Applies a function with eight additional parameters to the given object.
    /// </summary>
    public static TResult Then<TSelf, P1, P2, P3, P4, P5, P6, P7, P8, TResult>(this TSelf self, Func<P1, P2, P3, P4, P5, P6, P7, P8, TSelf, TResult> f, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, P6 p6, P7 p7, P8 p8) =>
        f(p1, p2, p3, p4, p5, p6, p7, p8, self);
}
