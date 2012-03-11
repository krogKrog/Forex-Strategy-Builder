﻿// Numeric parameters
// Part of Forex Strategy Builder
// Website http://forexsb.com/
// Copyright (c) 2006 - 2012 Miroslav Popov - All rights reserved.
// This code or any part of it cannot be used in other applications without a permission.

using System;

namespace Forex_Strategy_Builder
{
    /// <summary>
    /// Describes a parameter represented by means of a NumericUpDown control.
    /// </summary>
    public class NumericParam
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public NumericParam()
        {
            Caption = String.Empty;
            Value = 0;
            Min = 0;
            Max = 100;
            Point = 0;
            Enabled = false;
            ToolTip = String.Empty;
        }

        /// <summary>
        /// Gets or sets the text describing the parameter.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the value of parameter.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets the value of parameter as a string.
        /// </summary>
        public string ValueToString
        {
            get { return String.Format("{0:F" + Point + "}", Value); }
        }

        /// <summary>
        /// Gets or sets the minimum value of parameter.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of parameter.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Gets or sets the number of meaning decimal points of parameter.
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the control can respond to user interaction.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the text of tool tip associated with this control.
        /// </summary>
        public string ToolTip { get; set; }

        /// <summary>
        /// Gets the corrected value of parameter as a string.
        /// </summary>
        public string AnotherValueToString(double anotherValue)
        {
            return String.Format("{0:F" + Point + "}", anotherValue);
        }

        /// <summary>
        /// Returns a copy
        /// </summary>
        public NumericParam Clone()
        {
            var numericParam = new NumericParam
                                   {
                                       Caption = Caption,
                                       Value = Value,
                                       Min = Min,
                                       Max = Max,
                                       Point = Point,
                                       Enabled = Enabled,
                                       ToolTip = ToolTip
                                   };


            return numericParam;
        }
    }
}