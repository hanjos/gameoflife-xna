using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Utilities
{
    public class ComparisonException : ApplicationException 
    {
        public ComparisonException() {}

        public ComparisonException(string message) : base(message) {}

        public ComparisonException(string message, Exception inner) : base(message, inner) {}
    }

    public class TimeSpanUtils
    {
        public static TimeSpan Max(TimeSpan a, TimeSpan b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException(a + ", " + b);
            
            if (a == b)
                return a;

            int comparison = TimeSpan.Compare(a, b);

            if(comparison == -1) // a < b
                return b;
            else if(comparison == 0 || comparison == 1) // a >= b
                return a;
            else
                throw new ComparisonException("Unknown comparison result: " + comparison);
        }

        public static TimeSpan Min(TimeSpan a, TimeSpan b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException(a + ", " + b);

            if (a == b)
                return a;

            int comparison = TimeSpan.Compare(a, b);

            if (comparison == -1) // a < b
                return a;
            else if (comparison == 0 || comparison == 1) // a >= b
                return b;
            else
                throw new ComparisonException("Unknown comparison result: " + comparison);
        }
    }
}
