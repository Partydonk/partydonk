// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace System.Numerics
{
    #region Utility

    // Ideally we extend IEquatable<T> directly by folding
    // the members of this interface into it...
    public interface IOperatorEquatable<TSelf> :
        IEquatable<TSelf>
        where TSelf : IOperatorEquatable<TSelf>
    {
        #if HAVE_CSIM
        abstract static bool operator ==(TSelf left, TSelf right);
        abstract static bool operator !=(TSelf left, TSelf right);
        #endif
    }

    // Ideally we extend IComparable<T> directly by folding
    // the members of this interface into it...
    public interface IOperatorComparable<TSelf> :
        IEquatable<TSelf>
        where TSelf : IOperatorComparable<TSelf>
    {
        #if HAVE_CSIM
        abstract static bool operator <(TSelf left, TSelf right);
        abstract static bool operator >(TSelf left, TSelf right);
        abstract static bool operator <=(TSelf left, TSelf right);
        abstract static bool operator >=(TSelf left, TSelf right);
        #endif
    }

    #endregion

    #region Numerics

    public interface IAlgebraicField<TSelf> :
        ISignedNumeric<TSelf>
        where TSelf : struct, IAlgebraicField<TSelf>
    {
        #if HAVE_CSIM
        abstract static TSelf operator /(TSelf left, TSelf right);
        #endif

        // FIXME: this is why TSelf is constrained to struct (nullable)
        TSelf? Reciprocal { get; }
        // Alternates if the struct constraint should be removed:
        // bool TryGetReciprocal(out TSelf reciprocal);
        // (bool HasValue, TSelf Value) Reciprocal { get; }
    }

    public interface IReal<TSelf, TSignedInteger> :
        IFloatingPoint<TSelf, TSignedInteger>,
        IRealFunctions<TSelf>,
        IAlgebraicField<TSelf>
        where TSelf : struct, IReal<TSelf, TSignedInteger>
        where TSignedInteger : ISignedInteger<TSignedInteger>
    {
    }

    public interface IElementaryFunctions<TSelf> :
        IAdditiveArithmetic<TSelf>
        where TSelf : IElementaryFunctions<TSelf>
    {
        // exp, expMinusOne, a?(sin|cos|tan)h?, log, logOnePlus, pow, sqrt, root, ...
    }

    public interface IRealFunctions<TSelf> :
        IElementaryFunctions<TSelf>
        where TSelf : IRealFunctions<TSelf>
    {
        // atan2, erf, erfc, exp2, exp10, hypot, gamma, log2, log10, logGamma, signGamma, ...
    }

    #endregion

    #region Integers (https://github.com/apple/swift/blob/master/stdlib/public/core/Integers.swift)

    public interface IAdditiveArithmetic<TSelf> :
        IOperatorEquatable<TSelf>,
        IOperatorComparable<TSelf>
        where TSelf : IAdditiveArithmetic<TSelf>
    {
        #if HAVE_CSIM
        abstract static TSelf Zero { get; }
        abstract static TSelf operator +(TSelf left, TSelf right);
        abstract static TSelf operator -(TSelf left, TSelf right);
        #endif
    }

    public interface IExpressibleByIntegerLiteral<TSelf>
        where TSelf : IExpressibleByIntegerLiteral<TSelf>
    {
        #if HAVE_CSIM
        abstract static implicit operator TSelf(int value);
        #endif
    }

    public interface INumeric<TSelf> :
        IAdditiveArithmetic<TSelf>,
        IExpressibleByIntegerLiteral<TSelf>
        where TSelf : INumeric<TSelf>
    {
        TSelf Magnitude { get; }

        #if HAVE_CSIM
        abstract static TSelf operator *(TSelf left, TSelf right);
        #endif
    }

    public interface ISignedNumeric<TSelf> : 
        INumeric<TSelf>
        where TSelf : ISignedNumeric<TSelf>
    {
        #if HAVE_CSIM
        abstract static TSelf operator -(TSelf value);
        #endif
    }

    public interface IBinaryInteger<TSelf> :
        INumeric<TSelf>
        where TSelf : IBinaryInteger<TSelf>
    {
        bool IsSigned { get; }
        IReadOnlyList<uint> Words { get; }
        uint LowWord { get; }
        int BitWidth { get; }
        int TrailingZeroBitCount { get; }
        (TSelf Quotient, TSelf Remainder) QuotientAndRemainder { get; }
        bool IsMultipleOf(TSelf other);
        TSelf Signum { get; }

        #if HAVE_CSIM
        abstract static TSelf operator /(TSelf left, TSelf right);
        abstract static TSelf operator %(TSelf left, TSelf right);

        abstract static TSelf operator ~(TSelf value);
        abstract static TSelf operator &(TSelf left, TSelf right);
        abstract static TSelf operator |(TSelf left, TSelf right);
        abstract static TSelf operator ^(TSelf left, TSelf right);
        abstract static TSelf operator >>(TSelf left, int right);
        abstract static TSelf operator <<(TSelf left, int right);
        #endif
    }

    public interface IFixedWidthInteger<TSelf> :
        IBinaryInteger<TSelf>
        where TSelf : IFixedWidthInteger<TSelf>
    {
        #if HAVE_CSIM
        abstract static new int BitWidth { get; }
        abstract static TSelf MinValue { get; }
        abstract static TSelf MaxValue { get; }
        #endif
    }

    public interface IUnsignedInteger<TSelf> :
        IBinaryInteger<TSelf>
        where TSelf : IUnsignedInteger<TSelf>
    {
    }

    public interface ISignedInteger<TSelf> :
        IBinaryInteger<TSelf>,
        ISignedNumeric<TSelf>
        where TSelf : ISignedInteger<TSelf>
    {
    }

    #endregion

    #region Floating Point (https://github.com/apple/swift/blob/master/stdlib/public/core/FloatingPoint.swift)

    public interface IExpressibleByFloatLiteral<TSelf>
        where TSelf : IExpressibleByFloatLiteral<TSelf>
    {
        #if HAVE_CSIM
        abstract static implicit operator TSelf(float value);
        #endif
    }

    public interface IFloatingPoint<TSelf, TSignedInteger> :
        ISignedNumeric<TSelf>,
        IComparable<TSelf>
        where TSelf : IFloatingPoint<TSelf, TSignedInteger>
        where TSignedInteger : ISignedInteger<TSignedInteger>
    {   
        #if HAVE_CSIM
        abstract static int Radix { get; }
        abstract static TSelf NaN { get; }
        abstract static TSelf SignalingNaN { get; }
        abstract static TSelf Infinity { get; }
        abstract static TSelf GreatestFiniteMagnitude { get; }
        abstract static TSelf PI  { get; }
        abstract static TSelf Ulp { get; }
        abstract static TSelf UlpOfOne { get; }
        abstract static TSelf LeastNormalMagnitude { get; }
        abstract static TSelf LeastNonzeroMagnitude { get; }

        abstract static TSelf operator /(TSelf left, TSelf right);
        abstract static TSelf operator %(TSelf left, TSelf right);
        #endif

        FloatingPointSign Sign { get; }
        TSignedInteger Exponent { get; }
        TSelf Significand { get; }

        bool IsNormal  { get; }
        bool IsSubnormal { get; }
        bool IsFinite { get; }
        bool IsInfinite { get; }
        bool IsZero { get; }
        bool IsNaN { get; }
        bool IsSignalingNaN { get; }
        bool IsCanonical { get; }
        FloatingPointClassification FloatingPointClass { get; }
    }

    public interface IBinaryFloatingPoint<TSelf, TSignedInteger> :
        IFloatingPoint<TSelf, TSignedInteger>,
        IExpressibleByFloatLiteral<TSelf>
        where TSelf : IBinaryFloatingPoint<TSelf, TSignedInteger>
        where TSignedInteger : ISignedInteger<TSignedInteger>
    {
        #if HAVE_CSIM
        abstract static int ExponentBitCount { get; }
        abstract static int SignificandBitCount { get; }
        #endif
    }

    public enum FloatingPointSign
    {
        Plus,
        Minus
    }

    /// <summary>
    /// The IEEE 754 floating-point classes.
    /// </summary>
    public enum FloatingPointClassification
    {
        /// <summary>
        /// A signaling NaN ("not a number").
        /// </summary>
        /// <remarks>
        /// A signaling NaN sets the floating-point exception status
        /// when used in many floating-point operations.
        /// </summary>
        SignalingNaN,

        /// <summary>
        /// A silent NaN ("not a number") value.
        /// </summary>
        QuietNaN,

        /// <summary>
        /// A value equal to <c>-infinity</c>.
        /// </summary>
        NegativeInfinity,

        /// <summary>
        /// A negative value that uses the full precision of the
        /// floating-point type.
        /// </summary>
        NegativeNormal,

        /// <summary>
        /// A negative, nonzero number that does not use the full
        /// precision of the floating-point type.
        /// </summary>
        NegativeSubnormal,

        /// <summary>
        /// A value equal to zero with a negative sign.
        /// </summary>
        NegativeZero,

        /// <summary>
        /// A value equal to zero with a positive sign.
        /// </summary>
        PositiveZero,

        /// <summary>
        /// A positive, nonzero number that does not use the full
        ///  precision of the floating-point type.
        /// </summary>
        PositiveSubnormal,

        /// <summary>
        /// A positive value that uses the full precision of the
        /// floating-point type.
        /// </summary>
        PositiveNormal,

        /// <summary>
        /// A value equal to <c>+infinity</c>.
        /// </summary>
        PositiveInfinity
    }

    #endregion
}
