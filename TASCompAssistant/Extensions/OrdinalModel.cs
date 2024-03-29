﻿#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: OrdinalModel.cs
// 
// Current Data:
// 2019-08-13 8:50 PM
// 
// Creation Date:
// 2019-08-13 8:50 PM

#endregion

namespace TASCompAssistant.Extensions
{
    internal static class OrdinalModel
    {
        /// <summary>
        ///     Takes an integer number and returns a string of the number with the ordinal (e.g. 1 => 1st, 2 => 2nd, 3 => 3rd)
        /// </summary>
        /// <param name="num"></param>
        /// <returns>Returns string</returns>
        public static string FormatOrdinal(this int num)
        {
            if (num <= 0)
            {
                return num.ToString();
            }

            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num + "st";
                case 2:
                    return num + "nd";
                case 3:
                    return num + "rd";
                default:
                    return num + "th";
            }
        }
    }
}