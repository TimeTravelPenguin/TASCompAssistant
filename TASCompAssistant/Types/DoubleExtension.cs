using System;

namespace TASCompAssistant.Types
{
    public static class DoubleExtension
    {
        private const double Epsilon = 1E-07;

        public static bool IsZero(this double value)
        {
            return value.IsEqualTo(0.0);
        }

        public static bool IsEqualTo(this double lhs, double rhs)
        {
            return Math.Abs(lhs - rhs) < Epsilon;
        }
    }
}