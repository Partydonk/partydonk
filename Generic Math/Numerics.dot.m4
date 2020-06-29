//
// Author:
//   Aaron Bockover <abock@microsoft.com>
//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

define(`with_subtext',`$1[label = <<TABLE BORDER="0"><TR><TD>$1</TD></TR><TR><TD><FONT POINT-SIZE="9" FACE="Menlo">$2</FONT></TD></TR></TABLE>>]')

digraph {
  rankdir = TB;
  bgcolor = transparent;
  stylesheet = "Numerics.dot.css";

  edge [
    dir = back;
    color = "#505050";
    arrowsize = 0.75;
    penwidth = 1.5;
  ];

  node [
    shape = box;
    style = "filled, rounded";
    fontname = "Helvetica-Bold";
    fontsize = 12;
  ];

  subgraph System {
    node [fillcolor="#FEF000"; color="#FFB900"];
    with_subtext(IOperatorEquatable, == !=);
    IEquatable;
    with_subtext(IOperatorComparable, &lt; &lt;= &gt; &gt;);
    IComparable;
  }

  subgraph Integers {
    node [fillcolor="#FF9349"; color="#D83B01"];
    with_subtext(IBinaryInteger, / % ~ &amp; | ^ &lt;&lt; &gt;&gt;);
    with_subtext(IFixedWidthInteger, BitWidth Min Max);
    IUnsignedInteger;
    ISignedInteger;
  };

  subgraph FloatingPoint {
    node [fillcolor="#50E6FF"; color="#0078D4"];
    with_subtext(IExpressibleByFloatLiteral, implicit TSelf(double));
    with_subtext(IFloatingPoint, / % NaN PI Epsilon);
    with_subtext(IBinaryFloatingPoint, ExponentBitCount SignificandBitCount);
  };

  subgraph Numerics {
		node [fillcolor="#D59DFF"; color="#8661C5"];
    IReal;
    with_subtext(IAlgebraicField, /);
    with_subtext(IRealFunctions, atan2 hypot gamma log2 log10);
    with_subtext(IElementaryFunctions, exp a?(sin|cos|tan)h? pow sqrt);
  };

  subgraph CoreNumerics {
    node [fillcolor="#30E5D0"; color="#008575"];
    with_subtext(IAdditiveArithmetic, Zero + -);
    with_subtext(IExpressibleByIntegerLiteral, implicit TSelf(int));
    with_subtext(INumeric, *);
    with_subtext(ISignedNumeric, unary -);
  };

  subgraph Structs {
    node [fillcolor="#9BF00B"; color="#107C10"];
    rank = same;
    Double[label="Single, Double"];
    Int32[label="SByte, Int16, Int32, Int64"];
    UInt32[label="Byte, UInt16, UInt32, UInt64"];
  }

  IOperatorEquatable -> IEquatable;
  IOperatorComparable -> IComparable;
  IAlgebraicField -> ISignedNumeric;
  IReal -> {
    IFloatingPoint,
    IRealFunctions,
    IAlgebraicField
  };
  IElementaryFunctions -> IAdditiveArithmetic;
  IRealFunctions -> IElementaryFunctions;
  IAdditiveArithmetic -> {
    IOperatorEquatable,
    IOperatorComparable
  };
  IExpressibleByIntegerLiteral;
  INumeric -> {
    IAdditiveArithmetic,
    IExpressibleByIntegerLiteral
  };
  ISignedNumeric -> INumeric;
  IBinaryInteger -> INumeric;
  IFixedWidthInteger -> IBinaryInteger;
  IUnsignedInteger -> IBinaryInteger;
  ISignedInteger -> {
    IBinaryInteger,
    ISignedNumeric
  };
  IExpressibleByFloatLiteral;
  IFloatingPoint -> ISignedNumeric;
  IBinaryFloatingPoint -> {
    IFloatingPoint,
    IExpressibleByFloatLiteral
  };
  Double -> {
    IBinaryFloatingPoint,
    IReal
  };
  Int32 -> {
    IFixedWidthInteger,
    ISignedInteger
  };
  UInt32 -> {
    IFixedWidthInteger,
    IUnsignedInteger
  };
}