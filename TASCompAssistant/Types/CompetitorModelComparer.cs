#region Title Header

// Name: Phillip Smith
// 
// Solution: TASCompAssistant
// Project: TASCompAssistant
// File Name: CompetitorModelComparer.cs
// 
// Current Data:
// 2019-07-22 5:30 PM
// 
// Creation Date:
// 2019-06-23 8:30 PM

#endregion

using System.Collections.Generic;
using TASCompAssistant.Models;

namespace TASCompAssistant.Types
{
    internal sealed class CompetitorModelComparer : IComparer<CompetitorModel>
    {
        public int Compare(CompetitorModel x, CompetitorModel y)
        {
            if (x.DQ == y.DQ)
            {
                if (x.TimeUnitTotal == y.TimeUnitTotal)
                {
                    return 0;
                }

                return x.TimeUnitTotal < y.TimeUnitTotal ? -1 : 1;
            }

            return x.DQ == false ? -1 : 1;
        }
    }
}