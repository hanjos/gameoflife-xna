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

    public class TypeMismatchException : ApplicationException
    {
        private static string BuildMessage(Type actual, params Type[] expected)
        {
            return "Expected one of " + expected + ", got " + actual;
        }

        public TypeMismatchException(Type actual, params Type[] expected) : base(BuildMessage(actual, expected)) {
            Expected = expected;
            Actual = actual;
        }

        public Type[] Expected 
        {
            get { return expected; }
            private set { expected = value; }
        }
        private Type[] expected;

        public Type Actual
        {
            get { return actual; }
            private set { actual = value; }
        }
        private Type actual;
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

    public class Either<L, R>
    {
        #region Initialization
        public Either(object value)
        {
            if (value != null && !(value is L || value is R))
                throw new TypeMismatchException(value.GetType(), typeof(L), typeof(R));

            _value = value;
        }
        #endregion

        #region Operations
        public T Get<T>()
        {
            return (T) _value;
        }
        #endregion

        #region Properties & Fields
        private object _value;
        #endregion
    }
}
