#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: DoubleExtension.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-07-22 11:32 AM

#endregion

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